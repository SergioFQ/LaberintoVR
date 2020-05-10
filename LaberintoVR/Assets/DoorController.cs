using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject leftDoor, centerDoor, rightDoor, leftKnob, centerKnob, rightKnob;
    // Start is called before the first frame update
    public int[] doorState;
    void Start()
    {
        doorState = new int[3];
    }

    // Update is called once per frame
    void Update()
    {
        if (doorState[0] > 0 && doorState[0] < 90) {
            leftDoor.transform.RotateAround(new Vector3(1,0,0),new Vector3(1,0,0), 1);
            leftKnob.tag = "Untagged";
            doorState[0]++;
        }
        if (doorState[1] > 0 && doorState[1] < 90) {
            centerDoor.transform.RotateAround(new Vector3(1,0,0),new Vector3(1,0,0), 1);
            centerKnob.tag = "Untagged";
            doorState[1]++;
        }
        if (doorState[2] > 0 && doorState[2] < 90) {
            rightDoor.transform.RotateAround(new Vector3(1,0,0),new Vector3(1,0,0), 1);
            rightKnob.tag = "Untagged";
            doorState[2]++;
        }
    }

    public void awakeDoor(int door) {
        if (doorState[door] == 0) {
            doorState[door] = 1;
        }
    }
}
