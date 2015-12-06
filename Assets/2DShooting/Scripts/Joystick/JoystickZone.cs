using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class JoystickZone : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public GameObject JoystickIcon;
    public void OnPointerDown(PointerEventData eventData)
    {
        JoystickIcon.SetActive(true);
        JoystickIcon.GetComponent<RectTransform>().position = eventData.position;
        JoystickIcon.GetComponent<JoystickPlane>().OnPointerDown(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //throw new NotImplementedException();
        JoystickIcon.SetActive(false);
        
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
