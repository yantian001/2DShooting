using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Events;

public class TestTap : MonoBehaviour {

    // Use this for initialization
    public Transform gameObj;
    public float ratio = 0.1f;
    Animator anim;
	void Start () {
        anim = GetComponent<Animator>();
        
	}
	
	// Update is called once per frame
	void Update () {
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        gameObj.transform.position = gameObj.transform.position + new Vector3(h, v, 0) * ratio;
        CrossPlatformInputManager.SetAxisZero("Horizontal");
        CrossPlatformInputManager.SetAxisZero("Vertical");
        // Debug.Log("x :" + h + "y :" + v);
    }
}
