using UnityEngine;
using System.Collections;

public class GAFSortLayer : MonoBehaviour {

    // Use this for initialization
    MeshRenderer[] renders;
    public string sortLayerName = "";
	void Start () {
        renders = GetComponentsInChildren<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(renders != null && renders.Length > 0)
        {
            for(int i= 0;i<renders.Length;i++)
            {
                renders[i].sortingLayerName = sortLayerName;
            }
        }
	}
}
