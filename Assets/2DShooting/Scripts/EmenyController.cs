using UnityEngine;
using System.Collections;
using System.Collections.Generic;


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
		if (timeSinceSpwan < spwanInterval) {
			rst = false;
		}

		int enemyCount = 0;

		for (int i = 0; i<enemySpwanPosition.Count; i++) {
			enemyCount += enemySpwanPosition[i].transform.childCount;
		}

		if (maxEnemyCount <= enemyCount) {
			rst = false;
		}
		if (enemySpwanPosition.Count <= 0) {
					rst = false;
		}
		if (enemys.Length <= 0)
			rst = false;
		//if(maxEnemyCount <= spwanedPosition.)
		return rst;
	}

	//获取产生敌人的对象
	Transform getSpwanPosition(){
		Transform rst = null;
		List<GameObject> lstCanSpwan = enemySpwanPosition.FindAll (p => {
			return p.transform.childCount < maxEnemyPerPosition; 
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
            EnemyEmerge ee = parent.GetComponent<EnemyEmerge>();
            if (ee != null)
            {
                EnemyEmerge enew = swpanObj.AddComponent<EnemyEmerge>();
                enew.CopyFrom(ee);
                enew.RunEmerge();
            }
        }
        
        //swpanObj.AddComponent(typeof)
         
	}
}
