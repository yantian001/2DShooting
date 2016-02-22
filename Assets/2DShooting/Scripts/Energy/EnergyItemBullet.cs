
class EnergyItemBullet : EnergyItem
{
    public override void OnEnable()
    {
        base.OnEnable();
        LeanTween.addListener((int)Events.NEEDBULLET, OnNeedBullet);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        LeanTween.removeListener((int)Events.NEEDBULLET, OnNeedBullet);

    }
    public override void OnItemUse()
    {
        if (isActive)
        {
            base.OnItemUse();
            LeanTween.dispatchEvent((int)Events.CLIPREFILL);
        }
    }

    void OnNeedBullet(LTEvent evt)
    {
        // if (isActive)
        OnItemUse();
    }
}