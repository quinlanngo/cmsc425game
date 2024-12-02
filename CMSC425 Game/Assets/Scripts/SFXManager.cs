using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Refrence : https://www.youtube.com/watch?v=DU7cgVsU2rM&t=770s

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    [SerializeField] private AudioSource audioObject;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySFXClip(AudioClip audioClip, Transform location, float volume)
    {
        // Log the details of the audio clip and the parameters
        Debug.Log($"Playing SFX: {audioClip.name}, " +
                  $"Position: {location.position}, " +
                  $"Volume: {volume}, " +
                  $"Clip Length: {audioClip.length}");

        // Spawn in the game object
        AudioSource soundObject = Instantiate(audioObject, location.position, Quaternion.identity);

        // Assign audio clip
        soundObject.clip = audioClip;

        // Assign volume
        soundObject.volume = volume;

        // Set the sound to 3D (spatial blend = 1 for fully 3D sound)
        soundObject.spatialBlend = 1f;  // Set to 1 for full 3D sound
        soundObject.minDistance = 1f;   // You can adjust this depending on how close the sound should be
        soundObject.maxDistance = 50f;  // The maximum distance at which the sound can be heard
        soundObject.rolloffMode = AudioRolloffMode.Linear; // You can experiment with this

        // Play sound
        soundObject.Play();

        // Get length of SFX clip
        float clipLength = soundObject.clip.length;

        // Destroy the clip after it has finished
        Destroy(soundObject.gameObject, clipLength);
    }

    public void PlaySFXClip(AudioClip[] audioClips, Transform location, float volume)
    {
        int rand = Random.Range(0, audioClips.Length);

        // Spawn in the game object
        AudioSource soundObject = Instantiate(audioObject, location.position, Quaternion.identity);

        // Assign audio clip
        soundObject.clip = audioClips[rand];

        // Assign volume
        soundObject.volume = volume;

        // Set the sound to 3D (spatial blend = 1 for fully 3D sound)
        soundObject.spatialBlend = 1f;  // Set to 1 for full 3D sound
        soundObject.minDistance = 1f;   // Adjust based on how close the sound should be
        soundObject.maxDistance = 20f;  // Adjust based on how far the sound can be heard
        soundObject.rolloffMode = AudioRolloffMode.Linear; // You can experiment with this

        // Play sound
        soundObject.Play();

        // Get length of SFX clip
        float clipLength = soundObject.clip.length;

        // Destroy the clip after it has finished
        Destroy(soundObject.gameObject, clipLength);
    }
}
