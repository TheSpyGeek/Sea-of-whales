using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool gamePaused = false;

    public GameObject pauseMenuUI;

    public GameObject commencer;
    public GameObject reprendre;

    public Slider slider;

    private Canvas canva;

    public WorldScript world;

    private void Start() {
        reprendre.SetActive(false);
        commencer.SetActive(true);
        slider.value = Time.timeScale;
        Time.timeScale = 0f;
        gamePaused = true;
        pauseMenuUI.SetActive(true);
    }

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
        Time.timeScale = slider.value;
        gamePaused = false;

    }

    public void Commencer() {

        world.CleanScene();
        world.Setup();
        pauseMenuUI.SetActive(false);
        Time.timeScale = slider.value;
        gamePaused = false;
        reprendre.SetActive(true);
        commencer.SetActive(false);
    }


    public void Quit() {
        Time.timeScale = 1f;
        gamePaused = false;
        Application.Quit();
    }


    public void reloadScene()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = slider.value;
        gamePaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
