using UnityEngine;
using System.Collections;

public class GAFObjectColiderRoot : MonoBehaviour {

    Animator animator;
    public string specialClipName = "";

    Collider2D[] myColliders;
	// Use this for initialization
	void Start () {
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        if(colliders != null && colliders.Length > 0)
        {
            for(int i =0; i<colliders.Length;i++)
            {
                if(colliders[i].GetComponent<GAFObjectColider>()==null && colliders[i].gameObject != gameObject)
                {
                    colliders[i].gameObject.AddComponent<GAFObjectColider>();
                }
            }
        }
        animator = GetComponent<Animator>();
        myColliders = GetComponents<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if(animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName(specialClipName))
        {
            SetMyCollierStatus(true);
        }
        else
        {
            SetMyCollierStatus(false);
        }
            //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("gun"));

	}

    void SetMyCollierStatus(bool isEnable)
    {
        if (myColliders != null && myColliders.Length > 0)
        {
            for (int i = 0; i < myColliders.Length; i++)
            {
                myColliders[i].enabled = isEnable;
            }
        }
    }
}
