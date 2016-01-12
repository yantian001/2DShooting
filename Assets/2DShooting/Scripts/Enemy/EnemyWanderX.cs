using UnityEngine;
using System.Collections;

public class EnemyWanderX : EnemyAction
{


    /// <summary>
    /// 每次移动的最大距离
    /// </summary>
    public float maxMoveDistance = 1.0f;
    /// <summary>
    /// 运行速度
    /// </summary>
    public float speed = 1.0f;
    /// <summary>
    /// 移动的距离
    /// </summary>
    public float distance = 0.0f;
    /// <summary>
    /// 最小横向可移动
    /// </summary>
    public float minMovementX = -5.3f;
    /// <summary>
    /// 最大横向可移动
    /// </summary>
    public float maxMovementX = 5.3f;

    public override void Start()
    {
        base.Start();
    }

    public override bool Run()
    {
        bool ret = false;
        do
        {
            if (!base.Run())
                break;
            distance = Random.Range(-maxMoveDistance, maxMoveDistance);
            //float to = transform.localPosition.x + distance;
            float worldx =transform.parent.TransformPoint(transform.localPosition).x;
            float to = this.ClampMovementX(worldx + distance);
            distance = to - worldx;
            float time = Mathf.Abs(distance) / speed;
            to = transform.localPosition.x + distance;
            LeanTween.moveLocalX(gameObject, to, time)
                .setOnStart(OnMoveStart)
                .setOnComplete(OnMoveComplete);

            ret = true;
        }
        while (false);
        return ret;
    }

    public virtual void OnMoveStart()
    {
        //this.GetType.t
        UpdateActionStatus(true);
        SetParamValue(true);
    }

    public virtual void OnMoveComplete()
    {
        UpdateActionStatus(false);
        SetParamValue(false);
    }

    public float ClampMovementX(float x)
    {
        return Mathf.Clamp(x, minMovementX, maxMovementX);
    }
}
