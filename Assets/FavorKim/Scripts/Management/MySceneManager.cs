using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MySceneManager : MonoBehaviour
{
    private static MySceneManager instance;
    public static MySceneManager Instance {  get { return instance; } }


    [SerializeField] GameObject loading;
    [SerializeField]CanvasGroup blocker;
    [SerializeField] private float fadeDuration;
    float percentage;


    private void Awake()
    {
        blocker = GetComponentInChildren<CanvasGroup>();

    }
    private void Start()
    {
        if (instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        Debug.Log("start");
        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += FadeIn;
    }

    private void Update()
    {
        Debug.Log(percentage);
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
        blocker.DOFade(0, fadeDuration).OnStart(() => blocker.blocksRaycasts = false) ;
    }


    IEnumerator CorLoadScene(string sceneName)
    {
        //loading.SetActive(true);
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            yield return null;
            percentage = async.progress;
            if (percentage >= 0.9f)
                async.allowSceneActivation = true;
        }
    }

    // fadeout -> 완료 후 씬 로딩 -> 완료 후 FadeIn
    /*
    기존 - 미사일 프리팹 생성 (엄마, 자식 셋)
    변경 - 미사일 프리팹 (엄마 생성), 엄마 프리팹(자식 생성 및 변수 초기화)

    -기존-
    맨 위에 엄마를 생성하는건 objectPool이 projectile을 생성.
    자식 - 이 구조에선 Projectile 안에 missle, muzzle, explosion이 들어가있음.
    -변경-
    Projectile 안에 자식들을 넣어놓지 말고,
    자식프리팹을 Projectile에 직렬화 한 후, Projectile 내부에서 자식들을 Instantiate
    Instantiate 후 변수 초기화.


    class Projectile
    {
        [serializefield] GameObject misslePrefab;
        [serializefield] GameObject muzzlePrefab;
        [serializefield] GameObject explosionPrefab;
        ParticleSystem misslefx;
        ParticleSystem muzzlefx;
        ParticleSystem explosionfx;
        
        void Awake()
        {
            misslefx = Instantiate(missle, transform).GetComponent<ParticleSystem>();
            misslefx = Instantiate(missle, transform).GetComponent<ParticleSystem>();
            misslefx = Instantiate(missle, transform).GetComponent<ParticleSystem>();
        }

        void OnTriggerEnter(Collision other)
        {
            if(other.CompareTag("Player)
            {   
                misslefx.Stop();
                explosionfx.Play();
            }
        }
    }


    
    직렬화한 PS변수를 Stop하면
    clone이 아니라 프리팹 원본의 PS가 Stop.
    우리는 Clone에 접근해야한다.








    Awake
    ParticleSystem[] fxs = GetComponentsInChildren<GameObject>().GetComponent<ParticleSystem>();
    missle = fxs[0];
    explosion = fxs[1];
    muzzle = fxs[2];
    */

}
