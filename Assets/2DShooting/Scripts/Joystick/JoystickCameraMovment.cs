using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Camera))]
public class JoystickCameraMovment : MonoBehaviour {

    // Use this for initialization
    public bool restrictionMoveZone = true;
    public float smoothRatio = 10f;
    public bool checkNearTarget = true;
    public float nearSmoothRatio = 4f;
    public float textureWidthUnit = 2250;
    //资源高度单位
    public float textureHightUnit = 1600;
    public float pixelPerUnit = 100;
    float minWidth;
    float MaxWidth;
    float minHight;
    float maxHight;
    public Transform signTran;
    Camera came;
   

    void Start () {
        came = this.GetComponent<Camera>();
        CalculteSize();
	}

    void CalculteSize()
    {
        //宽高比
        float aspectRatio = came.aspect;

        //float resWidth =0, resHight = 0;
        Sprite sprite = null;

        if (sprite != null)
        {
            pixelPerUnit = sprite.pixelsPerUnit;
            textureWidthUnit = (float)sprite.texture.width / pixelPerUnit;
            textureHightUnit = (float)sprite.texture.height / pixelPerUnit;
        }
        else
        {
            textureWidthUnit = textureWidthUnit / pixelPerUnit;
            textureHightUnit = textureHightUnit / pixelPerUnit;
        }
        aspectRatio =came.aspect;
        float cameraHight = 0f;
        float cameraWidth = 0f;
        if (came.orthographic)
        {
            cameraHight = Camera.main.orthographicSize;
            cameraWidth = aspectRatio * cameraHight;
        }
        else
        {
            float digree = Camera.main.fieldOfView;
            float distance = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
            cameraHight = distance / Mathf.Tan(digree * Mathf.Deg2Rad);
            cameraWidth = aspectRatio * cameraHight;
        }


        minWidth = transform.position.x - (textureWidthUnit / 2) + cameraWidth;
        MaxWidth = transform.position.x + (textureWidthUnit / 2) - cameraWidth;
        minHight = transform.position.y - (textureHightUnit / 2) + cameraHight;
        maxHight = transform.position.y + (textureHightUnit / 2) - cameraHight;

        if (signTran == null)
        {
            signTran = GameObject.Find("Sign").transform;
        }
    }
	
	// Update is called once per frame
	void Update () {

        float horazital = CrossPlatformInputManager.GetAxis("JoyStickX");
        float vertical = CrossPlatformInputManager.GetAxis("JoyStickY");
        //CrossPlatformInputManager.SetAxisZero("JoyStickX");
        //CrossPlatformInputManager.SetAxisZero("JoyStickY");
        //Debug.Log(horazital);
        float smooth = smoothRatio;
        if (checkNearTarget && signTran != null)
        {
            //Transform signTran = GameObject.Find("Sign").transform;
            bool hasTarget = false;
            // Gizmos.DrawCube(tr)
            if (signTran != null)
            {
                RaycastHit2D[] raycasts = Physics2D.BoxCastAll(signTran.position, new Vector2(0.1f, 0.1f), 0f, Vector2.zero);
                if (raycasts != null && raycasts.Length > 0)
                {
                    for (int i = 0; i < raycasts.Length; i++)
                    {
                        if (raycasts[i].collider.gameObject.GetComponent<GameItem>() != null || raycasts[i].collider.gameObject.GetComponent<GAFEnemy>() != null || raycasts[i].collider.GetComponentInParent<GAFEnemy>() != null)
                        {
                            hasTarget = true;
                            break;
                        }
                    }
                }
            }
            if (hasTarget)
            {
                smooth = nearSmoothRatio;
                //Debug.Log("near target");
            }

        }
        Vector3 newPos = transform.position + new Vector3(horazital, vertical, 0);
        //transform.rotation = new Quaternion(transform.rotation.x+ horazital * Mathf.Deg2Rad * smoothRatio, transform.rotation.y + vertical * Mathf.Deg2Rad * smoothRatio, transform.rotation.z,transform.rotation.w);
        if (restrictionMoveZone)
        {
            newPos = new Vector3(Mathf.Clamp(newPos.x, minWidth, MaxWidth), Mathf.Clamp(newPos.y, minHight, maxHight), newPos.z);
        }

        transform.position = Vector3.Lerp(transform.position,newPos,Time.deltaTime * smooth);
    }
}
