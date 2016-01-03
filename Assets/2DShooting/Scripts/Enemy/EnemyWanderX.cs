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

    public override void Start()
    {
        base.Start();
    }

    public override void Run()
    {
        base.Run();
        distance = Random.Range(-maxMoveDistance, maxMoveDistance);
        float to = transform.localPosition.x + distance;
        to = EmenyController.Instance.ClampMoveX(to);
        distance = to - transform.localPosition.x;
        float time = Mathf.Abs(distance) / speed;

        // bool isWalkLeft = distance < 0;
        //run
        //LeanTween.moveX()
        LeanTween.moveLocalX(gameObject, to, time)
            .setOnStart(OnMoveStart)
            .setOnComplete(OnMoveComplete);
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
}
