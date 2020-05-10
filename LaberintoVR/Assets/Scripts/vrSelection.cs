using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class vrSelection : MonoBehaviour
{
    [SerializeField] private Image imgGaze;
    [SerializeField] private float totalTime = 2.0f;
    [SerializeField] private int distanceOfRay = 10;
    [SerializeField] private Canvas UI;
    [SerializeField] private Camera UICamera;
    [SerializeField] private SimonDiceController SDController;
    [SerializeField] private NPuzzleController NPController;
    [SerializeField] private EquationController EquController;
    [SerializeField] private ControladorFade FadeController;
    [SerializeField] private AudioClip _boton_Clip;

    private RaycastHit _hit;
    private bool gvrStatus = false;
    private float gvrTimer = 0.0f;
    private string tagAnterior = "";
    private string tagActual = "";

    public Pause pauseMenu;
    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out _hit, distanceOfRay) && (_hit.transform.tag != "noTeleport") && (_hit.transform.tag != "Untagged") && !SDController.secuenciaActiva)
        {
            Debug.Log(_hit.transform.name);
            UI.planeDistance = Vector3.Distance(UICamera.transform.position, _hit.point);
            tagActual = _hit.transform.tag;
            if (tagAnterior != tagActual)
            {
                gvrTimer = 0;
                imgGaze.fillAmount = 0;
            }
            if (!gvrStatus)
                gvrOn();
            if (gvrStatus)
            {
                gvrTimer += Time.deltaTime;
                imgGaze.fillAmount = gvrTimer / totalTime;
            }
            if (imgGaze.fillAmount == 1 && Physics.Raycast(ray, out _hit, distanceOfRay))
            {
                switch (_hit.transform.tag)
                {
                    case "teleport":
                        teleport(_hit.point);
                        imgGaze.fillAmount = 0;
                        gvrTimer = 0;
                        break;
                    case "SimonDice":
                        teleport(_hit.transform.position);
                        break;
                    case "CuboSimonDice":
                        if (SDController.esperandoInput)
                            SDController.añadirASecuencia(_hit.transform.name);
                        gvrOff();
                        break;
                    case "StartSimonDice":
                        SDController.iniciarSimonDice();
                        gvrOff();
                        break;

                    case "ResetNPuzzle":
                        if (!NPController.juegoTerminado())
                        {
                            gvrOff();
                            NPController.resetear();
                        }
                        break;

                    case "NPuzzle":
                        if (!NPController.juegoTerminado() && NPController.piezaMovible(_hit.transform.localPosition.x, _hit.transform.localPosition.y, false))
                        {
                            gvrOff();
                            NPController.moverPieza(_hit.transform.localPosition.x, _hit.transform.localPosition.y, false);
                        }
                        break;

                    case "equation":
                        if (!EquController.equationSolved)
                        {
                            EquController.selectNum(_hit.transform.name);
                            gvrOff();
                        }
                        break;
                    case "selectEquation":
                        EquController.selectEquation();
                        gvrOff();
                        break;

                    case "ButtonContinue":
                        Debug.Log("Continuar");
                        _hit.transform.GetComponent<AudioSource>().clip = _boton_Clip;
                        _hit.transform.GetComponent<AudioSource>().Play();
                        pauseMenu.resumeGame();
                        gvrOff();
                        break;
                    case "ButtonRestart":
                        _hit.transform.GetComponent<AudioSource>().clip = _boton_Clip;
                        _hit.transform.GetComponent<AudioSource>().Play();
                        Debug.Log("Reiniciar");
                        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                        if (pauseMenu != null) Destroy(pauseMenu.MainAruco.gameObject);
                        FadeController.toScene(SceneManager.GetActiveScene().name);
                        gvrOff();
                        break;
                    case "ButtonVolume":
                        _hit.transform.GetComponent<AudioSource>().clip = _boton_Clip;
                        _hit.transform.GetComponent<AudioSource>().Play();
                        Debug.Log("Volumen");
                        break;
                    case "ButtonExit":
                        _hit.transform.GetComponent<AudioSource>().clip = _boton_Clip;
                        _hit.transform.GetComponent<AudioSource>().Play();
                        Debug.Log("Salir juego");
                        //Application.Quit();
                        if (pauseMenu != null) Destroy(pauseMenu.MainAruco.gameObject);
                        FadeController.toScene("SALIR");
                        gvrOff();
                        break;
                    case "ButtonBegin":
                        _hit.transform.GetComponent<AudioSource>().clip = _boton_Clip;
                        _hit.transform.GetComponent<AudioSource>().Play();
                        Debug.Log("Empezar");
                        //SceneManager.LoadScene("mapScene");
                        FadeController.toScene("mapScene");
                        gvrOff();
                        break;
                    case "Libro":
                        pauseMenu.pauseGame();
                        gvrOff();
                        break;

                }
            }
            if (tagAnterior != tagActual)
                tagAnterior = tagActual;
        }
        else
        {
            UI.planeDistance = distanceOfRay;
            gvrOff();
            tagAnterior = "";
        }

    }

    private void teleport(Vector3 position)
    {
        transform.position = new Vector3(position.x, 1.7f, position.z);
    }

    public void gvrOn()
    {
        gvrStatus = true;
    }

    public void gvrOff()
    {
        gvrStatus = false;
        gvrTimer = 0.0f;
        imgGaze.fillAmount = 0;
    }
}
