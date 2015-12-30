using UnityEngine;
using System.Collections;

public class EnemyWander : MonoBehaviour {

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
    /// 运行速度
    /// </summary>
    public float speed = 1.0f;

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
    }

    #endregion


    /// <summary>
    /// 运行漫游
    /// </summary>
    public virtual void Run(Vector3 to)
    {

    }

    /// <summary>
    /// 设置动画变量的值
    /// </summary>
    /// <param name="type"></param>
    /// <param name="param"></param>
    /// <param name="val"></param>
    public void SetParamValue(object val = null)
    {
        if (animator == null)
            return;
        if(animParamType != AnimatorParameterType.Trigger)
        {
            if (val == null)
                return;
        }
        switch (animParamType)
        {
            case AnimatorParameterType.Bool:
                animator.SetBool(animParamName, ConvertUtil.ToBool(val));
                break;
            case AnimatorParameterType.Float:
                animator.SetFloat(animParamName, ConvertUtil.ToFloat(val));
                break;
            case AnimatorParameterType.Int:
                animator.SetInteger(animParamName, ConvertUtil.ToInt32(val));
                break;
            case AnimatorParameterType.Trigger:
                animator.SetTrigger(animParamName);
                break;
            default:break;
        }

    }
}
