using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class TestTap : MonoBehaviour {

    // Use this for initialization
    Animator anim;
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(CrossPlatformInputManager.GetButtonDown("Shoot"))
        {
            //Debug.Log("fire");
            anim.SetBool("isShooting",true);
            
        }
        if(CrossPlatformInputManager.GetButtonUp("Shoot")) 
        {
            anim.SetBool("isShooting", false);
        }
	}
}
