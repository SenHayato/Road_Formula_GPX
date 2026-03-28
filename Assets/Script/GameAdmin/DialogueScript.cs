using System.Collections;
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

    [Header("Reference")]
    [SerializeField] CarDriver carDriver;

    void Awake()
    {
        carDriver = FindFirstObjectByType<CarDriver>();
    }

    void Start()
    {

    }

    public void DialogueSetUp(CarDriver.DriverReaction driverReaction)
    {
        driverPicture.sprite = driverReaction switch
        {
            CarDriver.DriverReaction.Default => carDriver.driverDefault,
            CarDriver.DriverReaction.Panic => carDriver.driverPanic,
            CarDriver.DriverReaction.Mad => carDriver.driverMad,
            _ => carDriver.driverDefault
        };
    }

    IEnumerator DialogueEnable()
    {
        DialogueOn();

        yield return new WaitForSeconds(1f);

        DialogueOff();
    }

    void DialogueOn()
    {
        Debug.Log("Dial In");
        isActive = true;
    }

    void DialogueOff()
    {
        Debug.Log("Dial Out");
        isActive = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(DialogueEnable());
        }
    }
}
