using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // the array of our sounds
    public Sound[] sounds;
    public static AudioManager instance = null;     //Allows other scripts to call functions from SoundManager. 

    // called right before start
    // so we use this for initialization
    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Play(string name)
    {
        //find a sound in the sounds array where sound.name equals name
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) // if something goes wrong and the sound is not there
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        //play the sound
        s.source.Play();
    }

}
