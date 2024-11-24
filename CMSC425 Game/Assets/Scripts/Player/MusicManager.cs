using UnityEngine;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    // References to the audio sources for each element
    public AudioSource neutralTrack;
    public AudioSource fireTrack;
    public AudioSource iceTrack;
    public AudioSource airTrack;

    public float fadeSpeed = 0.25f; // Speed at which tracks fade in and out
    public float maxVolume = 1f; // Maximum volume of tracks

    public GunController.Element currentElement;

    private GunController gunController; // Reference to the GunController script
    private Dictionary<GunController.Element, AudioSource> elementTracks;

    private void Start()
    {
        // Find the GunController in the scene
        
        if (gunController == null)
        {
            Debug.LogError("GunController not found in the scene!");
            return;
        }

        // Initialize the dictionary to map elements to audio sources
      

        foreach (var track in elementTracks.Values)
        {
            Debug.Log("Music Starting");
            track.loop = true; // Ensure looping
            track.Play();      // Start playing the track
            track.volume = 0f; // Start muted
        }

        // Set the starting track to max volume
        if (elementTracks.ContainsKey(GunController.Element.Default))
        {
            elementTracks[GunController.Element.Default].volume = maxVolume;
        }
    }

    private void Update()
    {
        gunController = GetComponentInChildren<GunController>();
        if (gunController == null) return;

        elementTracks = new Dictionary<GunController.Element, AudioSource>
        {
            { GunController.Element.Default, neutralTrack },
            { GunController.Element.Fire, fireTrack },
            { GunController.Element.Ice, iceTrack },
            { GunController.Element.Air, airTrack }
        };

        // Get the current element from the GunController
        currentElement = gunController.currElement;

        // Gradually fade the volume of all tracks
        foreach (var kvp in elementTracks)
        {
            if (kvp.Key == currentElement)
            {
                // Fade in the active track
                kvp.Value.volume = Mathf.Min(kvp.Value.volume + fadeSpeed * Time.deltaTime, maxVolume);
            }
            else
            {
                // Fade out inactive tracks
                kvp.Value.volume = Mathf.Max(kvp.Value.volume - fadeSpeed * Time.deltaTime, 0f);
            }
        }
    }
}
