using UnityEngine;

public class CarDriver : MonoBehaviour
{
    [Header("Driver Image")]
    public Sprite driverDefault;
    public Sprite driverPanic;
    public Sprite driverMad;

    [Header("Dialogue")]
    public AudioClip[] voiceDefault;

    public enum DriverReaction
    {
        Default, Panic, Mad
    }
}
