using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour {

    public Vector3 shakeAmount = new Vector3(0.1f, 0.1f, 0.1f);

	// Use this for initialization
	void Start () {
        iTween.ShakePosition(gameObject,iTween.Hash("amount",shakeAmount,"time",1f, "islocal",true, "looptype",iTween.LoopType.loop));
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
