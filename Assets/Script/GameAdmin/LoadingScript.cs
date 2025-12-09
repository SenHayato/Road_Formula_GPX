using UnityEngine;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{
    [Header("Loading Properties")]
    [SerializeField] Slider loadingBar;
    //[SerializeField] string sceneToLoad;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void LoadingToScene(string sceneToLoad)
    {
        gameObject.SetActive(true);
        //loading ke scene dengan string sceneToLoad
    }

    void Update()
    {
        
    }
}
