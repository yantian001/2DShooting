using UnityEngine;
using System.Collections;

public class Tween  {

	// Use this for initialization
	static void ShakeUIScale(RectTransform rect ,float shakeTime ,float time)
    {
        Vector3 lclScale = rect.localScale;
        LeanTween.scale(rect, lclScale * shakeTime, time).setOnComplete(() => {
            LeanTween.scale(rect, lclScale, time);
        });
    }
}
