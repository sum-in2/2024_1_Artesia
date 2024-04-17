using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    protected static SceneLoader instance;
    public static SceneLoader Instance{
        get {
            if (instance == null){
                var obj = FindObjectOfType<SceneLoader>();
                if(obj != null)
                    instance = obj;
                else
                    instance = Create();
            }
            return instance;
        }
        private set{
            instance = value;
        }
    }
    [SerializeField] CanvasGroup m_canvasGroup;
    [SerializeField] Image m_progressBar;
    Canvas RenderCamera;
    private string LoadSceneName;
    public static SceneLoader Create(){
        var SceneLoaderPrefab = Resources.Load<SceneLoader>("Prefabs/SceneLoader");
        return Instantiate(SceneLoaderPrefab);
    }

    private void Awake() {
        if(Instance != this){
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void LoadSceneEnd(Scene scene, LoadSceneMode loadSceneMode){
        if(scene.name == LoadSceneName){
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= LoadSceneEnd;
        }
    }

    public void LoadScene(string SceneName){
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += LoadSceneEnd;
        LoadSceneName = SceneName;
        
        RenderCamera = gameObject.GetComponent<Canvas>();
        RenderCamera.worldCamera = Camera.main;

        StartCoroutine(Load(LoadSceneName));
    }

    private IEnumerator Fade(bool IsFade){
        float timer = 0f;

        while(timer < 1f){
            yield return null;
            timer += Time.unscaledDeltaTime * 2f;
            m_canvasGroup.alpha = Mathf.Lerp(IsFade ? 0 : 1, IsFade ? 1:0, timer);
        }

        if(!IsFade)
            gameObject.SetActive(false);
    }

    private IEnumerator Load(string SceneName){
        m_progressBar.fillAmount = 0f;
        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(SceneName);
        op.allowSceneActivation = false;
        float timer = 0f;

        while(!op.isDone){
            yield return null;

            timer += Time.unscaledDeltaTime;

            if(op.progress < 0.9f){
                m_progressBar.fillAmount = Mathf.Lerp(m_progressBar.fillAmount, op.progress, timer);
                if(m_progressBar.fillAmount >= op.progress)
                    timer = 0f;
            } else {
                m_progressBar.fillAmount = Mathf.Lerp(m_progressBar.fillAmount, 1f, timer);
                if(m_progressBar.fillAmount == 1f){
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
