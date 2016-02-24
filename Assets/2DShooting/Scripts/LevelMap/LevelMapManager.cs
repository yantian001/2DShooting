using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class LevelMapManager : MonoBehaviour
{
    #region 变量 & 属性
    // Use this for initialization
    /// <summary>
    /// 所有场景的父对象
    /// </summary>
    [Tooltip("所有场景的父对象")]
    public GameObject Scenes;
    /// <summary>
    /// 场景缩略图
    /// </summary>
    public Image sceneThumb;
    /// <summary>
    /// 玩家名称显示
    /// </summary>
    public Text playerNameText;
    /// <summary>
    /// 玩家金钱数
    /// </summary>
    public Text playerMoneyText;
    /// <summary>
    /// 排行版
    /// </summary>
    public GameObject ranks;
    /// <summary>
    /// 排名显示区域
    /// </summary>
    public RectTransform rankZone;
    /// <summary>
    /// 排名显示对象
    /// </summary>
    public GameObject rankItem;
    /// <summary>
    /// 对象的高度
    /// </summary>
    public float rankItemHight = 50f;
    /// <summary>
    /// 我的排名的显示区域
    /// </summary>
    [Tooltip("我的排名的显示区域")]
    public Transform myRankInfo;
    /// <summary>
    /// 关卡
    /// </summary>
    public GameObject levels;
    /// <summary>
    /// 关卡区域
    /// </summary>
    public RectTransform levelZone;
    /// <summary>
    /// 关卡对象模板
    /// </summary>
    public GameObject levelItemTemplate;
    /// <summary>
    /// 武器选择区域
    /// </summary>
    private GameObject weaponSelect;
    /// <summary>
    /// 排行版区域
    /// </summary>
    public GameObject highScore;
    /// <summary>
    /// 武器信息
    /// </summary>
    private Transform weaponInfo;

    /// <summary>
    /// 武器UI
    /// </summary>
    public GameObject UIWeapon;

    /// <summary>
    /// 开始按钮
    /// </summary>
    public Button playButton;
    /// <summary>
    /// Store按钮
    /// </summary>
    public Button storeButton;

    /// <summary>
    /// 返回按钮
    /// </summary>
    public Button backButton;

    private int selectScene = -1;
    private int selectLevel = -1;
    private int selectWeaponId = -1;
    private GameDifficulty selectDifficulty = GameDifficulty.Normal;
    /// <summary>
    /// 当前选中的场景对象
    /// </summary>
    private LevelMapObject currentMapObject;
    /// <summary>
    /// 现有排名对象
    /// </summary>
    private List<GameObject> rankItems;

    private int action = 0;

    #endregion

    #region MonoBehaviour
    public void Start()
    {
        //更新显示
        //1.故事模式显示关卡
        //2.无尽模式显示排行榜
        if(GameGlobalValue.s_CurrentGameType == GameType.Endless)
        {
            ranks.SetActive(true);
            levels.SetActive(false);
        }
        else if(GameGlobalValue.s_CurrentGameType == GameType.Story)
        {
            ranks.SetActive(false);
            levels.SetActive(true);
        }

        //更新名称显示
        if (playerNameText != null)
        {
            playerNameText.text = Player.CurrentPlayer.UserName;
        }
        if (playerMoneyText != null)
        {
            playerMoneyText.text = Player.CurrentPlayer.Money.ToString();
        }
        //附加选择事件
        if (Scenes != null)
        {
            Toggle[] toggles = Scenes.GetComponentsInChildren<Toggle>();
            if (toggles != null && toggles.Length > 0)
            {
                for (int i = 0; i < toggles.Length; i++)
                {
                    Toggle tog = toggles[i];
                    tog.onValueChanged.AddListener((b) =>
                    {
                        OnToggleValueChange(b, tog.GetComponent<LevelMapObject>());
                    });
                    if (tog.isOn)
                    {
                        OnToggleValueChange(true, tog.GetComponent<LevelMapObject>());
                    }
                }
            }
        }

        //显示武器现在
        AddWeaponEvent();

        //开始按钮点击事件
        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }
        //ChangeUIDisplay(action);

        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
        }
        if(storeButton != null)
        {
            storeButton.onClick.AddListener(OnStoreButtonClicked);
        }

        //添加事件监听
        //AddEventListener();

        //显示广告
        ChartboostUtil.Instance.ShowInterstitialOnHomescreen();
    }

    private void OnStoreButtonClicked()
    {
        //throw new NotImplementedException();
        if(UIWeapon != null)
        {
            UIWeapon.SetActive(true);
        }
    }

    void OnBackButtonClicked()
    {
        //Debug.Log("back clicked");
        LeanTween.dispatchEvent((int)Events.BACKTOSTART);
    }

    public void OnDisable()
    {
        RemoveEventListener();
    }

    public void AddWeaponEvent()
    {
        if (weaponSelect != null)
        {
            Transform content = weaponSelect.transform.FindChild("WeaponList/WeaponContent");
            if (content != null)
            {
                Toggle[] weapons = content.GetComponentsInChildren<Toggle>();
                if (weapons != null && weapons.Length > 0)
                {
                    for (int i = 0; i < weapons.Length; i++)
                    {
                        Toggle weapon = weapons[i];
                        weapon.onValueChanged.AddListener(b =>
                        {
                            OnSelectWeaponChanged(b, weapon.GetComponent<LevelMapWeaponObject>());
                        });
                        if (weapon.isOn)
                        {
                            OnSelectWeaponChanged(true, weapon.GetComponent<LevelMapWeaponObject>());
                        }
                    }
                }
            }
        }

    }

    #endregion

    #region Events
    void AddEventListener()
    {
        //监听排行版更新
        LeanTween.addListener((int)Events.LEARDBOARDUPDATED, OnLeardBoardUpdated);
    }

    void RemoveEventListener()
    {
        LeanTween.removeListener((int)Events.LEARDBOARDUPDATED, OnLeardBoardUpdated);
    }

    /// <summary>
    /// 排行版更新事件
    /// </summary>
    /// <param name="evt"></param>
    void OnLeardBoardUpdated(LTEvent evt)
    {
        Debug.Log("LeardBoard Updated!");
        UpdateRankDisplay(currentMapObject);
    }

    /// <summary>
    /// 开始按钮点击
    /// </summary>
    void OnPlayButtonClicked()
    {
        //if (action == 0)
        //{
        //    action = 1;
        //    ChangeUIDisplay(action);
        //}
        //else
        //{
        if (selectScene != -1 && Player.CurrentPlayer.EquipedWeaponId != -1)
        {
            GameGlobalValue.s_CurrentScene = selectScene;
            GameGlobalValue.s_CurrentDifficulty = selectDifficulty;
            GameGlobalValue.s_currentWeaponId = Player.CurrentPlayer.EquipedWeaponId;
            if(GameGlobalValue.s_CurrentGameType == GameType.Story)
            {
                if(selectLevel == -1)
                {
                    selectLevel = Player.CurrentPlayer.GetSceneCurrentLevel(selectScene);
                }
                GameGlobalValue.s_CurrentLevel = selectLevel;
            }
            AnalysticUtil.TrackEvent("Scene Start", new Dictionary<string, object>() { { "Scene Id", selectScene } });
            GameLogic.Instance.Loading();
        }
        else
        {
            Message.PopupMessage("Please select scene first !");
        }
        // }

    }

    void OnToggleValueChange(bool selected, LevelMapObject mapObj)
    {
        if (selected)
        {
            //ChangeUIDisplay(0);
            if (mapObj)
            {
                currentMapObject = mapObj;
                selectScene = mapObj.level;
                selectLevel = -1;
                if(GameGlobalValue.s_CurrentGameType == GameType.Endless)
                {
                    //获得当前的排名
                    UpdateRankDisplay(mapObj);
                }
                else if (GameGlobalValue.s_CurrentGameType == GameType.Story)
                {
                    
                    UpdateLevelDisplay(mapObj);
                }
               
            }
        }
        else
        {
            selectScene = -1;
            //playButton.enabled = false;
        }
    }


    void OnSelectWeaponChanged(bool isOn, LevelMapWeaponObject weaponObject)
    {
        if (isOn && weaponObject)
        {
            //Weapon weapon = weaponObject.GetWeapon();
            // WeaponProperty wp = WeaponManager.Instance.GetCurrentPropertyById(weaponObject.weaponId);
            WeaponItem wi = WeaponManager.Instance.GetWeaponItemById(weaponObject.weaponId);
            if (wi != null)
            {
                selectWeaponId = wi.Id;
                //设置名字
                SetChildText(weaponSelect.GetComponent<RectTransform>(), "SelectedWeaponName", wi.Name);
                WeaponProperty wp = wi.GetCurrentProperty();
                if (wp != null)
                {
                    //设置武器攻击值
                    SetChildSliderValue(weaponInfo, "Pwoer", wp.Power / GameGlobalValue.s_MaxWeaponAttack);
                    //设置攻击次数
                    SetChildSliderValue(weaponInfo, "FireRate", (wp.FireRate) / GameGlobalValue.s_MaxFireRatePerSeconds);

                    //准确度
                    //float stab = 1.0f;
                    //if (weapon.randomShooting)
                    //{
                    //    stab -= weapon.randomShootingSize.x / GameGlobalValue.s_MaxShakeDistance;
                    //}

                    //SetChildSliderValue(weaponInfo, "Stability", stab);

                    //弹夹
                    SetChildSliderValue(weaponInfo, "Magazine", (float)wp.ClipSize / GameGlobalValue.s_MaxMagazineSize);

                    //移动速度
                    // SetChildSliderValue(weaponInfo, "Mobility", weapon.moveSpeed / GameGlobalValue.s_MaxMobility);
                    //得分能力
                    SetChildSliderValue(weaponInfo, "ScoreBouns", wp.ScoreBonus / GameGlobalValue.s_MaxSocreBonus);
                }
            }
        }
        else
        {
            selectWeaponId = -1;
        }
    }
    #endregion

    /// <summary>
    /// 更新关卡显示
    /// </summary>
    /// <param name="mapObj"></param>
    void UpdateLevelDisplay(LevelMapObject mapObj)
    {
        if (currentMapObject == null)
            return;
        if (levelItemTemplate == null)
            Debug.LogError("Miss Level Item Template!");
        levelZone.DetachChildren();
        var gridGroup = levelZone.GetComponent<GridLayoutGroup>();
        if(gridGroup != null)
        {
            int col = Mathf.FloorToInt((levelZone.rect.width - gridGroup.padding.left - gridGroup.padding.right + gridGroup.spacing.x) / (gridGroup.cellSize.x + gridGroup.spacing.x));
            int row = Mathf.CeilToInt((float)currentMapObject.levelCount / col);
            //levelZone.rect.Set(levelZone.rect.x,levelZone.rect.y,levelZone.rect.width, row * gridGroup.cellSize.y + gridGroup.padding.top);
            levelZone.sizeDelta = new Vector2(levelZone.sizeDelta.x, row * gridGroup.cellSize.y + gridGroup.padding.top +(row -1) * gridGroup.spacing.y);
            for(int i = 1;i < currentMapObject.levelCount + 1;i++)
            {
                var levelItem = Instantiate(levelItemTemplate) as GameObject;
                //levelItem.GetComponent<RectTransform>().SetParent(levelZone);
                levelItem.transform.SetParent(levelZone);
                levelItem.transform.localScale = new Vector3(1f, 1f, 1f);
                CommonUtils.SetChildText(levelItem.GetComponent<RectTransform>(), "Text", i.ToString());

                var button = levelItem.GetComponent<Button>();
                if(button)
                {
                    bool isUnlocked = Player.CurrentPlayer.IsLevelUnlocked(currentMapObject.level, i);
                    button.interactable = isUnlocked;
                    if(isUnlocked)
                    {
                        button.onClick.AddListener(()=>{ OnLevelItemClicked(button); });
                    }
                }
            }
        }
    }
    /// <summary>
    /// 关卡号点击事件
    /// </summary>
    void OnLevelItemClicked(Button btn)
    {
        var text = btn.GetComponentInChildren<Text>();
        if(text)
        {
            selectLevel = ConvertUtil.ToInt32(text.text, -1);
        }
       // selectLevel = level;
        OnPlayButtonClicked();
    }
    /// <summary>
    /// 更新排行版显示
    /// </summary>
    /// <param name="mapObj"></param>
    void UpdateRankDisplay(LevelMapObject mapObj)
    {
        if (currentMapObject == null)
            return;
        //显示玩家的排名
        // LevelScore Player.CurrentPlayer.
        UpdatePlayerLevelRank(mapObj.level);
        // rankZone.rect.Set(rankZone.rect.x, rankZone.rect.y, rankZone.rect.width, 1000f);
        //rankZone.sizeDelta = new Vector2(rankZone.sizeDelta.x,1000f)x;
        UpdateLevelLeardBoard(mapObj.LeardBoardID);


    }

    void UpdateLevelLeardBoard(string leardid)
    {
        if (rankZone == null || rankItem == null)
        {
            return;
        }
        rankZone.transform.DetachChildren();
        List<SocialObject> socials = SocialManager.Instance.GetObjectsById(leardid);


        if (socials != null)
        {
            int itemCount = socials.Count;
            if (itemCount > 0)
            {
                for (int i = 0; i < itemCount; i++)
                {
                    //添加排名
                    SocialObject social = socials[i];
                    if (social != null)
                    {
                        GameObject item = Instantiate(rankItem);
                        SetChildText(item.transform, "TextRank", social.Rank.ToString());
                        SetChildText(item.transform, "TextUserName", social.UserName);
                        SetChildText(item.transform, "TexBestScore", social.Score);
                        //item.transform.parent = rankZone.transform;
                        item.transform.SetParent(rankZone.transform);
                    }
                }
                //if (itemCount > 5)
                //{
                //    float itemHight = rankZone.GetComponent<VerticalLayoutGroup>().cellSize.y;

                //}
            }
            rankZone.sizeDelta = new Vector2(rankZone.sizeDelta.x, itemCount * rankItemHight);
        }
    }

    /// <summary>
    /// 更新用户场景排名
    /// </summary>
    /// <param name="level"></param>
    void UpdatePlayerLevelRank(int level)
    {
        if (myRankInfo != null)
        {
            SetChildText(myRankInfo, "TextMyName", Player.CurrentPlayer.UserName);
        }
        LevelScore score = Player.CurrentPlayer.GetScoreByLevel(level);
        if (score != null)
        {
            SetChildText(myRankInfo, "TextMyScore", score.BestScore.ToString());
            if (score.Rank <= 0)
            {
                SetChildText(myRankInfo, "TextMyRank", "-");
            }
            else
            {
                SetChildText(myRankInfo, "TextMyRank", score.Rank.ToString());
            }
        }
        // if(score)
    }

    public void ChangeUIDisplay(int _action)
    {
        action = _action;
        //显示排行榜
        if (action == 0)
        {
            highScore.SetActive(true);
            weaponSelect.SetActive(false);
        }
        else if (action == 1)
        {
            highScore.SetActive(false);
            weaponSelect.SetActive(true);
            AddWeaponEvent();
        }
    }

    /// <summary>
    /// 设置子对象的文本值
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    /// <param name="value"></param>
    void SetChildText(Transform parent, string child, string value)
    {
        Transform childTran = parent.FindChild(child);
        if (childTran != null)
        {
            Text childText = childTran.GetComponent<Text>();
            if (childText)
            {
                childText.text = value;
            }
        }
    }
    /// <summary>
    /// 设置子对象的Slider值
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    /// <param name="value"></param>
    void SetChildSliderValue(Transform parent, string child, float value)
    {
        Transform childTran = parent.FindChild(child);
        if (childTran)
        {
            Slider slider = childTran.GetComponentInChildren<Slider>();
            if (slider)
            {
                slider.value = value;
            }
        }
    }
}
