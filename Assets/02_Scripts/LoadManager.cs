using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
    public Slider loadingBar;
    public float loadingTime;

    void Start()
    {
        loadingBar.value = 0;
        StartCoroutine(LoadAsyncScene());
    }

    public static void LoadScene()
    {
        SceneManager.LoadScene("Load");
    }

    IEnumerator LoadAsyncScene()
    {
        yield return null;
        loadingTime = 0;

        AsyncOperation asyncScene = SceneManager.LoadSceneAsync("Main");
        asyncScene.allowSceneActivation = false;

        while (!asyncScene.isDone)
        {
            if (asyncScene.progress < 0.9f)
            {
                loadingBar.value = Mathf.Lerp(loadingBar.value, asyncScene.progress, loadingTime);
                if (loadingBar.value >= asyncScene.progress)
                {
                    loadingTime = 0f;
                }
            }
            else
            {
                loadingBar.value = Mathf.Lerp(loadingBar.value, 1f, loadingTime);
                if (loadingBar.value == 1.0f)
                {
                    asyncScene.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
