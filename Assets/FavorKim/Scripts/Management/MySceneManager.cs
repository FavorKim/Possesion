using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MySceneManager : MonoBehaviour
{
    private static MySceneManager instance;
    public static MySceneManager Instance {  get { return instance; } }


    [SerializeField] GameObject loading;
    [SerializeField] TMP_Text loadingTxt;
    [SerializeField] Slider loadingBar;
    CanvasGroup blocker;
    [SerializeField] private float fadeDuration;


    private void Awake()
    {
        blocker = GetComponentInChildren<CanvasGroup>();
    }
    private void Start()
    {
        if (instance == null)
        {
            instance = FindAnyObjectByType<MySceneManager>();
            if (instance == null)
            {
                instance = this;
            }
            SceneManager.sceneLoaded += FadeIn;
            DontDestroyOnLoad(this);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= FadeIn;
    }

    public void ChangeScene(string sceneName)
    {
        FadeOut(sceneName);
    }

    void LoadScene(string sceneName)
    {
        StartCoroutine(CorLoadScene(sceneName));
    }



    void FadeOut(string sceneName)
    {
        blocker.DOFade(1, fadeDuration).OnStart(() => blocker.blocksRaycasts = true).OnComplete(() => { LoadScene(sceneName); });
    }
    void FadeIn(Scene scene, LoadSceneMode mode)
    {
        loading.SetActive(false);
        blocker.DOFade(0, fadeDuration).OnStart(() => blocker.blocksRaycasts = false) ;
    }


    IEnumerator CorLoadScene(string sceneName)
    {
        loading.SetActive(true);
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        float pastTime=0.0f;
        float percentage = 0f;
        while (!async.isDone)
        {
            yield return null;
            pastTime += Time.deltaTime;
            if (async.progress < 0.9)
                percentage = Mathf.Lerp(percentage, async.progress, pastTime);
            if (async.progress >= 0.90f)
                percentage = Mathf.Lerp(percentage, 1, pastTime);

            loadingBar.value = percentage;
            loadingTxt.text = (percentage * 100.0f).ToString("0") + '%';

            if (percentage >= 1)
                async.allowSceneActivation = true;
        }
    }
}
