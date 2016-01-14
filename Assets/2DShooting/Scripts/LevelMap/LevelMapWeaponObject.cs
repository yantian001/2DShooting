using UnityEngine;
using System.Collections;

public class LevelMapWeaponObject : MonoBehaviour {


    #region Variables
    /// <summary>
    /// 武器对象
    /// </summary>
    public GameObject WeaponObject;

    public int weaponId = 0;

    Weapon weapon = null;

    public Weapon GetWeapon()
    {
        if (weapon == null)
        {
             weapon = WeaponObject.transform.FindChild("Gun").GetComponent<Weapon>();
        }
        return weapon;
    }
    #endregion

    // Use this for initialization
    void Start () {
	    if(weapon==null && WeaponObject != null)
        {
           weapon = WeaponObject.transform.FindChild("Gun").GetComponent<Weapon>();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
