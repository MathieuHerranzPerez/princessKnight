using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private Sound[] listSound = new Sound[0];

    // ---- INTERN ----
    private AudioSource audioSource;
    private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();

    void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("More than 1 SoundManager");
        }
        Instance = this;

        audioSource = GetComponent<AudioSource>();

        foreach(Sound s in listSound)
        {
            soundDictionary.Add(s.name, s);
        }
    }


    public void Play(Sound sound)
    {
        if(sound.source != null)
        {
            sound.source.PlayOneShot(sound.clip, sound.volume);
        }
        else
        {
            audioSource.PlayOneShot(sound.clip, sound.volume);
        }
    }

    public void Play(string name)
    {
        Sound s = soundDictionary[name];
        if (s != null)
        {
            if (s.source != null)
            {
                s.source.PlayOneShot(s.clip, s.volume);
            }
            else
            {
                audioSource.PlayOneShot(s.clip, s.volume);
            }
        }

        else
            Debug.Log("Sound " + name + " doesn't exist");
    }

    public void Stop(string name)
    {
        Sound s = soundDictionary[name];
        if (s != null)
        {
            if (s.source != null)
            {
                s.source.Stop();
            }
        }
    }
}
