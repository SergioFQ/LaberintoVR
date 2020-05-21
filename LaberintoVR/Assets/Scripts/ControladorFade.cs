using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorFade : MonoBehaviour
{
    public Animator animator;
    private string scene;
    private vrSelection player;
    private GameObject pos;
    public void toScene(string scene) {
        animator.SetTrigger("fadeout");
        this.scene = scene;
    }
    public void tpPlayer(string scene, vrSelection player, GameObject pos) {
        animator.SetTrigger("fadeout");
        this.scene = scene;
        this.player = player;
        this.pos = pos;
    }

    public void onFadeComplete() {
        if (scene == "SALIR") {
            //SceneManager.LoadScene("MainMenu");
            Application.Quit();
        } else if (scene == "ButtonBegin") {
            player.transform.position = pos.transform.position;
        } else {
            SceneManager.LoadScene(scene);
        }
    }
}
