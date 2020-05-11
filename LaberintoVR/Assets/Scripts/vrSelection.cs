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
    [SerializeField] private DoorController doorCont;
    [SerializeField] private Light linterna;
    [SerializeField] private GameObject cubo;

    private RaycastHit _hit;
    private bool gvrStatus = false;
    private float gvrTimer = 0.0f;
    private string tagAnterior = "";
    private string tagActual = "";

    public Pause pauseMenu;
    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out _hit, distanceOfRay) && (_hit.transform.tag != "noTeleport") && !SDController.secuenciaActiva)
        {
            cubo.transform.position = _hit.point;
            //Debug.Log(_hit.transform.name);
            UI.planeDistance = Vector3.Distance(UICamera.transform.position, _hit.point);
            tagActual = _hit.transform.tag;
            if (_hit.transform.tag != "Untagged")
            {
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
            }
            else
            {
                gvrOff();
                tagAnterior = "";
            }
                
            if (imgGaze.fillAmount == 1 && Physics.Raycast(ray, out _hit, distanceOfRay))
            {
                switch (_hit.transform.tag)
                {
                    case "teleport":
                        teleport(_hit.point);
                        imgGaze.fillAmount = 0;
                        gvrTimer = 0;
                        if (SDController.active) { SDController.active = false; SDController.switchLampara(false); }
                        if (NPController.active) { NPController.active = false; NPController.switchLampara(false);  }
                        if (EquController.active) { EquController.active = false; EquController.switchLampara(false);  }
                        if (linterna.intensity == 0) { linterna.intensity = 1; }
                        break;
                    case "SimonDice":
                        teleport(_hit.transform.position);
                        if (!SDController.active) { SDController.active = true; SDController.switchLampara(true); }
                        if (linterna.intensity != 0) { linterna.intensity = 0; }
                        break;
                    case "Equ":
                        teleport(_hit.transform.position);
                        if (!EquController.active) { EquController.active = true; EquController.switchLampara(true);}
                        if (linterna.intensity != 0) { linterna.intensity = 0; }
                        break;
                    case "NPzz":
                        teleport(_hit.transform.position);
                        if (!NPController.active) { NPController.active = true; NPController.switchLampara(true);  }
                        if (linterna.intensity != 0) { linterna.intensity = 0; }
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
                            _hit.transform.GetComponent<AudioSource>().Play();
                            EquController.selectNum(_hit.transform.name);
                            gvrOff();
                        }
                        break;
                    case "selectEquation":
                        _hit.transform.GetComponent<AudioSource>().Play();
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
                        if (AudioListener.volume != 0)
                            AudioListener.volume = 0;
                        else
                            AudioListener.volume = 1;
                        Debug.Log("Volumen");
                        gvrOff();
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
                    case "LeftDoor":
                        doorCont.awakeDoor(0);
                        gvrOff();
                        break;
                    case "CenterDoor":
                        doorCont.awakeDoor(1);
                        gvrOff();
                        break;
                    case "RightDoor":
                        doorCont.awakeDoor(2);
                        gvrOff();
                        break;
                    case "MainMenuButton":
                        FadeController.toScene("MainMenu");
                        gvrOff();
                        break;

                }
            }
            if (tagAnterior != tagActual)
                tagAnterior = tagActual;
        }
        else
        {
            gvrOff();
            tagAnterior = "";
            UI.planeDistance = distanceOfRay;
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
