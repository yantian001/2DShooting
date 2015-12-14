
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
    GAMERESTART,
    /// <summary>
    /// 返回主页
    /// </summary>
    MAINMENU,
    /// <summary>
    /// 击中了道具  医疗包 ,附带参数为回复的血量
    /// </summary>
    ITEMMEDKITHIT,
    /// <summary>
    /// 击中了道具  盾牌 , 附带盾牌值
    /// </summary>
    ITEMSHIELDHIT,
    /// <summary>
    /// 排行榜更新了
    /// </summary>
    LEARDBOARDUPDATED

}