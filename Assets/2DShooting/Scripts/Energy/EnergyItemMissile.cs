using UnityEngine;
using System.Collections;

public class EnergyItemMissile : EnergyItem
{

    public GameObject rockect;

    public Transform parent;
    public float rockectSpwanY;
    public override void OnItemUse()
    {
        base.OnItemUse();
        if (rockect == null)
            return;
        Vector3 target = new Vector3(0f, 0f, 0f);
        {
            var currentWeapon = GameManager.Instance.GetCurrentWeapon();
            if (currentWeapon != null)
            {
                target = currentWeapon.signTransform.position;
            }
        }

        Vector3 spwanPosition = new Vector3(target.x, rockectSpwanY, target.z);
        var r = (GameObject)Instantiate(rockect, spwanPosition, Quaternion.identity);
        if (parent)
            r.transform.SetParent(parent);
        var sr = r.GetComponent<Rocket>();
        sr.SetTarget(target);
    }
}
