using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Audio_randomizer : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] Musics;
    [SerializeField]
    private AudioSource source;
    private List<AudioClip> MusicsNotPlayed;
    bool started = false;

    private void Start()
    {
        MusicsNotPlayed = Musics.ToList();
        GetRandom();
        source.Play();
        started = true;
    }

    private void GetRandom()
    {
        if(Musics != null)
        {
            int count = MusicsNotPlayed.Count;
            if(count <= 0)
            {
                MusicsNotPlayed = Musics.ToList();
            }
            int random = UnityEngine.Random.Range(0, count-1);
            source.clip = MusicsNotPlayed[random];
            MusicsNotPlayed.Remove(source.clip);
        }
    }

    private void Update()
    {
        if (!source.isPlaying && started)
        {
            GetRandom();
            source.Play();
        }
    }
}
