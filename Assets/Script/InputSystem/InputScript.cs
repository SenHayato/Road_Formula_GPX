using UnityEngine;
using UnityEngine.InputSystem;

public class InputScript : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;

    public InputAction gasAction, brakeAction, changeFormAction, boostAction, steerAction, pauseAction, changeMusic;
    private void Awake()
    {
         playerInput = FindFirstObjectByType<PlayerInput>();
    }

    void Start()
    {
        gasAction = playerInput.actions.FindAction("Gas");
        brakeAction = playerInput.actions.FindAction("Brake");
        changeFormAction = playerInput.actions.FindAction("FormChange");
        boostAction = playerInput.actions.FindAction("BoostMode");
        steerAction = playerInput.actions.FindAction("Steer");
        pauseAction = playerInput.actions.FindAction("Pause");
        changeMusic = playerInput.actions.FindAction("ChangeMusic");
    }

    //void OnChangeMusic(InputAction input)
    //{
    //    if (input.IsPressed())
    //    {
    //        Debug.Log("Test");
    //    }
    //}
}
