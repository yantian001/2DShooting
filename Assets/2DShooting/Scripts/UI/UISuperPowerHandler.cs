using UnityEngine;
using System.Collections;

public class UISuperPowerHandler : MonoBehaviour {


    void Awake()
    {
        LeanTween.addListener((int)Events.ENERGYPOWERIN, OnEnergyPowerIn);
        LeanTween.addListener((int)Events.ENERGYPOWEROUT, OnEnergyPowerOut);
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnDestroy()
    {
        LeanTween.removeListener((int)Events.ENERGYPOWERIN, OnEnergyPowerIn);
        LeanTween.removeListener((int)Events.ENERGYPOWEROUT, OnEnergyPowerOut);
    }

    void OnEnergyPowerIn(LTEvent evt)
    {
        gameObject.SetActive(true);
    }

    void OnEnergyPowerOut(LTEvent evt)
    {
        gameObject.SetActive(false);
    }
}
