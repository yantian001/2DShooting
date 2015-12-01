using UnityEngine;
using System.Collections;

public class Scale : MonoBehaviour {

    // Use this for initialization
    public float time = 0.1f;
	void Start () {
        Vector3 scale = gameObject.transform.lossyScale;
        gameObject.transform.localScale = Vector3.zero;
        iTween.ScaleTo(gameObject, scale, time);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
