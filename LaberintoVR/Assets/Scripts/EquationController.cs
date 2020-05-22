using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquationController : MonoBehaviour
{
    //Variables Locales
    public string correctCode="-1";
    public static string playerCode = "";
    public int totalDigits = 0;
    public bool equationSolved = false;
    public TextMeshPro text;
    public GameObject bulb;
    public TextMesh displayCodeTextMesh;
    public GameObject panel;
    public GameObject a0;
    public GameObject a1;
    public GameObject a2;
    public GameObject a3;
    public GameObject a4;
    public GameObject a5;
    public GameObject a6;
    public GameObject a7;
    public GameObject a8;
    public GameObject a9;
    public GameObject SelectButton;
    public AudioClip victoria, derrota;
    public GameObject b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, bc;
    private Vector3Int values;
    private int value1, value2, value3, finalValue;
    public bool active = false;
    [SerializeField] private Light lampara;

    // Start is called before the first frame update
    void Start()
    {
        values = new Vector3Int(Random.Range(1,9),Random.Range(1,9),Random.Range(1,9));
        value1 = 3*values.x;
        value2 = values.x + (2*values.y);
        value3 = values.y - values.z;
        finalValue = values.x + values.y + values.z;
        correctCode = ""+finalValue;
        text.text = "   +    +    = "+value1+"\n   +    +    = "+value2+"\n   -           = "+value3+"\n   +    +    = ??";
        //Debug.Log("X = " + value1 + ", Y = " + value2 + ", Z = " + value3);
    }

    public void switchLampara(bool act)
    {
        if (act)
        {
            lampara.intensity = 1;
            //Debug.Log("Encendido");
        }
        else
        {
            lampara.intensity = 0;
            //Debug.Log("Apagado");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (totalDigits > 2) {
            panel.GetComponent<AudioSource>().clip = derrota;
            panel.SetActive(true);
            restart();
        }

        display();
    }

    public void playAudio(string name)
    {
        switch (name)
        {
            case "0":
                a0.SetActive(true);
                break;
            case "1":
                a1.SetActive(true);
                break;
            case "2":
                a2.SetActive(true);
                break;
            case "3":
                a3.SetActive(true);
                break;
            case "4":
                a4.SetActive(true);
                break;
            case "5":
                a5.SetActive(true);
                break;
            case "6":
                a6.SetActive(true);
                break;
            case "7":
                a7.SetActive(true);
                break;
            case "8":
                a8.SetActive(true);
                break;
            case "9":
                a9.SetActive(true);
                break;
            case "SelectButton":
                SelectButton.SetActive(true);
                break;
        }
    }

    public void selectEquation()
    {
        if (playerCode == correctCode && equationSolved==false)
        {
            panel.GetComponent<AudioSource>().clip = victoria;
            panel.SetActive(true);
            b0.tag = "Untagged";
            b1.tag = "Untagged";
            b2.tag = "Untagged";
            b3.tag = "Untagged";
            b4.tag = "Untagged";
            b5.tag = "Untagged";
            b6.tag = "Untagged";
            b7.tag = "Untagged";
            b8.tag = "Untagged";
            b9.tag = "Untagged";
            bc.tag = "Untagged";
            equationSolved = true;
            changeColor();
        }
        else {
            panel.GetComponent<AudioSource>().clip = derrota;
            panel.SetActive(true);
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
//        GameObject.Find("DisplayCode").GetComponent<TextMesh>().text = playerCode;
        displayCodeTextMesh.text = playerCode;
    }

    void restart() {

        totalDigits = 0;
        playerCode = "";
    }

    public void changeColor()
    {
        //GameObject bulb = GameObject.Find("Bulb");
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
            color.g = 0.5f;
            color.b = 0.5f;
            cubo.transform.GetComponent<Renderer>().material.color = color;
            yield return new WaitForSeconds(time);
            color.r = defaultR;
            color.g = defaultG;
            color.b = defaultB;
            cubo.transform.GetComponent<Renderer>().material.color = color;
        }
        else if (totalDigits == 2) {

            color.r = 0.5f;
            color.g = 1.0f;
            color.b = 0.5f;
            cubo.transform.GetComponent<Renderer>().material.color = color;
            yield return new WaitForSeconds(time);
            color.r = defaultR;
            color.g = defaultG;
            color.b = defaultB;
            cubo.transform.GetComponent<Renderer>().material.color = color;
        }

       
    }
}
