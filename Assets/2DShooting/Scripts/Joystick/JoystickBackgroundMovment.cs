using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class JoystickBackgroundMovment : MonoBehaviour {

	public float smoothRatio = 0.08f;
    public float updateInterval = 0.1f;
    public float timeOnInterval = 5f;
	public bool useRestriction = true;
    //是否检测接近目标，如果开启，靠近目标时减速
    public bool checkNearTarget = true;
    //靠近目标时的ratio
    public float nearSmoothRatio = 1.2f;

    public float nearInteval = 0.06f;
	//资源宽度单位
	float textureWidthUnit;
	//资源高度单位
	float textureHightUnit;

	float minWidth;
	float MaxWidth;
	float minHight;
	float maxHight;
	// Use this for initialization
	void Start () {
		Sprite sprite = this.GetComponent<SpriteRenderer> ().sprite;
		float pixelPerUnit = sprite.pixelsPerUnit;
		textureWidthUnit = (float)sprite.texture.width / pixelPerUnit;
		textureHightUnit = (float)sprite.texture.height / pixelPerUnit;
		float aspectRatio = Camera.main.aspect;
        float cameraHight = 0f;
        float cameraWidth = 0f;
        if (Camera.main.orthographic)
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


		minWidth = transform.position.x - (textureWidthUnit / 2 ) + cameraWidth;
		MaxWidth = transform.position.x + (textureWidthUnit / 2 ) - cameraWidth;
		minHight = transform.position.y - (textureHightUnit / 2 ) + cameraHight;
		maxHight = transform.position.y + (textureHightUnit / 2 ) - cameraHight;
        StartCoroutine(checkMove());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
       // Move();
    }

    void Move()
    {
        float horazital = CrossPlatformInputManager.GetAxis("JoyStickX");
        float vertical = CrossPlatformInputManager.GetAxis("JoyStickY");
        CrossPlatformInputManager.SetAxisZero("JoyStickX");
        CrossPlatformInputManager.SetAxisZero("JoyStickY");
        float interval = timeOnInterval;
        if (checkNearTarget)
        {
            Transform signTran = GameObject.Find("Sign").transform;
            bool hasTarget = false;
           // Gizmos.DrawCube(tr)
            if (signTran != null)
            {
                RaycastHit2D[] raycasts = Physics2D.BoxCastAll(signTran.position, new Vector2(1, 1), 0f, Vector2.zero);
                if (raycasts != null && raycasts.Length > 0)
                {
                    for(int i = 0;i<raycasts.Length;i++)
                    {
                        if (raycasts[i].collider.gameObject.GetComponent<Enemy>() != null || raycasts[i].collider.gameObject.GetComponent<GameItem>() != null)
                        {
                            hasTarget = true;
                            break;
                        }
                    }
                }
            }
            if(hasTarget)
            {
                interval = nearInteval;
              //  Debug.Log("near target");
            }
            
        }
        //Physics2D.BoxCastAll()
       // Vector3.SmoothDamp()
        Vector3 newPos = transform.position - new Vector3(horazital, vertical, 0) * smoothRatio;
        if (useRestriction)
        {
            newPos = new Vector3(Mathf.Clamp(newPos.x, minWidth, MaxWidth), Mathf.Clamp(newPos.y, minHight, maxHight), newPos.z);
        }
        //transform.position = newPos;
        //transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * (smoothRatio > 100 ? smoothRatio /100 : 1));

        transform.position = Vector3.Lerp(transform.position, newPos,updateInterval / interval);
        //transform.position = newPos;
        //iTween.MoveTo(gameObject, newPos, updateInterval);
    }

    IEnumerator checkMove()
    {
        while(true)
        {
            
            yield return new WaitForSeconds(updateInterval);
            Move();
        }
    }
}
