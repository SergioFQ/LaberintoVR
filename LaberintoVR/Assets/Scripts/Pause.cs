using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject posMenu;
    public GameObject sala;
    public GameObject player;

    private Vector3 prePlayerPos;
    // Start is called before the first frame update
    void Start()
    {
        sala.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyUp("up")) {
            prePlayerPos = player.transform.position;
            player.transform.position = new Vector3(posMenu.transform.position.x, posMenu.transform.position.y, posMenu.transform.position.z);
            sala.SetActive(true);
        }/*

        if (Input.GetKeyUp("down"))
        {
            player.transform.position = prePlayerPos;
            sala.SetActive(false);
        }*/
    }

    public void resumeGame()
    {

        player.transform.position = prePlayerPos;
        sala.SetActive(false);
    }

}
