using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject posMenu;
    public GameObject muro;

    // Start is called before the first frame update
    void Start()
    {
        muro.active = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey("up")) {
            this.transform.position = new Vector3(posMenu.transform.position.x, posMenu.transform.position.y, posMenu.transform.position.z);
            muro.active = true;
        }
    }
}
