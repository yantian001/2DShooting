using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnergyHandler : MonoBehaviour
{
    /// <summary>
    /// 当前能量值
    /// </summary>
    private int currentEnergy;
    /// <summary>
    /// 父对象
    /// </summary>
    public Transform UIEnergy;
    /// <summary>
    /// 特效
    /// </summary>
    public GameObject energyEffect;

    public Slider display;

    public int maxDisplayValue = 100;

    public int headshotEnergy = 1;
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

    #region Monobehavior
    // Use this for initialization
    void Start()
    {
        if (display != null)
        {
            display.maxValue = maxDisplayValue; 
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
    }

    public void OnDestroy()
    {
        LeanTween.removeListener((int)Events.ENERGYITEMUSED, OnEnergyItemUsed);
        LeanTween.removeListener((int)Events.EMENYDIE, OnEnemyDie);
        LeanTween.removeListener((int)Events.ENEMYCOMBO, OnEnemyCombo);
    }

    #endregion
    /// <summary>
    /// 监听连杀
    /// </summary>
    /// <param name="evt"></param>
    void OnEnemyCombo(LTEvent evt)
    {
        int combo = ConvertUtil.ToInt32(evt.data);
        if(combo >= killingSpreeCombo)
        {
            AddEnergy(killingSpreeEnergy);
        }
    }

    /// <summary>
    /// 监听敌人死亡
    /// </summary>
    /// <param name="evt"></param>
    void OnEnemyDie(LTEvent evt)
    {
        bool isHeadShot = ConvertUtil.ToBool(evt.data);
        if(isHeadShot)
        {
            AddEnergy(headshotEnergy);
        }
    }

    void AddEnergy(int value)
    {
        currentEnergy += value;
        if (display != null)
        {
            display.value = currentEnergy;
        }
        LeanTween.dispatchEvent((int)Events.ENERGYCHANGED, currentEnergy);
    }

    void OnEnergyItemUsed(LTEvent evt)
    {
        int cost = 0 - ConvertUtil.ToInt32(evt.data);
        AddEnergy(cost);
    }

}
