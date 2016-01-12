using UnityEngine;
using System.Collections;

public class CameraActions : MonoBehaviour
{

    public Vector3 defaultShakeAmount = new Vector3(0.05f, 0.05f, 0.05f);

    void Awake()
    {
        LeanTween.addListener((int)Events.SHAKECAMERA, OnCameraShake);
    }

    public void OnDisable()
    {
        LeanTween.removeListener((int)Events.SHAKECAMERA, OnCameraShake);
    }

    //void

    void OnCameraShake(LTEvent evt)
    {
        Vector3 amount = defaultShakeAmount;
        if(evt.data != null)
        {
            amount = (Vector3)evt.data;
        }
        iTween.ShakePosition(gameObject, amount, 0.5f);
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
