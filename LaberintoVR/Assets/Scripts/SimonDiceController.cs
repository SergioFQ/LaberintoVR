using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonDiceController : MonoBehaviour
{
    private IEnumerator coroutine;
    private List<string> colores = new List<string> { "CuboAmarillo", "CuboAzul", "CuboVerde", "CuboRojo"};
    private List<int> secuencia = new List<int> { };
    public bool secuenciaActiva = false;

    public void cambiarColor(string nombre)
    {
        GameObject cubo = GameObject.Find(nombre);
        Color color = cubo.transform.GetComponent<Renderer>().material.color;
        cubo.transform.GetComponent<Renderer>().material.color = color;
        StartCoroutine(iluminar(color, cubo, 1.0f));
    }

    public void iniciarSimonDice()
    {
        for (int i = 0; i < 5; i++)
            secuencia.Add(Random.Range(0, 4));

        StartCoroutine(empezarSecuencia());
    }

    private IEnumerator empezarSecuencia()
    {
        yield return new WaitForSeconds(2.0f);
        for (int i = 0; i < secuencia.Count; i++)
        {
            Debug.Log("Numero en secuencia: "+i);
            string color = colores[secuencia[i]];
            cambiarColor(color);
            yield return new WaitForSeconds(1.5f);
        }
        secuencia.Clear();
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
