﻿using UnityEngine;
using System.Collections;
//using System.Collections.Generic;

[System.Serializable]
public struct WaveDataItem 
{
    [System.Serializable]
    public struct Burst
    {
        /// <summary>
        /// 产生时间
        /// </summary>
        public int time;
        /// <summary>
        /// 产生的数量
        /// </summary>
        public int count;
        /// <summary>
        /// 是否已完成
        /// </summary>
        public bool isFinish;
    }
    /// <summary>
    /// 敌人对象
    /// </summary>
    [Tooltip("敌人对象")]
    public GameObject emenyObject;
    /// <summary>
    /// 敌人数量
    /// </summary>
    [Tooltip("敌人数量")]
    public int emenyCount ;
    /// <summary>
    /// 产生的间隔时间
    /// </summary>
    public float rate ;

    /// <summary>
    /// 连续产生
    /// </summary>
   //[System.Serializable]
    public Burst[] bursts;

    /// <summary>
    /// 已经产生的数量
    /// </summary>
    int createdEmenyCount;

    float lastCreatedTime;
    /// <summary>
    /// 是否已经完成
    /// </summary>
    /// <returns></returns>
    public bool Cleared()
    {
        //bool burstCleared = true;
        //if(bursts!= null && bursts.Length > 0)
        //{
        //    for(int i= 0;i <bursts.Length; i++)
        //    {
        //        bursts[i].
        //    }
        //}
        return emenyCount <= createdEmenyCount;
    }

    /// <summary>
    /// 在时间点time,是否能产生相应数量的敌人
    /// </summary>
    /// <param name="time">时间点</param>
    /// <returns>可创建的敌人数量</returns>
    public int CanCreateAtTime(float time)
    {
        int createCount = 0;
        if (rate >= 0 && time -lastCreatedTime >= rate)
        {
            createCount += 1;
            lastCreatedTime = time;
        }
            

        if(bursts != null )
        {
            for(int i=0;i<bursts.Length;i++)
            {
                if(!bursts[i].isFinish)
                {
                    if(time >=bursts[i].time)
                    {
                        createCount += bursts[i].count;
                        bursts[i].isFinish = true;
                    }
                }
            }
        }

        createdEmenyCount += createCount;

        return createCount;

    }

    /// <summary>
    /// 重置
    /// </summary>
    public void Initialize()
    {
        createdEmenyCount = 0;
        lastCreatedTime = 0f;
        if(bursts != null)
        {
            for(int i=0;i<bursts.Length;i++)
            {
                bursts[i].isFinish = false;
            }
        }
    }
}
