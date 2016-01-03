using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GAFInternal.Objects;
using GAF.Core;

public class EmenyController : MonoBehaviour
{

    //敌人对象
    public GameObject[] enemys;
    //敌人产生的位置
    public List<GameObject> enemySpwanPosition;

    /// <summary>
    /// 敌人x轴上可移动的最大范围
    /// </summary>
    public  float maxMovemontX = 5.3f;
    /// <summary>
    /// 敌人x轴上可移动的最小范围
    /// </summary>
    public  float minMovementX = -5.3f;
    /// <summary>
    /// 敌人产生位置类型
    /// </summary>
    [Tooltip("敌人产生位置类型")]
    public EnemySpwanPosition spwanPostionType;
    //敌人产生的间隔
    public float spwanInterval = 5f;
    //同时存在的敌人的最大量
    public int maxEnemyCount = 1;

    public int maxEnemyPerPosition = 1;

    public GameData gameData { get; set; }

    //已经产生了的位置
    List<Transform> spwanedPosition;

    float timeSinceSpwan = 0.0f;

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

    public void Awake()
    {
        _instance = this;
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
        spwanedPosition = new List<Transform>();
    }

    void Update()
    {
        timeSinceSpwan += Time.deltaTime;
        //Debug.Log(gameData.maxEnemyCount);
        //判断能否产生敌人
        if (CanSpwanEnemy())
        {
            Transform spwanTransform = GetSpwanPosition();
            GameObject spwanEnemy = GetSpwanEnemy();
            SpwanObjectAt(spwanEnemy, spwanTransform);
            timeSinceSpwan = 0;
        }

    }

    //判断能否可以产生敌人
    bool CanSpwanEnemy()
    {
        bool rst = true;

        if (!GameManager.Instance.IsInGame())
        {
            return false;
        }

        if (gameData == null)
        {
            return false;
        }
        if (timeSinceSpwan < gameData.emenySpwanInterval)
        {
            rst = false;
            return rst;
        }

        int enemyCount = 0;

        for (int i = 0; i < enemySpwanPosition.Count; i++)
        {
            enemyCount += enemySpwanPosition[i].transform.childCount;
        }

        if (gameData.maxEnemyCount <= enemyCount)
        {
            return false;
        }
        if (enemySpwanPosition.Count <= 0)
        {
            return false;
        }
        if (enemys.Length <= 0)
            return false;
        //if(maxEnemyCount <= spwanedPosition.)
        return true;
    }

    /// <summary>
    /// 获取产生的位置
    /// </summary>
    /// <returns></returns>
    Transform GetSpwanPosition()
    {
        Transform rst = null;
        List<GameObject> lstCanSpwan = enemySpwanPosition.FindAll(p =>
        {
            return p.transform.childCount < gameData.maxEnemyPerPosition;
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
       return enemys[Random.Range(0, enemys.Length)];
    }

    void SpwanObjectAt(GameObject obj, Transform parent)
    {
        GameObject swpanObj = null;

        var posProperty = parent.GetComponent<PositionProperty>();
        if(posProperty == null)
        {
            return;
        }

        if(posProperty.positionType == EnemySpwanPosition.FixedPosition)
        {
            swpanObj = (GameObject)Instantiate(obj, parent.position, parent.rotation);
            swpanObj.transform.parent = parent.transform;
        }
        else if(posProperty.positionType == EnemySpwanPosition.RandomPosition)
        {
            Vector3 postion = new Vector3(posProperty.GetRandomX(), parent.position.y, parent.position.z);
            swpanObj = (GameObject)Instantiate(obj);
            swpanObj.transform.SetParent(parent.parent);
            swpanObj.transform.localPosition = postion;
        }
        if (swpanObj != null)
        {
            //swpanObj.transform. = parent.transform;
            swpanObj.transform.localScale = parent.transform.localScale;
            

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

            e.shootInterval = gameData.emenyShootInterval;
            //e.attack = gameData.emenyAttack;
            //if (gameData.useRondomAttack)
            //{
            //    e.attack += Random.Range(-gameData.emenyAttackRandomVal, gameData.emenyAttackRandomVal);
            //}
            e._HP = gameData.emenyHP;
            if (gameData.useRandomHP)
            {
                e._HP += Random.Range(-gameData.emenyHPRandomVal, gameData.emenyHPRandomVal);
            }


            //更改显示SortingLayer
            SortLayer sl = parent.GetComponent<SortLayer>();
            if (sl != null && sl.layerName != "")
            {
                SpriteRenderer render = swpanObj.GetComponent<SpriteRenderer>();
                var sortlayer = swpanObj.AddComponent<GAFSortLayer>();
                sortlayer.sortLayerName = sl.layerName;
                //gafAnimator.settings.spriteLayerName = sl.layerName;
                if (render != null)
                {
                    render.sortingLayerName = sl.layerName;
                }
            }
            //是否能够横向漫游
            EnemyWanderX[] wanders = swpanObj.GetComponents<EnemyWanderX>();
            if(wanders!=null && wanders.Length > 0)
            {
                for(int i =0;i<wanders.Length;i++)
                {
                    wanders[i].enabled = posProperty.allowWanderX;
                    
                    wanders[i].minMovementX = posProperty.minMovementX;
                    wanders[i].maxMovementX = posProperty.maxMovementX;
                }
            }



            //通知GameManager ，产生了敌人
            GameManager.Instance.SpawnedEnemy();
        }

    }


    public float ClampMoveX(float x)
    {
        return Mathf.Clamp(x, minMovementX, maxMovemontX);
    }
}
