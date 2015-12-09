using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameItemManager : MonoBehaviour {

    /// <summary>
    /// 产生道具的间隔
    /// </summary>
    [Tooltip("产生道具的间隔时间")]
    public float spwanInterval = 30f;
    /// <summary>
    /// 是否随机道具间隔时间
    /// </summary>
    [Tooltip("是否使用随机值")]
    public bool useRandomInterval = false;
    /// <summary>
    /// 随机值
    /// </summary>
    [Tooltip("随机值")]
    public float randomIntervalVal = 10f;
    /// <summary>
    /// 场景中同时存在的道具数
    /// </summary>
    public int maxExsitItem = 1;
    /// <summary>
    /// 产生道具的位置
    /// </summary>
    [Tooltip("产生道具的位置")]
    public List<Transform> spwanTranforms;
    /// <summary>
    /// 道具集合
    /// </summary>
    [Tooltip("可产生的道具")]
    public GameObject[] spwanItems;

    private float timeFromLastSpwan = 0.0f;

    private float currentSpwanInterval = 0.0f;

    #region MonoBehaviour Method
    // Use this for initialization
    void Start () {
        timeFromLastSpwan = 0.0f;

        //取得当前产生间隔
        GetInterval();
	}

    void GetInterval()
    {
        currentSpwanInterval = spwanInterval;
        if (useRandomInterval)
        {
            currentSpwanInterval += Random.Range(-randomIntervalVal, randomIntervalVal);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(CanSpwanItem())
        {
            timeFromLastSpwan += Time.deltaTime;

            if(timeFromLastSpwan >= currentSpwanInterval)
            {
                SpwanItem();
            }
        }
	}
    
    #endregion

    /// <summary>
    /// 产生道具
    /// </summary>
    void SpwanItem()
    {
        Transform spwPos = this.GetSpwanPosition();
        GameObject obj = this.GetSpwanObject();
        if(spwPos != null && obj != null)
        {
            GameObject newobj = Instantiate(obj, spwPos.position, Quaternion.identity) as GameObject;
            newobj.transform.localScale = spwPos.localScale;
            newobj.transform.parent = spwPos;
            //newobj.transform = spwPos;
        }
        timeFromLastSpwan -= currentSpwanInterval;
        GetInterval();
    }
    /// <summary>
    /// 获得道具产生的位置
    /// </summary>
    /// <returns></returns>
    Transform GetSpwanPosition()
    {
        Transform tran = null;
        List<Transform> newList = spwanTranforms.FindAll(p => {
            return p.GetComponentsInChildren<GameItem>().Length < 1;
        });
        if(newList!=null && newList.Count > 0)
        {
            tran = newList[(int)Random.Range(0, newList.Count - 1)];
        }
        return tran;
    }

    /// <summary>
    /// 获取产生的道具对象
    /// </summary>
    /// <returns></returns>
    GameObject GetSpwanObject()
    {
        GameObject obj = null;

        if(spwanItems != null && spwanItems.Length > 0)
        {
            obj = spwanItems[(int)Random.Range(0, spwanItems.Length - 1)];
        }
        return obj;
    }

    /// <summary>
    /// 是否可以产生道具
    /// </summary>
    /// <returns></returns>
    bool CanSpwanItem()
    {
        if(!GameManager.Instance.IsInGame())
        {
            return false;
        }
        if(spwanItems ==null || spwanItems.Length < 1 )
        {
            return false;
        }

        if(spwanTranforms == null || spwanTranforms.Count < 1)
        {
            return false;
        }

        if (GetItemCount() >= maxExsitItem)
        {
            return false;
        }
        return true;
       
    }
    /// <summary>
    /// 获得当前存在的道具数
    /// </summary>
    /// <returns></returns>
    int GetItemCount()
    {
        int count = 0;
        if(spwanTranforms != null)
        {
            for (int i = 0; i < spwanTranforms.Count; i++)
            {
                count += spwanTranforms[i].GetComponentsInChildren<GameItem>().Length;
            }
        }
        return count;
    }
}
