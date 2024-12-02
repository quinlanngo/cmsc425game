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

    private bool allTracksFinished; // Boolean to track if all tracks have stopped

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

        // Prepare tracks: mute, synchronize, and disable looping
        int syncTimeSamples = 0;
        foreach (var track in elementTracks.Values)
        {
            track.loop = false; // Disable individual looping
            track.volume = 0f;  // Start muted
            track.timeSamples = syncTimeSamples; // Synchronize tracks
            track.Play();
        }

        // Set the default track to max volume
        if (elementTracks.ContainsKey(currentElement))
        {
            elementTracks[currentElement].volume = maxVolume;
        }

        allTracksFinished = false; // Initialize boolean state
    }

    private void Update()
    {

        /*
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player object not found! Make sure the Player has the correct tag.");
            return;
        }
        IInventoryItem gun = player.GetComponent<PlayerInventory>().GetCurrentItem();
        gunController = gun.GetComponent<GunController>();
        if (gunController == null)
        {
            Debug.LogError("GunController reference is missing!");
            return;
        } */


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

        // Check if all tracks have finished playing
        allTracksFinished = CheckIfAllTracksFinished();

        // Restart all tracks if they have all finished
        if (allTracksFinished)
        {
            RestartAllTracks();
        }
    }

    private bool CheckIfAllTracksFinished()
    {
        foreach (var track in elementTracks.Values)
        {
            if (track.isPlaying)
            {
                return false; // At least one track is still playing
            }
        }
        return true; // All tracks have finished
    }

    private void RestartAllTracks()
    {
        int syncTimeSamples = 0;
        foreach (var track in elementTracks.Values)
        {
            track.timeSamples = syncTimeSamples; // Reset track to the start
            track.Play(); // Restart the track
        }
        allTracksFinished = false; // Reset the boolean
    }
}
