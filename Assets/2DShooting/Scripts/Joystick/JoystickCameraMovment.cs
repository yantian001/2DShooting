using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Camera))]
public class JoystickCameraMovment : MonoBehaviour {

    // Use this for initialization
    public bool restrictionMoveZone = true;
    public float smoothRatio = 0.08f;
    public float minWidth = 0.0f;
    public float maxWidth = 0.0f;
    public float minHight = 0.0f;
    public float maxHight = 0.0f;
    //the Camera move zone
    float minCameraMoveWidth = 0.0f;
    float maxCameraMoveWidth = 0.0f;
    float minCameraMoveHight = 0.0f;
    float maxCameraMoveHight = 0.0f;

    Camera came;
   

    void Start () {
        came = this.GetComponent<Camera>();
        float aspectRatio = came.aspect;
        float orthGraphicSize = came.orthographicSize;
        float horizortal = aspectRatio * orthGraphicSize;

        minCameraMoveWidth = minWidth + horizortal;
        maxCameraMoveWidth = maxWidth - horizortal;
        maxCameraMoveHight = maxHight - orthGraphicSize;
        minCameraMoveHight = minHight + orthGraphicSize;
	}
	
	// Update is called once per frame
	void Update () {

        float horazital = CrossPlatformInputManager.GetAxis("JoyStickX");
        float vertical = CrossPlatformInputManager.GetAxis("JoyStickY");

        Vector3 newPos = transform.position + new Vector3(horazital, vertical, 0) * smoothRatio;
        if(restrictionMoveZone)
        {
            newPos = new Vector3(Mathf.Clamp(newPos.x, minCameraMoveWidth, maxCameraMoveWidth), Mathf.Clamp(newPos.y, minCameraMoveHight, maxCameraMoveHight), newPos.z);
        }
        transform.position = newPos;
	}
}
