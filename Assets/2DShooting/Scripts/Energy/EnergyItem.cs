using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnergyItem : MonoBehaviour
{

    /// <summary>
    /// 能量花费
    /// </summary>
    public int energyCost;
    /// <summary>
    /// 使用按钮
    /// </summary>
    public Button useButton;
    /// <summary>
    /// 激活时晃动
    /// </summary>
    public bool shakeOnEnable;

    bool enabled = false;
    // Use this for initialization
    void Start()
    {
        if (useButton == null)
        {
            useButton = GetComponent<Button>();
        }
        if (useButton != null)
        {
            useButton.onClick.AddListener(OnItemUse);
        }
    }

    public virtual void OnEnable()
    {
        
        LeanTween.addListener((int)Events.ENERGYCHANGED, OnEnergyChanged);
    }

    public virtual void OnDisable()
    {
        LeanTween.removeListener((int)Events.ENERGYCHANGED, OnEnergyChanged);
    }

    protected virtual void OnEnergyChanged(LTEvent evt)
    {
        int currentEnergy = 0;
        if (evt.data != null)
        {
            currentEnergy = ConvertUtil.ToInt32(evt.data);
        }
        enabled = currentEnergy >= energyCost;
        bool statusChange = false;
        if(useButton.interactable != enabled)
        {
            statusChange = true;
        }
        if (statusChange && enabled && shakeOnEnable)
        {
            iTween.ShakeScale(gameObject, new Vector3(1.2f, 1.2f, 1.2f), 0.5f);
            SoundManager.Instance.PlaySound(SoundManager.SoundType.EnergyItemEnabled);
        }
            
        useButton.interactable = (currentEnergy >= energyCost);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void OnItemUse()
    {
        LeanTween.dispatchEvent((int)Events.ENERGYITEMUSED, energyCost);
    }
}
