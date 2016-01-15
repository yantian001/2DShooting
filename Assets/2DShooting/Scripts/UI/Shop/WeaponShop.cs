using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponShop : MonoBehaviour
{
    public RectTransform weaponContent;

    public RectTransform weaponDisplay;

    public RectTransform weaponInfo;

    WeaponShopItem selectWeaponItem;

    bool toggleEventListened = false;

    int selectWeaponId = -1;
    // Use this for initialization
    void Start()
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public void OnEnable()
    {
        if (!toggleEventListened)
        {
            var toggles = weaponContent.GetComponentsInChildren<Toggle>();
            if (toggles != null && toggles.Length > 0)
            {
                for (int i = 0; i < toggles.Length; i++)
                {
                    Toggle tog = toggles[i];

                    tog.onValueChanged.AddListener((b) =>
                    {
                        this.OnWeaponSelected(b, tog.GetComponent<WeaponShopItem>());
                    });
                    if(tog.isOn)
                    {
                        OnWeaponSelected(true, tog.GetComponent<WeaponShopItem>());
                    }
                }

            }
            toggleEventListened = true;
        }

    }

    public void OnWeaponSelected(bool b, WeaponShopItem item)
    {
        if(!b || item == null)
        {
            this.selectWeaponId = -1;
        }
        else
        {
            selectWeaponItem = item;
            if(weaponDisplay != null)
            {
                WeaponItem wi = WeaponManager.Instance.GetWeaponItemById(item.weaponId);
                if(wi == null)
                {
                    selectWeaponId = -1;
                }
                else
                {
                    selectWeaponId = wi.Id;
                    CommonUtils.SetChildText(weaponDisplay, "WeaponName", wi.Name);
                    CommonUtils.SetChildRawImage(weaponDisplay, "WeaponIcon", item.weaponIcon);
                    CommonUtils.SetChildText(weaponInfo, "WeaponName", wi.Name);
                    var weaponUnlockInfo = Player.CurrentPlayer.GetWeaponInfoById(wi.Id);
                    if(weaponUnlockInfo!=null && weaponUnlockInfo.IsUnlocked)
                    {
                        CommonUtils.SetChildActive(weaponDisplay, "Locked", false);
                        DisplayWeaponLevelInfo(wi.GetCurrentProperty(), true);
                    }
                    else
                    {
                        CommonUtils.SetChildActive(weaponDisplay, "Locked", true);
                        CommonUtils.SetChildText(weaponDisplay, "Locked/Price", wi.Prices.ToString());
                        DisplayWeaponLevelInfo(wi.GetCurrentProperty(), false);
                    }
                }
            }
        }
    }

    public void DisplayWeaponLevelInfo(WeaponProperty wp,bool unlocked)
    {

        CommonUtils.SetChildSliderValue(weaponInfo, "Infos/Power/Silder", wp.Power / GameGlobalValue.s_MaxWeaponAttack);
        CommonUtils.SetChildSliderValue(weaponInfo, "Infos/FireRate/Silder", (wp.FireRate) / GameGlobalValue.s_MaxFireRatePerSeconds);
        CommonUtils.SetChildSliderValue(weaponInfo, "Infos/MagSize/Silder", (float)wp.ClipSize / GameGlobalValue.s_MaxMagazineSize);
        CommonUtils.SetChildSliderValue(weaponInfo, "Infos/ScoreBouns/Silder", wp.ScoreBonus / GameGlobalValue.s_MaxSocreBonus);
        if(wp.UpgPrice >0)
        {
            CommonUtils.SetChildActive(weaponInfo, "Background/Upgrade", true);
            CommonUtils.SetChildActive(weaponInfo, "Background/TextMax", false);
            CommonUtils.SetChildText(weaponInfo, "Background/Upgrade/UpgPrice", wp.UpgPrice.ToString());
            
        }
        else
        {
            CommonUtils.SetChildActive(weaponInfo, "Background/Upgrade", false);
            CommonUtils.SetChildActive(weaponInfo, "Background/TextMax", true);
            CommonUtils.SetChildButtonActive(weaponInfo, "Background", true);
        }
        if (wp.UpgPrice > 0 && unlocked)
        {
            CommonUtils.SetChildButtonActive(weaponInfo, "Background", true);
        }
        else
        {
            CommonUtils.SetChildButtonActive(weaponInfo, "Background", false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


}
