using UnityEngine;
using System.Collections;

public class EnemyWanderMoveX : EnemyWanderX
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
    public override void OnMoveStart()
    {
        
        bool isWalkLeft = distance < 0;
        SetParamValue("isWalkLeft", AnimatorParameterType.Bool, isWalkLeft);
        base.OnMoveStart();
    }

    public override bool Run()
    {
       return base.Run();
    }
}
