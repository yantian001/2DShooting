using UnityEngine;
using System.Collections;

public class WaveData : ScriptableObject {

    /// <summary>
    /// 敌人信息
    /// </summary>
    public WaveDataItem[] dataItems;

    /// <summary>
    /// 初始化
    /// </summary>
    public void Initialize()
    {
        if(dataItems != null && dataItems.Length > 0)
        {
            for(int i= 0;i<dataItems.Length;i++)
            {
                dataItems[i].Initialize();
            }
        }
    }
}
