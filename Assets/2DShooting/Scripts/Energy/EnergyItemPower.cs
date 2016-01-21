using UnityEngine;
using System.Collections;

public class EnergyItemPower : EnergyItem {

    private bool isInPower = false;
    public override void OnItemUse()
    {
        if (!isInPower)
        {
            base.OnItemUse();
            isInPower = true;
            LeanTween.dispatchEvent((int)Events.ENERGYPOWERIN);
        }
        else
        {
            isInPower = false;
            LeanTween.dispatchEvent((int)Events.ENERGYPOWEROUT);
        }
    }
}
