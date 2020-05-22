using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SimonDiceController : MonoBehaviour
{
    private IEnumerator coroutine;
    private List<string> colores = new List<string> { "CuboAmarillo", "CuboAzul", "CuboVerde", "CuboRojo" };
    private List<int> secuenciaCreada = new List<int> { };
    private List<int> secuenciaInput = new List<int> { };
    private bool secuenciaFallada = false;
    private int numeroPrueba = 0;

    [SerializeField] private GameObject _AudioSource_SimonDice;
    [SerializeField] private AudioClip _pruebaSuperada;
    [SerializeField] private AudioClip _SecuenciaSuperada;
    [SerializeField] private GameObject _RojoSimon;
    [SerializeField] private GameObject _AmarilloSimon;
    [SerializeField] private GameObject _AzulSimon;
    [SerializeField] private GameObject _VerdeSimon;
    [SerializeField] private AudioClip _wrongSimon;
    [SerializeField] private GameObject startButton;
    public bool secuenciaActiva = false;
    public bool esperandoInput = false;
    public bool win = false;
    public bool active = false;
    [SerializeField] private Light lampara;

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

    private void Update()
    {
        if (esperandoInput)
        {
            if (secuenciaCreada.Count == secuenciaInput.Count)
            {
                esperandoInput = false;
                cambiarTagsCubos("Untagged");
                GameObject.Find("StartSimonDice").tag = "StartSimonDice";
                if (!secuenciaFallada)
                {
                    numeroPrueba++;
                    if (numeroPrueba >= 3)
                    {
                        setTexto("Mensaje_Secuencia", "");
                        setTexto("Nivel", "Trial Succed!");
                        GameObject.Find("StartSimonDice").tag = "Untagged";
                        _AudioSource_SimonDice.SetActive(true);
                        win = true;
                    }
                    else
                    {
                        setTexto("Nivel", "Sequences completed: " + numeroPrueba.ToString() + "/3");
                        setTexto("Mensaje_Secuencia", "Correct sequence!");
                        _AudioSource_SimonDice.SetActive(true);
                    }
                        
                }
                else
                {
                    numeroPrueba = 0;
                    setTexto("Nivel", "Secuencias superadas: " + numeroPrueba.ToString() + "/3");
                    setTexto("Mensaje_Secuencia", "Incorrect sequence!");
                    _AudioSource_SimonDice.SetActive(true);
                }

            }
            else
            {
                if (secuenciaFallada)
                {
                    esperandoInput = false;
                    cambiarTagsCubos("Untagged");
                    GameObject.Find("StartSimonDice").tag = "StartSimonDice";
                    numeroPrueba = 0;
                    setTexto("Nivel", "Sequences completed: " + numeroPrueba.ToString() + "/3");
                    setTexto("Mensaje_Secuencia","Incorrect sequence!");
                    _AudioSource_SimonDice.SetActive(true);
                }
            }
        }
    }

    public void setActive(bool act)
    {
        active = act;
    }

    private void setTexto(string nombre, string texto)
    {
        GameObject.Find(nombre).GetComponent<Text>().text = texto;
    }

    public void añadirASecuencia(string nombre)
    {
        secuenciaInput.Add(colores.IndexOf(nombre));
        comprobarSecuencia();
        cambiarColor(nombre);
    }

    private void comprobarSecuencia()
    {
        for (int i = 0; i < secuenciaInput.Count; i++)
        {
            if (secuenciaInput[i] != secuenciaCreada[i])
            {
                secuenciaFallada = true;
            }
        }
    }

    public void cambiarColor(string nombre)
    {
        switch (nombre)
        {
            case "CuboAmarillo":
                _AmarilloSimon.SetActive(true);
                break;
            case "CuboAzul":
                _AzulSimon.SetActive(true);
                break;
            case "CuboVerde":
                _VerdeSimon.SetActive(true);
                break;
            case "CuboRojo":
                _RojoSimon.SetActive(true);
                break;
        }

        GameObject cubo = GameObject.Find(nombre);
        Color color = cubo.transform.GetComponent<Renderer>().material.color;
        cubo.transform.GetComponent<Renderer>().material.color = color;
        StartCoroutine(iluminar(color, cubo, 1.0f));
    }

    public void iniciarSimonDice()
    {
            if (numeroPrueba == 0)
            {
                setTexto("Nivel", ("Sequences completed: " + numeroPrueba.ToString() + "/3"));
            }
            if (numeroPrueba < 3)
            {
                setTexto("Mensaje_Secuencia", "Level " + (numeroPrueba + 1).ToString());
                secuenciaFallada = false;
                GameObject.Find("StartSimonDice").tag = "Untagged";
                cambiarTagsCubos("CuboSimonDice");
                secuenciaActiva = true;
                secuenciaInput.Clear();
                secuenciaCreada.Clear();
                for (int i = 0; i < 5; i++)
                    secuenciaCreada.Add(Random.Range(0, 4));

                StartCoroutine(empezarSecuencia());
            }
            else
            {
                setTexto("Mensaje_Secuencia", "");
                setTexto("Nivel", "Trial succed!");
                win = true;
            }
        
    }

    private void cambiarTagsCubos(string tag)
    {
        for (int i = 0; i < colores.Count; i++)
        {
            GameObject.Find(colores[i]).tag = tag;
        }
    }

    private IEnumerator empezarSecuencia()
    {
        yield return new WaitForSeconds(2.0f);
        for (int i = 0; i < secuenciaCreada.Count; i++)
        {
            string color = colores[secuenciaCreada[i]];
            cambiarColor(color);
            yield return new WaitForSeconds(1.5f);
        }
        secuenciaActiva = false;
        esperandoInput = true;
    }

    private IEnumerator iluminar(Color color, GameObject cubo, float time)
    {
        color.a = 1.0f;
        cubo.transform.GetComponent<Renderer>().material.color = color;
        yield return new WaitForSeconds(time);
        color.a = 0.3f;
        cubo.transform.GetComponent<Renderer>().material.color = color;
    }
}
