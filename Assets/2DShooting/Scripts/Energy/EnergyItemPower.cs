using UnityEngine;
using System.Collections;

public class EnergyItemPower : EnergyItem {


    private bool isInPower = false;

    public override void OnEnable()
    {
        base.OnEnable();
        LeanTween.addListener((int)Events.ENERGYPOWEROUT, OnExitPower);
    }

    void OnExitPower(LTEvent evt)
    {
        isInPower = false;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        LeanTween.removeListener((int)Events.ENERGYPOWEROUT, OnExitPower);
    }

    public override void OnItemUse()
    {
        if (!isInPower)
        {
            //base.OnItemUse();
            isInPower = true;
            LeanTween.dispatchEvent((int)Events.ENERGYPOWERIN);
        }
    }
}
