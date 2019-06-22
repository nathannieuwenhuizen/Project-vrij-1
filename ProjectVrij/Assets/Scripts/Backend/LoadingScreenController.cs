using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private GameObject loadingScreenObj;

    AsyncOperation async;
    public void LoadScene(int buildIndex)
    {
        StartCoroutine(Loading(buildIndex));
    }
    IEnumerator Loading(int buildIndex)
    {
        loadingScreenObj.SetActive(true);
        async = SceneManager.LoadSceneAsync(buildIndex);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            slider.value = async.progress;
            if (async.progress >= 0.9f) {
                slider.value = 1f;
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        loadingScreenObj.SetActive(false);
        //LoadScene(1);

    }

}
