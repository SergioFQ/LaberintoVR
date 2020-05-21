using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArreglarAudio : MonoBehaviour
{
    private int count = 0;
    private AudioSource audio;
    private void Start()
    {
        audio = this.GetComponent<AudioSource>();
    }
    void Update()
    {
        if (count < 11)
        {
            count++;
        }
        if (count == 10)
        {
            audio.bypassEffects = false;
            audio.bypassEffects = true;
        }
    }
}
