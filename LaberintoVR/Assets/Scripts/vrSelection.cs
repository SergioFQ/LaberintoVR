using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class vrSelection : MonoBehaviour
{
    [SerializeField] private Image imgGaze;
    [SerializeField] private float totalTime = 2.0f;
    [SerializeField] private int distanceOfRay = 10;

    private RaycastHit _hit;
    private bool gvrStatus = false;
    private float gvrTimer = 0.0f;
    private string tagAnterior = "";
    private string tagActual = "";
    
    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out _hit, distanceOfRay) && (_hit.transform.tag != "noTeleport") && (_hit.transform.tag != "Untagged"))
        {
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
                }
            }
            if(tagAnterior!=tagActual) 
                tagAnterior = tagActual;
        }
        else
        {
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
