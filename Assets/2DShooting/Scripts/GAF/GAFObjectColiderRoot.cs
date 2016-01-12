using UnityEngine;
using System.Collections;

public class GAFObjectColiderRoot : MonoBehaviour {

   public Animator animator;
    public string specialClipName = "";

    public Collider2D[] myColliders;

    public Collider2D[] colliders;

    public bool isBaked = false;
	// Use this for initialization
	void Start () {
      
        
        if (!isBaked)
        {
            if(colliders == null)
            {
                colliders = GetComponentsInChildren<Collider2D>();
            }
            myColliders = GetComponents<Collider2D>();
            if (colliders != null && colliders.Length > 0)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].GetComponent<GAFObjectColider>() == null && colliders[i].gameObject != gameObject)
                    {
                        colliders[i].gameObject.AddComponent<GAFObjectColider>();
                    }
                }
            }
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }
        else
        {
            if(animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName(specialClipName));
        if (animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName(specialClipName))
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

        if(isBaked)
        {
            if(colliders != null)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    colliders[i].enabled = !isEnable;
                }

            }
        }
    }
}
