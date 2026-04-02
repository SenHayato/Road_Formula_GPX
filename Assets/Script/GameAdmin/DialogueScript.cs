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
    public bool isActive;
    [SerializeField] Animation dialAnimation;

    [Header("Reference")]
    [SerializeField] CarDriver carDriver;

    void Awake()
    {
        carDriver = FindFirstObjectByType<CarDriver>();
    }

    void Start()
    {
        isActive = false;
    }

    public void DialogueSetUp(CarDriver.DriverReaction driverReaction)
    {
        if (!isActive)
        {
            driverPicture.sprite = driverReaction switch
            {
                CarDriver.DriverReaction.Default => carDriver.driverDefault,
                CarDriver.DriverReaction.Panic => carDriver.driverPanic,
                CarDriver.DriverReaction.Mad => carDriver.driverMad,
                _ => carDriver.driverDefault
            };

            StartCoroutine(DialogueEnable());
        }
       
    }

    IEnumerator DialogueEnable()
    {
        DialogueOn();

        yield return new WaitForSeconds(2f);

        DialogueOff();
    }

    void DialogueOn()
    {
        Debug.Log("Dial In");
        isActive = true;
        dialAnimation.Play("DialogueIn");
    }

    void DialogueOff()
    {
        Debug.Log("Dial Out");
        dialAnimation.Play("DialogueOut");
        isActive = false;
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.L))
    //    {
    //        StartCoroutine(DialogueEnable());
    //    }
    //}
}
