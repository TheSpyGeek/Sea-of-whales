using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{

    public GameObject objSlider;
    public Slider slider;

    public Text sliderText;

    public void LoadScene(int indexScene) {
        StartCoroutine(LoadSceneAsync(indexScene));
    }


    IEnumerator LoadSceneAsync(int indexScene) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(indexScene);

        objSlider.SetActive(true);

        while(!operation.isDone) {

            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progress;
            sliderText.text = progress * 100 + "%";

            yield return null;
        }

    }
    

}
