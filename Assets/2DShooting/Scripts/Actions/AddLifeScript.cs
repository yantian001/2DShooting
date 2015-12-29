using UnityEngine;
using System.Collections;

public class AddLifeScript : MonoBehaviour {

    private ParticleSystem particle;
	// Use this for initialization
	void Start () {
        particle = GetComponent<ParticleSystem>();
        if(particle != null)
        {
            LeanTween.addListener((int)Events.ITEMMEDKITHIT, OnGetMedkit);
        }
    }
	
    void OnGetMedkit(LTEvent evt)
    {
        if(particle)
        {
            if(particle.isPlaying)
            {
                particle.Pause();
            }
            particle.Play();
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
