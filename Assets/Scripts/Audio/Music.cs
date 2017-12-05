using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {

    public AudioClip FirstClip;

    private AudioSource _audioSourceMusic;

    void Awake()
    {
        _audioSourceMusic = GetComponent<AudioSource>();
    }

    void Start()
    {
        StartCoroutine(PlayMusic());

    }


    IEnumerator PlayMusic()
    {
        _audioSourceMusic.PlayOneShot(FirstClip);
        yield return new WaitForSeconds(FirstClip.length);
        _audioSourceMusic.Play();
    }
}
