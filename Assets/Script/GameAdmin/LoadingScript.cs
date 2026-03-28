using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{
    [Header("Loading Properties")]
    [SerializeField] Slider loadingBar;
    [SerializeField] GameObject loadingCanvas;
    //[SerializeField] string sceneToLoad;

    void Start()
    {
        loadingCanvas.SetActive(false);
        loadingBar.value = 0;
    }

    public void LoadingToScene(string sceneToLoad)
    {
        loadingCanvas.SetActive(true);
        StartCoroutine(LoadAsync(sceneToLoad));
    }

    IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        float current = 0f;

        while (!operation.isDone)
        {
            float target = Mathf.Clamp01(operation.progress / 0.9f);
            current = Mathf.Lerp(current, target, Time.deltaTime * 10f);

            loadingBar.value = current;

            yield return null;
        }
    }

    void Update()
    {
        
    }
}
