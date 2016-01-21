using UnityEngine;
using System.Collections;

public class CanvasHandler : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
        LeanTween.addListener((int)Events.ENERGYPOWERIN, OnEnergyPowerIn);
        LeanTween.addListener((int)Events.ENERGYPOWEROUT, OnEnergyPowerOut);
    }

   
    public void OnDestroy()
    {
        LeanTween.removeListener((int)Events.ENERGYPOWERIN, OnEnergyPowerIn);
        LeanTween.removeListener((int)Events.ENERGYPOWEROUT, OnEnergyPowerOut);
    }

    void OnEnergyPowerIn(LTEvent evt)
    {
        gameObject.SetActive(false);
    }

    void OnEnergyPowerOut(LTEvent evt)
    {
        gameObject.SetActive(true);
    }
}
