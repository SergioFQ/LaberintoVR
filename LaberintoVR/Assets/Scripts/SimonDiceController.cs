﻿using System.Collections;
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

    public bool secuenciaActiva = false;
    public bool esperandoInput = false;

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
                        setTexto("Nivel", "¡Prueba Superada!");
                        GameObject.Find("StartSimonDice").tag = "Untagged";
                    }
                    else
                    {
                        setTexto("Nivel", "Secuencias superadas: " + numeroPrueba.ToString() + "/3");
                        setTexto("Mensaje_Secuencia", "¡Secuencia correcta!");
                    }
                        
                }
                else
                {
                    numeroPrueba = 0;
                    setTexto("Nivel", "Secuencias superadas: " + numeroPrueba.ToString() + "/3");
                    setTexto("Mensaje_Secuencia", "¡Secuencia incorrecta!");
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
                    setTexto("Nivel", "Secuencias superadas: " + numeroPrueba.ToString() + "/3");
                    setTexto("Mensaje_Secuencia","¡Secuencia incorrecta!");
                }
            }
        }
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
        GameObject cubo = GameObject.Find(nombre);
        Color color = cubo.transform.GetComponent<Renderer>().material.color;
        cubo.transform.GetComponent<Renderer>().material.color = color;
        StartCoroutine(iluminar(color, cubo, 1.0f));
    }

    public void iniciarSimonDice()
    {
        if (numeroPrueba == 0)
        {
            setTexto("Nivel", ("Secuencias superadas: " + numeroPrueba.ToString() + "/3"));
        }
        if (numeroPrueba < 3)
        {
            setTexto("Mensaje_Secuencia", "Nivel "+(numeroPrueba+1).ToString());
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
            setTexto("Nivel", "¡Prueba Superada!");
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