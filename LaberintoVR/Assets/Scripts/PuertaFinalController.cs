using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaFinalController : MonoBehaviour
{
    public SimonDiceController simonDice;
    public NPuzzleController NPController;
    public EquationController EqController;

    private bool destroyed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!(destroyed)  && (simonDice.win) && (NPController.completo) && (EqController)) {
            destroyed = true;
            this.gameObject.SetActive(false);
        }
    }
}
