using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GAFInternal.Objects;
using GAF.Core;

public class EmenyController : MonoBehaviour
{

    ////敌人对象
    //public GameObject[] enemys;
    //敌人产生的位置
    public List<GameObject> enemySpwanPosition;

    //敌人产生的间隔
    public float spwanInterval = 0.5f;

    [HideInInspector]
    public GameData gameData { get; set; }
    /// <summary>
    /// 每圈增强系数
    /// </summary>
    public float enhancePerTurn = 0.2f;
    /// <summary>
    /// 每波增强的系数
    /// </summary>
    public float enhancePerWave = 0.1f;

    /// <summary>
    /// 当前波数信息
    /// </summary>
    [HideInInspector]
    public WaveData waveData = null;

    int currentWave = 1;

    int currentTurn = 1;
    /// <summary>
    /// 当前敌人数
    /// </summary>
    int aliveEnemyCount = 0;
    /// <summary>
    /// 需要产生的敌人数
    /// </summary>
    int needCreateEnemy = 0;

    float timeSinceSpwan = 0.0f;

    /// <summary>
    /// 该波数已运行的时间
    /// </summary>
    float timeSinceWaveStart = 0.0f;
    /// <summary>
    /// 单例模式
    /// </summary>
    public static EmenyController Instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    private static EmenyController _instance;

    #region MonoBehavior
    public void Awake()
    {
        _instance = this;

        //添加事件监听
        LeanTween.addListener((int)Events.EMENYDIE, OnEmenyDie);
    }


    public void OnDestroy()
    {
        _instance = null;
    }

    void Start()
    {
        timeSinceSpwan = 0.0f;
        if (enemySpwanPosition == null)
            enemySpwanPosition = new List<GameObject>();

    }


    void Update()
    {
        if (CanSpwanEnemy())
        {
            for (int i = 0; i < waveData.dataItems.Length; i++)
            {
                int canCreateCount = waveData.dataItems[i].CanCreateAtTime(timeSinceWaveStart);
                if (canCreateCount > 0)
                {
                    StartCoroutine(CreateEnemyCorutine(waveData.dataItems[i], canCreateCount));
                }
            }

            timeSinceWaveStart += Time.deltaTime;
        }
        timeSinceSpwan += Time.deltaTime;


        //Debug.Log(gameData.maxEnemyCount);
        //判断能否产生敌人
        //if (CanSpwanEnemy())
        //{
        //    Transform spwanTransform = GetSpwanPosition();
        //    GameObject spwanEnemy = GetSpwanEnemy();
        //    SpwanObjectAt(spwanEnemy, spwanTransform);
        //    timeSinceSpwan = 0;
        //}

    }

    #endregion


    IEnumerator CreateEnemyCorutine(WaveDataItem dataItem, int count)
    {
        needCreateEnemy += count;
        while (count > 0)
        {
            if (CanSpwanEnemy())
            {
                GameObject enemy = dataItem.GetCreateGameObject();
                Transform spwanTransform = GetSpwanPosition(enemy);
                if (SpwanObjectAt(enemy, spwanTransform))
                    count--;
            }
            yield return new WaitForSeconds(Random.Range(0, spwanInterval));
        }

    }

    /// <summary>
    /// 敌人死亡
    /// </summary>
    /// <param name="evt"></param>
    void OnEmenyDie(LTEvent evt)
    {
        AddAliveEmeny(-1);
    }

    /// <summary>
    /// 当前波数是否已完成
    /// </summary>
    /// <returns></returns>
    public bool IsWaveCompleted()
    {
        if (waveData == null || (waveData.dataItems == null || waveData.dataItems.Length == 0))
            return true;

        bool allWaveCleared = true;
        for (int i = 0; i < waveData.dataItems.Length; i++)
        {
            if (!waveData.dataItems[i].Cleared())
            {
                allWaveCleared = false;
                break;
            }
        }

        return aliveEnemyCount <= 0 && needCreateEnemy <= 0 && allWaveCleared;
    }


