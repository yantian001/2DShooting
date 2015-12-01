using UnityEngine;
using System.Collections;

public class DestroySelf : MonoBehaviour {


    public float time = 0.5f;
    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, time);
    }

}
