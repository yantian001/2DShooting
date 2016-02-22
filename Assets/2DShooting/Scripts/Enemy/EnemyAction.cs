using UnityEngine;
using System.Collections;

public class EnemyAction : MonoBehaviour {

    /// <summary>
    /// 动画控制器
    /// </summary>
   public Animator animator = null;
    /// <summary>
    /// 动画控制器在子节点
    /// </summary>
    public bool animatorInChildren = false;
    /// <summary>
    /// 动画变量名称
    /// </summary>
    public string animParamName = "";
    /// <summary>
    /// 动画变量的类型
    /// </summary>
    public AnimatorParameterType animParamType;
    /// <summary>
    /// 动作的权重
    /// </summary>
    public int weight = 1;
    /// <summary>
    /// 所属的Enemy类
    /// </summary>
    public GAFEnemy enemy = null;
    /// <summary>
    /// 是否主要动作?主动作的权重会随圈数增强
    /// </summary>
    public bool isMainAction = false;

    bool isDie = false;

    #region MonoBehaviour Function
    public virtual void Start()
    {
        
        if(animator == null)
        {
            if(animatorInChildren)
            {
                animator = GetComponentInChildren<Animator>();
            }
            else
            {
                animator = GetComponent<Animator>();
            }
        }

        if(enemy == null)
        {
            enemy = GetComponent<GAFEnemy>();
            //GAFEnemyEvent
        }
    }

    #endregion


    /// <summary>
    /// 运行动作
    /// </summary>
    public virtual bool Run()
    {
        return enabled;
    }

    /// <summary>
    /// 设置动画变量的值
    /// </summary>
    /// <param name="type"></param>
    /// <param name="param"></param>
    /// <param name="val"></param>
    public void SetParamValue(object val = null)
    {
        SetParamValue(this.animParamName, this.animParamType, val);
    }

    /// <summary>
    /// 设置动画的变量值
    /// </summary>
    /// <param name="paramName"></param>
    /// <param name="type"></param>
    /// <param name="val"></param>
    public void SetParamValue(string paramName , AnimatorParameterType type ,object val = null)
    {
        if (animator == null)
            return;
        if (type != AnimatorParameterType.Trigger)
        {
            if (val == null)
                return;
        }
        switch (type)
        {
            case AnimatorParameterType.Bool:
                animator.SetBool(paramName, ConvertUtil.ToBool(val));
                break;
            case AnimatorParameterType.Float:
                animator.SetFloat(paramName, ConvertUtil.ToFloat(val));
                break;
            case AnimatorParameterType.Int:
                animator.SetInteger(paramName, ConvertUtil.ToInt32(val));
                break;
            case AnimatorParameterType.Trigger:
                animator.SetTrigger(paramName);
                break;
            default: break;
        }

    }

    public virtual void UpdateActionStatus(bool isRunning)
    {
        if(enemy)
        {
            enemy.UpdateAction(isRunning, this.GetType().ToString());
        }
    }

    public virtual void EnemyDie()
    {
        isDie = true;
    }
}
