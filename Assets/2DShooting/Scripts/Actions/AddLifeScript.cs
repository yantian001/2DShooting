﻿using UnityEngine;
using System.Collections;

public class AddLifeScript : MonoBehaviour
{

    public GameObject lifeParticle;

    // Use this for initialization
    //void Start()
    //{
    //    particle = GetComponent<ParticleSystem>();
    //    if (particle != null)
    //    {
    //        LeanTween.addListener((int)Events.ITEMMEDKITHIT, OnGetMedkit);
    //    }
    //}

    void Awake()
    {
        LeanTween.addListener((int)Events.ITEMMEDKITHIT, OnGetMedkit);
    }

    void OnGetMedkit(LTEvent evt)
    {
        //if(particle)
        //{
        //    if(particle.isPlaying)
        //    {
        //        particle.Pause();
        //    }
        //    particle.Play();
        //}
        var obj = (GameObject)Instantiate(lifeParticle, gameObject.transform.position, gameObject.transform.rotation);
        if (obj)
        {
            obj.transform.SetParent(gameObject.transform);
            obj.GetComponent<ParticleSystem>().Play(true);
           // Destroy(obj, 2f);
            // Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDisable()
    {
        LeanTween.removeListener((int)Events.ITEMMEDKITHIT, OnGetMedkit);
    }
}
