using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnergyHandler : MonoBehaviour
{

    enum IconType
    {
        /// <summary>
        /// 爆头
        /// </summary>
        HeadShot = 0,
        /// <summary>
        /// 大开杀戒
        /// </summary>
        Slaughter,
        /// <summary>
        /// 波数完成
        /// </summary>
        WaveComplete
    }

    /// <summary>
    /// 当前能量值
    /// </summary>
    private int currentEnergy;
    /// <summary>
    /// 父对象
    /// </summary>
    public Transform UIEnergy;
    ///// <summary>
    ///// 特效
    ///// </summary>
    //public GameObject energyEffect;
    /// <summary>
    /// 是否显示获得能量的图标
    /// </summary>
    [Tooltip("是否显示获得能量的图标")]
    public bool isShowIcon = false;
    /// <summary>
    /// 图标显示位置
    /// </summary>
    public Transform iconPlace;
    ///// <summary>
    ///// 效果目的地
    ///// </summary>
    //public Transform effectTarget;

    public Slider display;

    public Color displayFlashColor = Color.white;

   public Color displayCurrentColor;
    public int maxDisplayValue = 100;

    public int headshotEnergy = 1;
    /// <summary>
    /// 爆头图标
    /// </summary>
    public GameObject headshotIcon;
    /// <summary>
    /// 大开杀戒人数
    /// </summary>
    [Tooltip("连杀多少人时获得大开杀戒")]
    public int killingSpreeCombo = 5;
    /// <summary>
    /// 大开杀戒能量
    /// </summary>
    [Tooltip("大开杀戒获得的能量")]
    public int killingSpreeEnergy = 2;
    /// <summary>
    /// 大开杀戒图标
    /// </summary>
    public GameObject killingSpreeIcon;

    /// <summary>
    /// 波数完成
    /// </summary>
    public GameObject waveCompleteIcon;
    /// <summary>
    /// 每秒减少的能量
    /// </summary>
    [Tooltip("每秒减少的能量")]
    public int reducePerSecond = 2;


    bool isExitPower = false;

    #region Monobehavior
    // Use this for initialization
    void Start()
    {
        if (display != null)
        {
            display.maxValue = maxDisplayValue;
        }
        if (display != null)
        {
            var image = display.targetGraphic;
            if (image != null)
                displayCurrentColor = image.color;
        }
        AddEnergy(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnEnable()
    {
        LeanTween.addListener((int)Events.ENERGYITEMUSED, OnEnergyItemUsed);
        LeanTween.addListener((int)Events.EMENYDIE, OnEnemyDie);
        LeanTween.addListener((int)Events.ENEMYCOMBO, OnEnemyCombo);
        LeanTween.addListener((int)Events.ENERGYPOWERIN, OnEnterEnergyPower);
        LeanTween.addListener((int)Events.ENERGYPOWEROUT, OnExitEnergyPower);
        LeanTween.addListener((int)Events.WAVECOMPLETED, OnWaveCompleted);

    }

    public void OnDestroy()
    {
        LeanTween.removeListener((int)Events.ENERGYITEMUSED, OnEnergyItemUsed);
        LeanTween.removeListener((int)Events.EMENYDIE, OnEnemyDie);
        LeanTween.removeListener((int)Events.ENEMYCOMBO, OnEnemyCombo);
        LeanTween.removeListener((int)Events.ENERGYPOWERIN, OnEnterEnergyPower);
        LeanTween.removeListener((int)Events.ENERGYPOWEROUT, OnExitEnergyPower);
        LeanTween.removeListener((int)Events.WAVECOMPLETED, OnWaveCompleted);
    }

    void OnWaveCompleted(LTEvent evt)
    {
        int wave = ConvertUtil.ToInt32(evt.data);
        if (wave > 0)
        {
            AddEnergy(wave);
            ShowIcon(IconType.WaveComplete);
        }
    }

    void OnEnterEnergyPower(LTEvent evt)
    {
        isExitPower = false;
        StartCoroutine(DecreaseEnergy());
    }


    IEnumerator DecreaseEnergy()
    {
        while (currentEnergy >= reducePerSecond && !isExitPower)
        {
            yield return new WaitForSeconds(1f);
            AddEnergy(0 - reducePerSecond);

        }
        //isExitPower = true;
        if (!isExitPower)
            LeanTween.dispatchEvent((int)Events.ENERGYPOWEROUT);
    }

    void OnExitEnergyPower(LTEvent evt)
    {
        isExitPower = true;
    }
    #endregion
    /// <summary>
    /// 监听连杀
    /// </summary>
    /// <param name="evt"></param>
    void OnEnemyCombo(LTEvent evt)
    {
        int combo = ConvertUtil.ToInt32(evt.data);
        if (combo >= killingSpreeCombo)
        {
            AddEnergy(killingSpreeEnergy);
            ShowIcon(IconType.Slaughter);
        }
    }

    /// <summary>
    /// 监听敌人死亡
    /// </summary>
    /// <param name="evt"></param>
    void OnEnemyDie(LTEvent evt)
    {
        bool isHeadShot = ConvertUtil.ToBool(evt.data);
        if (isHeadShot)
        {
            AddEnergy(headshotEnergy);
            ShowIcon(IconType.HeadShot);
        }
    }


    void ShowIcon(IconType type)
    {
        if (!isShowIcon || iconPlace == null)
            return;
        GameObject icon = null;
        if (type == IconType.HeadShot)
        {
            if (headshotIcon != null)
            {
                icon = Instantiate(headshotIcon);
            }
        }
        else if (type == IconType.Slaughter)
        {
            if (headshotIcon != null)
            {
                icon = Instantiate(killingSpreeIcon);
            }
        }
        else if (type == IconType.WaveComplete)
        {
            if (headshotIcon != null)
            {
                icon = Instantiate(waveCompleteIcon);
            }
        }
        if (icon != null)
        {
            //icon.transform.SetParent(iconPlace);
            CommonUtils.CopyRectTransfrom(iconPlace.GetComponent<RectTransform>(), icon.GetComponent<RectTransform>());
            icon.transform.localScale = new Vector3(1f, 1f, 1f);
            //if(energyEffect != null)
            //{
            //    var effect = Instantiate(energyEffect) as GameObject;
            //    CommonUtils.CopyRectTransfrom(iconPlace.GetComponent<RectTransform>(), effect.GetComponent<RectTransform>());
            //    //effect.transform.SetParent(iconPlace);
            //    //effect.transform.localScale = new Vector3(1f, 1f, 1f);
            //    LeanTween.move(effect, effectTarget.position, 1f).setDestroyOnComplete(true);
            //}
        }
    }


    void AddEnergy(int value)
    {
        currentEnergy += value;
        if (display != null)
        {
            SetDisplayColor(displayFlashColor);
            display.value = currentEnergy;
            SetDisplayColor(displayCurrentColor);

        }
        LeanTween.dispatchEvent((int)Events.ENERGYCHANGED, currentEnergy);
    }

    void SetDisplayColor(Color c)
    {
        var graphic = display.targetGraphic;
        if(graphic != null)
        {
            graphic.color = c;
        }
    }

    void OnEnergyItemUsed(LTEvent evt)
    {
        int cost = 0 - ConvertUtil.ToInt32(evt.data);
        AddEnergy(cost);
    }

}
