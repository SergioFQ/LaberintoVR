using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArreglarAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audio;
    private bool _needsToReset;

    void Update()
    {
        if (audio.isPlaying)
        {
            _needsToReset = true;
        }
        else
        {
            if (_needsToReset)
            {
                gameObject.SetActive(false);
                _needsToReset = false;
            }
        }
    }
}