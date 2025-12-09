using UnityEngine;
using UnityEngine.UI;

public class DialogueScript : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] GameObject dialogueObj;
    [SerializeField] Image driverPicture;
    [SerializeField] AudioSource driverVoice;

    [Header("Dialogue Component")]
    [SerializeField] bool isActive = false;
    [SerializeField] Animation dialAnimation;
    bool wasActive = false;

    [Header("Reference")]
    [SerializeField] CarDriver carDriver;

    void Awake()
    {
        carDriver = FindFirstObjectByType<CarDriver>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
