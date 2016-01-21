using UnityEngine;
using System.Collections;

public class EnergyItemPower : EnergyItem {

    private bool isInPower = false;
    public override void OnItemUse()
    {
        base.OnItemUse();
        isInPower = false;
        LeanTween.dispatchEvent((int)Events.ENERGYPOWERIN);
    }
}
