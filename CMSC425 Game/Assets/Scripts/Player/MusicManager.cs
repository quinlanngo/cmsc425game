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

    [SerializeField] private GunController gunController; // Reference to the GunController script
    private Dictionary<GunController.Element, AudioSource> elementTracks;

    private void Start()
    {
        // Initialize the dictionary to map elements to audio sources
        elementTracks = new Dictionary<GunController.Element, AudioSource>
        {
            { GunController.Element.Default, neutralTrack },
            { GunController.Element.Fire, fireTrack },
            { GunController.Element.Ice, iceTrack },
            { GunController.Element.Air, airTrack }
        };

        // Synchronize all tracks
        foreach (var track in elementTracks.Values)
        {
            track.loop = true; // Ensure looping
            track.volume = 0f; // Start muted
            track.Play();
        }

        // Ensure synchronization by setting timeSamples to 0 for all tracks
        int syncTimeSamples = 0;
        foreach (var track in elementTracks.Values)
        {
            track.timeSamples = syncTimeSamples;
        }

        // Set the starting track to max volume
        if (elementTracks.ContainsKey(GunController.Element.Default))
        {
            elementTracks[GunController.Element.Default].volume = maxVolume;
        }
    }

    private void Update()
    {
        // Ensure gunController is assigned
        if (gunController == null)
        {
            Debug.LogError("GunController reference is missing!");
            return;
        }

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
