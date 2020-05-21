using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorController : MonoBehaviour
{
    public SimonDiceController sdCont;
    public NPuzzleController npCont;
    public EquationController eqCont;
    public Renderer luzSD, luzNP, luzEQ;
    private int tareasCompletadas;

    // Start is called before the first frame update
    void Start()
    {
        tareasCompletadas = 0;
    }

    // Update is called once per frame
    void Update()
    {
        tareasCompletadas = 0;
        if (sdCont.win) {
            tareasCompletadas++;
            luzSD.material.color = new Color(0.5f, 1.0f, 0.5f, 1.0f);
        } else {
            luzSD.material.color = new Color(1.0f, 0.5f, 0.5f, 1.0f);
        }

        if (npCont.juegoTerminado()) {
            tareasCompletadas++;
            luzNP.material.color = new Color(0.5f, 1.0f, 0.5f, 1.0f);
        } else {
            luzNP.material.color = new Color(1.0f, 0.5f, 0.5f, 1.0f);
        }

        if (eqCont.equationSolved) {
            tareasCompletadas++;
            luzEQ.material.color = new Color(0.5f, 1.0f, 0.5f, 1.0f);
        } else {
            luzEQ.material.color = new Color(1.0f, 0.5f, 0.5f, 1.0f);
        }

        if (tareasCompletadas == 3) {
            Destroy(this.gameObject);
        }

    }
}
