
class EnergyItemBullet : EnergyItem
{
    public override void OnItemUse()
    {
        base.OnItemUse();
        LeanTween.dispatchEvent((int)Events.CLIPREFILL);
    }
}