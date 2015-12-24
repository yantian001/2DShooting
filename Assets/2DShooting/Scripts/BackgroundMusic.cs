using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour {

    static BackgroundMusic _instance = null;
	// Use this for initialization
	void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
	
}
