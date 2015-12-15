using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class AutoFadeAndDestory : MonoBehaviour {

    public float delay = 1.0f;
    public float fadeTo = 0f;
    public float fadeTime = 1f;
    public bool affectChild = true;
	// Use this for initialization
	void Start () {
        RectTransform rect = GetComponent<RectTransform>();
        LeanTween.alpha(rect, fadeTo, fadeTime).setDelay(delay).setOnComplete(
            ()=>
            {
                Destroy(gameObject);
            }
            );
        if (affectChild)
        {
            RectTransform[] childRects = rect.GetComponentsInChildren<RectTransform>();
            if(childRects!=null && childRects.Length > 0)
            {
                for(int i = 0;i<childRects.Length;i++)
                {
                    LeanTween.alpha(childRects[i], fadeTo, fadeTime);
                }
            } 
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
