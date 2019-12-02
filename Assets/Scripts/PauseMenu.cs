using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool gamePaused = false;

    public GameObject pauseMenuUI;

    public Slider slider;

    private void Start()
    {
        slider.value = Time.timeScale;
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


    public void LoadMenu() {
        Time.timeScale = 1f;
        gamePaused = false;
        SceneManager.LoadScene("Start Menu");
    }


    public void reloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log(scene.name);
        SceneManager.LoadScene(scene.buildIndex);
        Time.timeScale = slider.value;
    }
}
