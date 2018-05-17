using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour
{
    public static float Loaded = .5f;
    public Slider loadingSlider;

    public Text loadingText;

    // Use this for initialization
    public void LoadScene()
    {
        Debug.Log("I have clicked");
        loadingText.text = "0%";
        StartCoroutine("LoadNewScene");
    }

    private IEnumerator LoadNewScene()
    {
        Loaded = 0f;
        var async = SceneManager.LoadSceneAsync("PlayScene");

        while (!async.isDone)
            /*
                if (async.progress < 0.9f)
                {
                    float progress = Mathf.Clamp01(async.progress / 0.9f);
                    loadingSlider.value = progress;
                    loadingText.text = progress * 100f + "%";
                }
                else
                {
                    float progress = Mathf.Clamp01(async.progress / 0.9f);
                    loadingSlider.value = progress;
                    loadingText.text = progress * 100f + "%";
                    async.allowSceneActivation = true;
                }
                */
            yield return null;
    }

    public void Update()
    {
    }
}