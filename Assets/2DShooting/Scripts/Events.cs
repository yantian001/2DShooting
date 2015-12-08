
public enum Events
{
    RELOAD,//换子弹
    BULLETCHANGED, //子弹数量变化了
    /// <summary>
    /// 人物受到攻击 , 附带攻击值
    /// </summary>
    PLAYERINJURED,
    /// <summary>
    /// 任务成功
    /// </summary>
    GAMESUCCESS,
    /// <summary>
    /// 任务失败
    /// </summary>
    GAMEFAILED,
    /// <summary>
    /// 游戏暂停
    /// </summary>
    GAMEPAUSED,
    /// <summary>
    /// 游戏重新开始
    /// </summary>
    GAMERESTART
}