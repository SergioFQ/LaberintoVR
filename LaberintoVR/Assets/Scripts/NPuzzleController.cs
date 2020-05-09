using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPuzzleController : MonoBehaviour
{

    public GameObject pieza1, pieza2, pieza3, pieza4, pieza5, pieza6, pieza7, pieza8, pieza9, piezaReset;

    public Text texto1, texto2, texto3, texto4, texto5, texto6, texto7, texto8, texto9;
    public Material doneMaterial;
    private int[,] nPuzzle, nPuzzleGenerado;
    private GameObject[,] piezas;
    private Text[,] textos;
    private bool completo = false;
    // Start is called before the first frame update
    void Start()
    {
        //inicializar las piezas
        //nPuzzle = new int[3, 3];
        piezas = new GameObject[3, 3];
        textos = new Text[3,3];

        piezas[0,0] = pieza1;
        piezas[1,0] = pieza2;
        piezas[2,0] = pieza3;
        piezas[0,1] = pieza4;
        piezas[1,1] = pieza5;
        piezas[2,1] = pieza6;
        piezas[0,2] = pieza7;
        piezas[1,2] = pieza8;
        piezas[2,2] = pieza9;

        textos[0,0] = texto1;
        textos[1,0] = texto2;
        textos[2,0] = texto3;
        textos[0,1] = texto4;
        textos[1,1] = texto5;
        textos[2,1] = texto6;
        textos[0,2] = texto7;
        textos[1,2] = texto8;
        textos[2,2] = texto9;

        //List<int> listaRandom = new List<int>();
        //while (esCompleto() || !esSolucionable()) {
            nPuzzle = new int[3, 3];
            //listaRandom.Clear();
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    nPuzzle[i,j] = int.Parse(textos[i,j].text);
                    //listaRandom.Add(i+j*3);
                }
            }
            /*for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    int indice = Random.Range(0,listaRandom.Count);
                    nPuzzle[i,j] = listaRandom[indice];
                    listaRandom.RemoveAt(indice);
                }
            }*/
        //}
        nPuzzleGenerado = (int[,])nPuzzle.Clone();
        //Cambiar eso
        //Hacer que empiece resuelto y moverlo aleatoriamente 12 veces
        int movimientos = 0;
        int ultimaPieza = -1;
        while (movimientos < 10) {
            int x = Random.Range(0,3);
            int y = Random.Range(0,3);
            //Debug.Log("probamos con la pieza " + x + ", " + y);
            if (piezaMovible(x,y,true)) {
                //Debug.Log("esto se puede mover");
                if (ultimaPieza != -1) {
                    if (ultimaPieza != nPuzzle[x,y]) {
                        ultimaPieza = nPuzzle[x,y];
                        moverPieza(x,y,true);
                        movimientos++;
                        Debug.Log("movemos " + ultimaPieza);
                    }
                } else {
                        ultimaPieza = nPuzzle[x,y];
                        moverPieza(x,y,true);
                        movimientos++;
                        Debug.Log("movemos " + ultimaPieza);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!juegoTerminado()) {
            //Pintar numericos
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (piezaMovible(i,j,true)) {
                        piezas[i,j].tag = "NPuzzle";
                    } else {
                        piezas[i,j].tag = "Untagged";
                    }
                    if (nPuzzle[i,j] == 0) {
                        piezas[i,j].SetActive(false);
                        textos[i,j].text = "";
                    } else {
                        piezas[i,j].SetActive(true);
                        textos[i,j].text = nPuzzle[i,j].ToString();
                    }
                }
            }

            //Comprobar si es completo
            if (esCompleto()) {
                completo = true;
                foreach (GameObject g in piezas) {
                    g.tag = "Untagged";
                    g.GetComponent<Renderer>().material = doneMaterial;
                }
                piezaReset.tag = "Untagged";
                piezaReset.GetComponent<Renderer>().material = doneMaterial;
            }
        }
    }

    bool esCompleto() {
        if (nPuzzle == null) {
            //Debug.Log("jaja no");
            return true;
        }
        if ((nPuzzle[0,0] == 1) && (nPuzzle[1,0] == 2) && (nPuzzle[2,0] == 3) && (nPuzzle[0,1] == 8) && (nPuzzle[1,1] == 0) && (nPuzzle[2,1] == 4) && (nPuzzle[0,2] == 7) && (nPuzzle[1,2] == 6) && (nPuzzle[2,2] == 5)) {
            return true;
        } else {
            return false;
        }
        /*if (nPuzzle[0,0] == 1 && nPuzzle[1,0] == 2 && nPuzzle[1,0] == 3 && nPuzzle[0,1] == 8 && nPuzzle[1,1] == 0 && nPuzzle[2,1] == 4 && nPuzzle[0,2] == 7 && nPuzzle[1,2] == 6 && nPuzzle[2,2] == 5) {
            //Debug.Log("buena");
            return true;
        } else {
            //Debug.Log("será que no");
            return false;
        }*/
    }

    int numInversiones(int[,] nPuzzle) { 
        int inversiones = 0; 
        for (int i = 0; i < 3 - 1; i++) 
            for (int j = i + 1; j < 3; j++) 
                if (nPuzzle[j,i] > 0 && nPuzzle[j,i] > nPuzzle[i,j]) 
                    inversiones++; 
        return inversiones; 
    } 
    
    bool esSolucionable() { 
        if (nPuzzle == null) {
            return false;
        }
        int inversiones = numInversiones(nPuzzle); 
        return (inversiones % 2 == 0); 
    } 


    public bool piezaMovible(float inX, float inY, bool directo) {
        int x = -1, y = -1;
        if (directo) {
            x = (int)inX;
            y = (int)inY;
        } else {
            //Debug.Log("estamos dentro");
            //Debug.Log(inY);
            switch (inX) {
                case -0.3f:
                x = 0;
                break;
                case 0f:
                x = 1;
                break;
                case 0.3f:
                x = 2;
                break;
            }
            if (inY > 0.2) {
                y = 0;
            } else if (inY > 0.05) {
                y = 1;
            } else {
                y = 2;
            }
            /*switch (inY) {
                case 0.38f:
                y = 0;
                break;
                case 0.14f:
                y = 1;
                break;
                case -0.1f:
                y = 2;
                break;
            }*/
            //Debug.Log("comprobando ("+x+","+y+"): ("+inX+"/"+inY+") (" + inY + "==0.38f) " + (inY.Equals(0.38f)) + "(" + inY + "==0.14f) " + (inY.Equals(0.14f)) + "(" + inY + "==-0.1f) " + (inY.Equals(-0.1f)));
            if (x == -1 || y == -1) {
                return false;
            }
        }//if (!directo) Debug.Log("comprobando ("+x+","+y+"): ("+inX+","+inY+")");
        //Debug.Log("comprobando ("+x+","+y+")");
        if (x > 0) {
            if (nPuzzle[x-1,y] == 0) {
                return true;
            }
        }
        if (x < 2) {
            if (nPuzzle[x+1,y] == 0) {
                return true;
            }
        }
        if (y > 0) {
            if (nPuzzle[x,y-1] == 0) {
                return true;
            }
        }
        if (y < 2) {
            if (nPuzzle[x,y+1] == 0) {
                return true;
            }
        }
        //Debug.Log("pues nada oye");
        return false;
    }

    public void moverPieza(float inX, float inY, bool directo) {
        int x = -1, y = -1;
        if (directo) {
            x = (int)inX;
            y = (int)inY;
        } else {
            switch (inX) {
                case -0.3f:
                x = 0;
                break;
                case 0f:
                x = 1;
                break;
                case 0.3f:
                x = 2;
                break;
            }
            if (inY > 0.2) {
                y = 0;
            } else if (inY > 0.05) {
                y = 1;
            } else {
                y = 2;
            }
            //Debug.Log("comprobando ("+x+","+y+"): ("+inX+","+inY+")");
            if (x == -1 || y == -1) {
                return;
            }
        }

        if (x > 0) {
            if (nPuzzle[x-1,y] == 0) {
                nPuzzle[x-1,y] = nPuzzle[x,y];
                nPuzzle[x,y] = 0;
            }
        }
        if (x < 2) {
            if (nPuzzle[x+1,y] == 0) {
                nPuzzle[x+1,y] = nPuzzle[x,y];
                nPuzzle[x,y] = 0;
            }
        }
        if (y > 0) {
            if (nPuzzle[x,y-1] == 0) {
                nPuzzle[x,y-1] = nPuzzle[x,y];
                nPuzzle[x,y] = 0;
            }
        }
        if (y < 2) {
            if (nPuzzle[x,y+1] == 0) {
                nPuzzle[x,y+1] = nPuzzle[x,y];
                nPuzzle[x,y] = 0;
            }
        }
    }

    public void resetear() {
        nPuzzle = (int[,])nPuzzleGenerado.Clone();
    }

    public bool juegoTerminado() {
        return completo;
    }
}