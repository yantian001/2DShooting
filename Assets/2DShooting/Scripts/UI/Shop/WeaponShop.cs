using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponShop : MonoBehaviour
{
    public RectTransform weaponContent;

    public RectTransform weaponDisplay;

    public RectTransform weaponInfo;

    public RectTransform buttons;

    WeaponShopItem selectWeaponItem;

    bool toggleEventListened = false;

    int selectWeaponId = -1;
    // Use this for initialization
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();
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

                    WeaponShopItem wsi = tog.GetComponent<WeaponShopItem>();
                    if (wsi != null)
                    {
                        tog.onValueChanged.AddListener((b) =>
                        {
                            this.OnWeaponSelected(b, tog.GetComponent<WeaponShopItem>());
                        });
                        if (wsi.weaponId == Player.CurrentPlayer.EquipedWeaponId)
                        {
                            tog.isOn = true;
                        }
                    }
                }

            }
            toggleEventListened = true;
        }

    }

    /// <summary>
    /// 选择武器变化
    /// </summary>
    /// <param name="b"></param>
    /// <param name="item"></param>
    public void OnWeaponSelected(bool b, WeaponShopItem item)
    {
        if (!b || item == null)
        {
            this.selectWeaponId = -1;
        }
        else
        {
            selectWeaponItem = item;
            if (weaponDisplay != null)
            {
                WeaponItem wi = WeaponManager.Instance.GetWeaponItemById(item.weaponId);
                if (wi == null)
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

                    if (weaponUnlockInfo != null && weaponUnlockInfo.IsUnlocked)
                    {
                        CommonUtils.SetChildActive(weaponDisplay, "Locked", false);
                        DisplayWeaponLevelInfo(wi.GetLevelProperty(weaponUnlockInfo.Level), true);
                    }
                    else
                    {
                        CommonUtils.SetChildActive(weaponDisplay, "Locked", true);
                        CommonUtils.SetChildText(weaponDisplay, "Locked/Price", wi.Prices.ToString());
                        DisplayWeaponLevelInfo(wi.GetCurrentProperty(), false);
                    }

                    //显示按钮
                    if (Player.CurrentPlayer.EquipedWeaponId == wi.Id)
                    {
                        //CommonUtils.SetChildActive(buttons, "BtnEquiped",true);
                        SetButtonActive("BtnEquiped");
                    }
                    else
                    {
                        if (weaponUnlockInfo != null && weaponUnlockInfo.IsUnlocked)
                        {
                            SetButtonActive("BtnEquip");
                        }
                        else
                        {
                            SetButtonActive("BtnBuy");
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// 设置Button的显示
    /// </summary>
    /// <param name="btnName"></param>
    public void SetButtonActive(string btnName)
    {
        // buttons.gameObject.g

        //Button[] btns = buttons.gameObject.GetComponentsInChildren<Button>();
        //if(btns !=null && btns.Length > 0 )
        //{
        //    for(int i =0;i<btns.Length;i++)
        //    {
        //        if(btns[i].name == btnName)
        //        {
        //            btns[i].gameObject.SetActive(true);
        //        }
        //        else
        //        {
        //            btns[i].gameObject.SetActive(false);
        //        }
        //    }
        //}
        for (int i = 0; i < buttons.childCount; i++)
        {
            if (buttons.GetChild(i).name == btnName)
            {
                buttons.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                buttons.GetChild(i).gameObject.SetActive(false);

            }
        }
    }

    public void DisplayWeaponLevelInfo(WeaponProperty wp, bool unlocked)
    {

        CommonUtils.SetChildSliderValue(weaponInfo, "Infos/Power/Silder", wp.Power / GameGlobalValue.s_MaxWeaponAttack);
        CommonUtils.SetChildSliderValue(weaponInfo, "Infos/FireRate/Silder", (wp.FireRate) / GameGlobalValue.s_MaxFireRatePerSeconds);
        CommonUtils.SetChildSliderValue(weaponInfo, "Infos/MagSize/Silder", (float)wp.ClipSize / GameGlobalValue.s_MaxMagazineSize);
        CommonUtils.SetChildSliderValue(weaponInfo, "Infos/ScoreBouns/Silder", wp.ScoreBonus / GameGlobalValue.s_MaxSocreBonus);
        if (wp.UpgPrice > 0)
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

    /// <summary>
    /// 购买武器
    /// </summary>
    public void OnWeaponBuy()
    {
        //Player.CurrentPlayer.UseMoney(-10000);
        if (selectWeaponId != -1)
        {
            var wi = WeaponManager.Instance.GetWeaponItemById(selectWeaponId);
            if (!wi.Enabled())
            {
                if (Player.CurrentPlayer.Money >= wi.Prices)
                {
                    //Player.CurrentPlayer.UpgradeWeapon(selectWeaponId, -wp.UpgPrice);
                    WeaponManager.Instance.WeaponBuy(selectWeaponId);
                    SoundManager.Instance.PlaySound(SoundManager.SoundType.WeaponBought);
                    OnWeaponSelected(true, selectWeaponItem);
                }
                else
                {
                    Message.PopupMessage("NOT ENOUGH CASH!", 2f);
                }
            }
            else
            {
                Message.PopupMessage("Bought!!!", 2f);
            }

        }
        else
        {
            Message.PopupMessage("PLEASE SELECT WEAPON FIRST!", 2f);
        }
    }

    public void OnWeaponUpgrade()
    {
        if (selectWeaponId != -1)
        {
            var wp = WeaponManager.Instance.GetCurrentPropertyById(selectWeaponId);
            if (wp != null)
            {
                if (wp.UpgPrice > 0)
                {
                    if (Player.CurrentPlayer.Money >= wp.UpgPrice)
                    {
                        //Player.CurrentPlayer.UpgradeWeapon(selectWeaponId, -wp.UpgPrice);
                        WeaponManager.Instance.WeaponUpgrade(selectWeaponId);
                        SoundManager.Instance.PlaySound(SoundManager.SoundType.WeaponUpgrade);
                        OnWeaponSelected(true, selectWeaponItem);
                    }
                    else
                    {
                        Message.PopupMessage("NOT ENOUGH CASH!", 2f);
                    }
                }
                else
                {
                    Message.PopupMessage("MAX", 2f);
                }
            }
        }
        else
        {
            Message.PopupMessage("PLEASE SELECT WEAPON FIRST!", 2f);
        }
    }

    public void OnWeaponEquiped()
    {
        if (selectWeaponId != -1)
        {
            Player.CurrentPlayer.EquipWeapon(selectWeaponId);
            SoundManager.Instance.PlaySound(SoundManager.SoundType.WeaponEqiuped);
            OnWeaponSelected(true, selectWeaponItem);
        }

    }

    public void OnButtonCloseClicked()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }


}
