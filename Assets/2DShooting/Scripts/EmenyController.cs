using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GAFInternal.Objects;
using GAF.Core;

public class EmenyController : MonoBehaviour {

	//敌人对象
	public GameObject[] enemys;
	//敌人产生的位置
	public List<GameObject> enemySpwanPosition;

    //敌人产生的间隔
    public float spwanInterval = 5f;
    //同时存在的敌人的最大量
    public int maxEnemyCount = 1;

    public int maxEnemyPerPosition = 1;

    public GameData gameData { get; set; }

    //已经产生了的位置
    List<Transform> spwanedPosition;

	float timeSinceSpwan = 0.0f;
	
	void Start(){
		timeSinceSpwan = 0.0f;
		if (enemySpwanPosition == null)
			enemySpwanPosition = new List<GameObject> ();
		spwanedPosition = new List<Transform> ();
	}

	void Update(){
		timeSinceSpwan += Time.deltaTime;
        //Debug.Log(gameData.maxEnemyCount);
		//判断能否产生敌人
		if (CanSpwanEnemy ()) {
			Transform spwanTransform = getSpwanPosition();
			GameObject spwanEnemy = enemys[Random.Range(0,enemys.Length)];
			SpwanObjectAt(spwanEnemy,spwanTransform);
			timeSinceSpwan = 0;
		}

	}

	//判断能否可以产生敌人
	bool CanSpwanEnemy(){
        bool rst = true;

        if(!GameManager.Instance.IsInGame())
        {
            return false;
        }

        if(gameData == null)
        {
            return false;
        }
		if (timeSinceSpwan < gameData.emenySpwanInterval) {
			rst = false;
            return rst;
		}

		int enemyCount = 0;

		for (int i = 0; i<enemySpwanPosition.Count; i++) {
			enemyCount += enemySpwanPosition[i].transform.childCount;
		}

		if (gameData.maxEnemyCount <= enemyCount) {
			return false;
		}
		if (enemySpwanPosition.Count <= 0) {
					return false;
		}
        if (enemys.Length <= 0)
            return false;
        //if(maxEnemyCount <= spwanedPosition.)
        return true;
	}

	//获取产生敌人的对象
	Transform getSpwanPosition(){
		Transform rst = null;
		List<GameObject> lstCanSpwan = enemySpwanPosition.FindAll (p => {
			return p.transform.childCount < gameData.maxEnemyPerPosition; 
		});

		if (lstCanSpwan != null && lstCanSpwan.Count > 0) {
			rst = lstCanSpwan[Random.Range(0,lstCanSpwan.Count)].transform;
		}
		return rst;
	}

	void SpwanObjectAt(GameObject obj , Transform parent){
		GameObject swpanObj = (GameObject)Instantiate (obj, parent.position, parent.rotation);
       
        if (swpanObj != null) {
            //swpanObj.transform. = parent.transform;
            swpanObj.transform.localScale = parent.transform.localScale;
            swpanObj.transform.parent = parent.transform;
            

            var path = parent.GetComponent<iTweenPath>();
            if (path != null)
            {
               // iTween.EaseType.
                iTween.MoveTo(swpanObj, iTween.Hash("path", path.nodes.ToArray(), "easetype", "linear", "time", 1.6f));
            }
            else
            {
                EnemyEmerge ee = parent.GetComponent<EnemyEmerge>();
                if (ee != null)
                {
                    EnemyEmerge enew = swpanObj.AddComponent<EnemyEmerge>();
                    enew.CopyFrom(ee);
                    enew.RunEmerge();
                }
            }

            //敌人的属性
            GAFEnemy e = swpanObj.GetComponent<GAFEnemy>();
            if(e == null)
            {
                Debug.Log("Dont have Enemy component!");
                e = swpanObj.AddComponent<GAFEnemy>();
            }

            e.shootInterval = gameData.emenyShootInterval;
            e.attack = gameData.emenyAttack;
            if (gameData.useRondomAttack)
            {
                e.attack += Random.Range(-gameData.emenyAttackRandomVal, gameData.emenyAttackRandomVal);
            }
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

            //通知GameManager ，产生了敌人
            GameManager.Instance.SpawnedEnemy();
        } 

	}
}
