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
    public GameObject Backgroud;

    Camera came;
   

    void Start () {
        came = this.GetComponent<Camera>();
        CalculteSize();
	}

    void CalculteSize()
    {
        //宽高比
        float aspectRatio = came.aspect;

        float resWidth =0, resHight = 0;
        Sprite sprite = Backgroud.GetComponent<SpriteRenderer>().sprite;
        if(sprite != null)
        {
            resWidth = (float)sprite.texture.width / sprite.pixelsPerUnit;
            resHight = (float)sprite.texture.height / sprite.pixelsPerUnit;
        }

        if(came.orthographic)
        {
            float orthGraphicSize = came.orthographicSize;
            float horizortal = aspectRatio * orthGraphicSize;

            minCameraMoveWidth = minWidth + horizortal;
            maxCameraMoveWidth = maxWidth - horizortal;
            maxCameraMoveHight = maxHight - orthGraphicSize;
            minCameraMoveHight = minHight + orthGraphicSize;
        }
        else
        {
            if (Backgroud == null)
            {
                Debug.LogError("need background !!");
                return;
            }
            Transform bgTransform = Backgroud.transform;
            float digree = came.fieldOfView;
            float distance = Mathf.Abs(came.transform.position.z - bgTransform.position.z);

            float inviewhalfSizeY = distance / Mathf.Tan(digree * Mathf.Deg2Rad);
            float inviewHalfSizeX = inviewhalfSizeY * aspectRatio;
            minCameraMoveWidth = -(resWidth / 2 - inviewHalfSizeX);
            maxCameraMoveWidth = resWidth/2 - inviewHalfSizeX ;
            minCameraMoveHight = -(resHight/2 - inviewhalfSizeY);
            maxCameraMoveWidth = resHight/2 - inviewhalfSizeY ;
        }
    }
	
	// Update is called once per frame
	void Update () {

        float horazital = CrossPlatformInputManager.GetAxis("JoyStickX");
        float vertical = CrossPlatformInputManager.GetAxis("JoyStickY");
        CrossPlatformInputManager.SetAxisZero("JoyStickX");
        CrossPlatformInputManager.SetAxisZero("JoyStickY");
        Vector3 newPos = transform.position - new Vector3(horazital, vertical, 0) * smoothRatio;
        //transform.rotation = new Quaternion(transform.rotation.x+ horazital * Mathf.Deg2Rad * smoothRatio, transform.rotation.y + vertical * Mathf.Deg2Rad * smoothRatio, transform.rotation.z,transform.rotation.w);
        if (restrictionMoveZone)
        {
            newPos = new Vector3(Mathf.Clamp(newPos.x, minCameraMoveWidth, maxCameraMoveWidth), Mathf.Clamp(newPos.y, minCameraMoveHight, maxCameraMoveHight), newPos.z);
        }
        transform.position = newPos;
    }
}
