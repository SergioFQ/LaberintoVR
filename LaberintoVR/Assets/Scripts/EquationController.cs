using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquationController : MonoBehaviour
{
    //Variables Locales
    public string correctCode="16";
    public static string playerCode = "";
    public int totalDigits = 0;
    public bool equationSolved = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (totalDigits > 2) {

            restart();
        }

        display();
    }

    public void selectEquation()
    {
        if (playerCode == correctCode && equationSolved==false)
        {
            Debug.Log("gg");
            equationSolved = true;
            changeColor();
        }
        else {
            restart();
            changeColor();
        }
    }

    // para debug
    public void imprimirNumero() {

        Debug.Log(playerCode);
    }

    public void selectNum(string num) {

        playerCode += num;
        totalDigits++;
    }

    void display()
    {
        GameObject.Find("DisplayCode").GetComponent<TextMesh>().text = playerCode;
    }

    void restart() {

        totalDigits = 0;
        playerCode = "";
    }

    public void changeColor()
    {
        GameObject bulb = GameObject.Find("Bulb");
        Color color = bulb.transform.GetComponent<Renderer>().material.color;
        float defaultR = color.r;
        float defaultG = color.g;
        float defaultB = color.b;
        bulb.transform.GetComponent<Renderer>().material.color = color;
        StartCoroutine(iluminar(color, bulb, 1.0f, defaultR, defaultG, defaultB));
    }

    private IEnumerator iluminar(Color color, GameObject cubo, float time, float defaultR, float defaultG, float defaultB)
    {
        if (!equationSolved)
        {
            color.r = 1.0f;
            color.g = defaultG;
            color.b = defaultB;
            cubo.transform.GetComponent<Renderer>().material.color = color;
            yield return new WaitForSeconds(time);
            color.r = defaultR;
            color.g = defaultG;
            color.b = defaultB;
            cubo.transform.GetComponent<Renderer>().material.color = color;
        }
        else if (totalDigits == 2) {

            color.r = defaultR;
            color.g = 1.0f;
            color.b = defaultB;
            cubo.transform.GetComponent<Renderer>().material.color = color;
            yield return new WaitForSeconds(time);
            color.r = defaultR;
            color.g = defaultG;
            color.b = defaultB;
            cubo.transform.GetComponent<Renderer>().material.color = color;
        }

       
    }
}
