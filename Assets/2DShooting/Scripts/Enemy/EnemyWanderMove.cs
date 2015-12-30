using UnityEngine;
using System.Collections;

public class EnemyWanderMove : EnemyWander
{

    public override void Start()
    {
        base.Start();
        animParamName = "isWalking";
        animParamType = AnimatorParameterType.Bool;
    }

    /// <summary>
    /// 运行
    /// </summary>
    public override void Run(Vector3 to)
    {
        float distance = Vector3.Distance(transform.position, to);
        float time = distance / speed;

        //run
        LeanTween.move(gameObject, to, time)
            .setOnStart(() =>
            {
                if (animator != null)
                {
                    SetParamValue(true);
                }
            })
            .setOnComplete(() => {
                SetParamValue(false);
            });
    }
   
}
