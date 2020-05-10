using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorFade : MonoBehaviour
{
    public Animator animator;
    private string scene;
    public void toScene(string scene) {
        animator.SetTrigger("fadeout");
        this.scene = scene;
    }

    public void onFadeComplete() {
        if (scene == "SALIR") {
            Application.Quit();
        } else {
            SceneManager.LoadScene(scene);
        }
    }
}
