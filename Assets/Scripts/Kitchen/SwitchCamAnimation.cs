using UnityEngine;

public class SwitchCamAnimation : MonoBehaviour
{
    public CameraSwitcher camSwitcher;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayTVIn()
    {
        camSwitcher.TriggerSwitchToKitchen();
    }

    public void PlayTVOut()
    {
        camSwitcher.TriggerSwitchToFPS();
    }

}