    //判断能否可以产生敌人
    bool CanSpwanEnemy()
    {

        if (!GameManager.Instance.IsInGame())
        {
            return false;
        }
        if (waveData == null)
        {
            return false;
        }

        if (IsWaveCompleted())
        {
            return false;
        }
        if (enemySpwanPosition.Count <= 0)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 增加当前敌人数
    /// </summary>
    /// <param name="count"></param>
    public void AddAliveEmeny(int count = 1)
    {
        aliveEnemyCount += count;
    }

    /// <summary>
    /// 获取产生的位置
    /// </summary>
    /// <returns></returns>
    Transform GetSpwanPosition(GameObject obj = null)
    {
        Transform rst = null;
        List<GameObject> lstCanSpwan = enemySpwanPosition.FindAll(p =>
        {
            var prop = p.GetComponent<PositionProperty>();
            if (prop == null)
            {
                return false;
            }
            else
            {
                if (prop.maxEnemyCount == -1 || p.transform.childCount < prop.maxEnemyCount)
                {
                    if (obj == null)
                        return true;
                    else
                    {
                        if (prop.allowedEmenys == null)
                            return false;
                        else
                        {
                            bool b = false;
                            for (int i = 0; i < prop.allowedEmenys.Length; i++)
                            {
                                if (prop.allowedEmenys[i] == obj)
                                {
                                    b = true;
                                    break;
                                }

                            }
                            return b;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
        });

        if (lstCanSpwan != null && lstCanSpwan.Count > 0)
        {
            rst = lstCanSpwan[Random.Range(0, lstCanSpwan.Count)].transform;
        }
        return rst;
    }

    /// <summary>
    /// 获取产生的敌人
    /// </summary>
    /// <returns></returns>
    GameObject GetSpwanEnemy()
    {
        // return enemys[Random.Range(0, enemys.Length)];
        return null;
    }

    bool SpwanObjectAt(GameObject obj, Transform parent)
    {
        if (parent == null)
            return false;
        GameObject swpanObj = null;

        var posProperty = parent.GetComponent<PositionProperty>();
        if (posProperty == null)
        {
            return false;
        }

        if (posProperty.positionType == EnemySpwanPosition.FixedPosition)
        {
            swpanObj = (GameObject)Instantiate(obj, parent.position, parent.rotation);
            swpanObj.transform.parent = parent.transform;
        }
        else if (posProperty.positionType == EnemySpwanPosition.RandomPosition)
        {
            Vector3 postion = new Vector3(posProperty.GetRandomX(), parent.position.y, parent.position.z);
            swpanObj = (GameObject)Instantiate(obj);
            swpanObj.transform.SetParent(parent);
            swpanObj.transform.position = postion;
        }
        if (swpanObj != null)
        {
            //避免重叠
            float timeFlag = Time.time / 100000;
            timeFlag = timeFlag - Mathf.FloorToInt(timeFlag);
            swpanObj.transform.position += new Vector3(0, 0, -timeFlag);
            //swpanObj.transform. = parent.transform;
            swpanObj.transform.localScale = parent.transform.lossyScale;

            //disable mask 
            if(!posProperty.enableMask)
            {
                var mask = swpanObj.transform.FindChild("mask");
                mask.gameObject.SetActive(false);
            }
            EnemyEmerge ee = parent.GetComponent<EnemyEmerge>();
            if (ee != null)
            {
                EnemyEmerge enew = swpanObj.AddComponent<EnemyEmerge>();
                enew.CopyFrom(ee);
                enew.RunEmerge();
            }

            //敌人的属性
            GAFEnemy e = swpanObj.GetComponent<GAFEnemy>();
            if (e == null)
            {
                Debug.Log("Dont have Enemy component!");
                e = swpanObj.AddComponent<GAFEnemy>();
            }

            e.EnhanceByTurn(1 + currentTurn  * enhancePerTurn + currentWave * enhancePerWave);
            SortLayer sl = parent.GetComponent<SortLayer>();
            if (sl != null && sl.layerName != "")
            {
                SpriteRenderer[] render = swpanObj.GetComponentsInChildren<SpriteRenderer>();
                if (render != null && render.Length > 0)
                {
                    for (int i = 0; i < render.Length; i++)
                    {
                        render[i].sortingLayerName = sl.layerName;
                    }
                }

                var sortlayer = swpanObj.AddComponent<GAFSortLayer>();
                sortlayer.sortLayerName = sl.layerName;
            }
            //是否能够横向漫游
            EnemyWanderX[] wanders = swpanObj.GetComponents<EnemyWanderX>();
            if (wanders != null && wanders.Length > 0)
            {
                for (int i = 0; i < wanders.Length; i++)
                {
                    if(wanders[i].GetType() == typeof(EnemyWanderRunX))
                    {
                        wanders[i].enabled = posProperty.allowRunX;
                    }
                    else
                    {
                        wanders[i].enabled = posProperty.allowWanderX;
                    }
                    wanders[i].minMovementX = posProperty.minMovementX;
                    wanders[i].maxMovementX = posProperty.maxMovementX;
                }
            }
            
            //通知GameManager ，产生了敌人
            AddAliveEmeny();
            needCreateEnemy--;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 重置
    /// </summary>
    void Reset()
    {
        timeSinceWaveStart = 0f;
        aliveEnemyCount = 0;
        needCreateEnemy = 0;

        if (waveData != null)
        {
            waveData.Initialize();
        }
    }

    /// <summary>
    /// 设置波数
    /// </summary>
    /// <param name="data"></param>
    /// <param name="turn"></param>
    public void SetWave(WaveData data, int turn,int wave = 1)
    {
        waveData = data;
        currentWave = wave;
        currentTurn = turn;
        Reset();
    }


}
