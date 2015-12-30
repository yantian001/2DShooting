using UnityEngine;
using System.Collections;

public class EnemyWanderRollX : EnemyWanderX {

    public override void Start()
    {
        base.Start();
        this.animParamType = AnimatorParameterType.Bool;
        this.animParamName = "isRolling";
    }

    public override void OnMoveStart()
    {
        base.OnMoveStart();
    }

    public override void Run()
    {
        base.Run();
    }
}
