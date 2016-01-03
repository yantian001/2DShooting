using UnityEngine;
using System.Collections;

public class AutoScaleByZOrder : MonoBehaviour {

    // Use this for initialization
    public bool onlyEffectChild = true;
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(onlyEffectChild)
        {
            
        }
        float scale = (transform.position.z )/ 10;
        transform.localScale = new Vector3(scale,scale,scale);
        Debug.Log(transform.localScale);
        
	}
}
