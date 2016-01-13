using UnityEngine;
using System.Collections;


public class WeaponItem  {
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
    /// 武器当前级别
    /// </summary>
    public int Level = 0;
    /// <summary>
    /// 武器级别属性
    /// </summary>
    public WeaponProperty[] Levels;
}
