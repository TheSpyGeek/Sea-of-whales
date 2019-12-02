using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool gamePaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(gamePaused) {
                Reprendre();
            } else {
                Pause();
            }
        }
        
    }



    public void Pause() {
        pauseMenuUI.SetActive(true);
        gamePaused = true;
        Time.timeScale = 0f;
    }


    public void Reprendre() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;

    }


    public void LoadMenu() {
        Time.timeScale = 1f;
        gamePaused = false;
        SceneManager.LoadScene("Start Menu");
    }

}
