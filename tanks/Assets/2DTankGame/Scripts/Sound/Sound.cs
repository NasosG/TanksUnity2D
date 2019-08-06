using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    //sound name
    public string name;

    //audio clip
    public AudioClip clip;

    //sound's volume which goes from 0 to 1
    [Range(0f, 1f)]
    public float volume;

    //sound's pitch which goes from 0 to 3
    [Range(0f, 3f)]
    public float pitch;

    //do we want the sound to loop?
    public bool loop;

    [HideInInspector]  // we don't want it to show up in the inspector
    //add audio source
    public AudioSource source;
}
