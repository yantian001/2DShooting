using UnityEngine;
using System.Collections;

public class GAFObjectColider : MonoBehaviour {

    // Use this for initialization
    MeshRenderer mesh;
    Collider2D[] colliders;
	void Start () {
        mesh = GetComponent<MeshRenderer>();
        colliders = GetComponents<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        bool isColliderEnable = mesh != null && mesh.isVisible;
        if (colliders != null && colliders.Length > 0)
        {
            for(int i= 0; i<colliders.Length;i++)
            {
                colliders[i].enabled = isColliderEnable;
            }
        }

	}
}
