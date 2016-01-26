using UnityEngine;
using System.Collections;

public class SpriteFade : MonoBehaviour {

    public float to = 0f;

    public float time = 1f;

    public float delay = 0;

    public bool destoryAfterFade = true;
	// Use this for initialization
	void Start () {
        LeanTween.alpha(gameObject, to, time).setDelay(delay).setDestroyOnComplete(destoryAfterFade);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
