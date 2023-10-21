using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class LoadManager : MonoBehaviour
{
    public Image loadingBar;
    public float loadingTime;

    void Start()
    {
        loadingBar.fillAmount = 0;
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

        int loopNum = 0;
        while (!asyncScene.isDone)
        {
            yield return null;
            loadingTime += Time.deltaTime;

            if (asyncScene.progress >= 0.9f)
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1f, loadingTime);

                if (loadingBar.fillAmount == 1.0f)
                {
                    asyncScene.allowSceneActivation = true;
                }
            }
            else
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, asyncScene.progress, loadingTime);

                if (loadingBar.fillAmount >= asyncScene.progress)
                {
                    loadingTime = 0f;
                }
            }

            if (loopNum++ > 10000)
                throw new Exception("Infinite Loop");
        }
    }
}
