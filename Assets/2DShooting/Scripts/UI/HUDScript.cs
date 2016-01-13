using UnityEngine;
using System.Collections;

public class HUDScript : MonoBehaviour
{
    #region 敌人伤害
    [Tooltip("敌人伤害prefab")]
    public bl_HUDText hudRoot ;

    public Transform HUDCanvas;
    /// <summary>
    /// 普通伤害字体颜色
    /// </summary>
    public Color normalHUDColor = Color.red;
    /// <summary>
    /// 爆头伤害字体颜色
    /// </summary>
    public Color headshotHUDColor =Color.red;

    void OnDemaged(LTEvent evt)
    {
        if (evt.data != null)
        {
            Demage d = evt.data as Demage;
            if (d != null && hudRoot != null)
            {

                if (d.isEnemy)
                {
                    if (d.isHeadShot)
                    {
                        if (Random.Range(0, 2) == 1)
                        {
                            hudRoot.NewText("- " + d.demageVal.ToString(), d.tran, headshotHUDColor, 10, 10f, 1f, 2.5f, bl_Guidance.RightDown);
                        }
                        else
                        {
                            hudRoot.NewText("- " + d.demageVal.ToString(), d.tran, headshotHUDColor, 10, 10f, 1f, 2.5f, bl_Guidance.LeftDown);
                        }
                    }
                    else
                    {
                        if (Random.Range(0, 2) == 1)
                        {
                            hudRoot.NewText("- " + d.demageVal.ToString(), d.tran, normalHUDColor, 10, 10f, -1f, 2.2f, bl_Guidance.RightDown);
                        }
                        else
                        {
                            hudRoot.NewText("- " + d.demageVal.ToString(), d.tran, normalHUDColor, 10, 10f, -1f, 2.2f, bl_Guidance.LeftDown);
                        }
                    }

                }

            }
        }
    }

    #endregion

    #region Monobehavior

    void Awake()
    {
        LeanTween.addListener(gameObject, (int)Events.DEMAGE, OnDemaged);
    }

    public void OnDisable()
    {
        LeanTween.removeListener(gameObject, (int)Events.DEMAGE, OnDemaged);
    }



    // Use this for initialization
    void Start()
    {
        if (hudRoot.CanvasParent == null)
            hudRoot.CanvasParent = HUDCanvas;
    }
   
    #endregion
}
