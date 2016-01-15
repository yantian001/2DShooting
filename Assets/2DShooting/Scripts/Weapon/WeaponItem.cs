using UnityEngine;
using System.Collections;


public class WeaponItem
{
    /// <summary>
    /// 武器ID
    /// </summary>
    public int Id;
    /// <summary>
    /// 武器名字
    /// </summary>
    public string Name;
    /// <summary>
    /// 是否默认可以使用
    /// </summary>
    public bool IsDefault;
    /// <summary>
    /// 是否已购买
    /// </summary>
    public bool IsEnabled;
    /// <summary>
    /// 武器价格
    /// </summary>
    public int Prices;
    /// <summary>
    /// 武器当前级别
    /// </summary>
    public int Level = 0;
    /// <summary>
    /// 武器级别属性
    /// </summary>
    public WeaponProperty[] Levels;

    /// <summary>
    /// 获取当前武器等级信息
    /// </summary>
    /// <returns></returns>
    public WeaponProperty GetCurrentProperty()
    {
        if (Level < Levels.Length)
            return Levels[Level];
        return null;
    }
    /// <summary>
    /// 获取武器等级信息
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public WeaponProperty GetLevelProperty(int level)
    {
        if (level >= 0 && level < Levels.Length)
            return Levels[level];
        return null;
    }
    /// <summary>
    /// 是否解锁了.
    /// </summary>
    /// <returns></returns>
    public bool Enabled()
    {
        return IsDefault || IsEnabled;
    }
}
