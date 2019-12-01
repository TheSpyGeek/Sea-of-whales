using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{



    public void LoadScene(int indexScene) {
        StartCoroutine(LoadSceneAsync(indexScene));
    }


    IEnumerator LoadSceneAsync(int indexScene) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(indexScene);

        while(!operation.isDone) {

            yield return null;
        }

    }
    

}
