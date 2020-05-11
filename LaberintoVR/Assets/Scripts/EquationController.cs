using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquationController : MonoBehaviour
{
    //Variables Locales
    public string correctCode="-1";
    public static string playerCode = "";
    public int totalDigits = 0;
    public bool equationSolved = false;
    public Text text;
    public GameObject bulb;
    public TextMesh displayCodeTextMesh;
    public AudioSource audioSource;
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
        Debug.Log("X = " + value1 + ", Y = " + value2 + ", Z = " + value3);
    }

    public void switchLampara(bool act)
    {
        if (act)
        {
            lampara.intensity = 1;
            Debug.Log("Encendido");
        }
        else
        {
            lampara.intensity = 0;
            Debug.Log("Apagado");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (totalDigits > 2) {
            audioSource.clip = derrota;
            audioSource.Play();
            restart();
        }

        display();
    }

    public void selectEquation()
    {
        if (playerCode == correctCode && equationSolved==false)
        {
            audioSource.clip = victoria;
            audioSource.Play();
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
            Debug.Log("gg");
            equationSolved = true;
            changeColor();
        }
        else {
            audioSource.clip = derrota;
            audioSource.Play();
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
