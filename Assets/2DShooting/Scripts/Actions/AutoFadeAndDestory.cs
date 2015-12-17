using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class AutoFadeAndDestory : MonoBehaviour {

    public bool shakeFirst = false;
    public float delay = 1.0f;
    public float fadeTo = 0f;
    public float fadeTime = 1f;
    public bool affectChild = true;
	// Use this for initialization
	void Start () {
        if(shakeFirst)
        {
            
            //iTween.ShakeScale(gameObject, iTween.Hash( "amount" ,new Vector3(0, 0, 30), "time",0.2f, "oncomplete", "OnShakeComplete", "oncompletetarget", gameObject));
            iTween.ShakeScale(gameObject, iTween.Hash( "amount" ,new Vector3(1.1f, 1.1f, 1), "time",0.2f, "oncomplete", "OnShakeComplete", "oncompletetarget", gameObject));
        }
        else
        {
            FadeOut();
        }
    }
	
    void OnShakeComplete()
    {
        FadeOut();
    }

    void FadeOut()
    {
        RectTransform rect = GetComponent<RectTransform>();
        LeanTween.alpha(rect, fadeTo, fadeTime).setDelay(delay).setOnComplete(
            () =>
            {
                Destroy(gameObject);
            }
            );
        if (affectChild)
        {
            RectTransform[] childRects = rect.GetComponentsInChildren<RectTransform>();
            if (childRects != null && childRects.Length > 0)
            {
                for (int i = 0; i < childRects.Length; i++)
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
