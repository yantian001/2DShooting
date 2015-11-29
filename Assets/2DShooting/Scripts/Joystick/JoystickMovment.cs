using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class JoystickMovment : MonoBehaviour {

    //限制移动区域
    public float minWidth = 0;
    public float maxWidth = 0;
    public float minHeight = 0;
    public float maxHeight = 0;
    //屏幕尺寸
    float minScreenWidth;
    float maxScreenWidth;
    float minScreenHight;
    float maxScreenHight;

    public float smoothRate = 0.08f;

    Vector3 lastPosition;
    // Use this for initialization
    void Start () {
        lastPosition = transform.position;
        //自动计算边界 ，
        //如果需要设置边界值为0 ，设置为0.01.
        float aspect = Camera.main.aspect;
        float orthSize = Camera.main.orthographicSize;
        float dist = orthSize * aspect;
        Vector3 camerapos = Camera.main.transform.position;
        minScreenWidth = camerapos.x - dist;
        maxScreenWidth = camerapos.x + dist;
        minScreenHight = camerapos.y - orthSize;
        maxScreenHight = camerapos.y + orthSize;

        if (minWidth == 0)
        {
            minWidth = minScreenWidth;
        }
        if(maxWidth == 0)
        {
            maxWidth = maxScreenWidth;
        }
        if(minHeight == 0)
        {
            minHeight = minScreenHight;
        }
        if(maxHeight == 0)
        {
            maxHeight = maxScreenHight;
        }
	}
	
	// Update is called once per frame
	void Update () {
        float horizotal = CrossPlatformInputManager.GetAxis("JoyStickX") * smoothRate;
        float vertical = CrossPlatformInputManager.GetAxis("JoyStickY") * smoothRate;

        //Debug.Log("x :" + horizotal + ", Y:" + vertical);
        // Vector3 movePostion = new Vector3(horizotal, vertical, 0);
        lastPosition = new Vector3(Mathf.Clamp(lastPosition.x + horizotal, minScreenWidth, maxScreenWidth), Mathf.Clamp(lastPosition.y + vertical, minScreenHight, maxScreenHight), 0);
        
        Vector3 newPos = new Vector3(Mathf.Clamp(lastPosition.x, minWidth, maxWidth), Mathf.Clamp(lastPosition.y, minHeight, maxHeight), lastPosition.z);
        transform.position = newPos;
	}
}
