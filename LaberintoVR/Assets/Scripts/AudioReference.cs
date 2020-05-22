using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReference : MonoBehaviour
{
    public GameObject audio;

    public void play()
    {
        audio.SetActive(true);
    }
}
