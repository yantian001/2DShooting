using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class JoystickBackgroundMovment : MonoBehaviour {

	public float smoothRatio = 0.08f;
	public bool useRestriction = true;
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
		float cameraHight = Camera.main.orthographicSize;
		float cameraWidth = aspectRatio * cameraHight;

//		minWidth = transform.position.x - (textureWidthUnit / 2 - cameraWidth / 4);
//		MaxWidth = transform.position.x + (textureWidthUnit / 2 - cameraWidth / 4);
//		minHight = transform.position.y - (textureHightUnit / 2 - cameraHight / 4);
//		maxHight = transform.position.y + (textureHightUnit / 2 - cameraHight / 4);
		minWidth = transform.position.x - (textureWidthUnit / 2 ) + cameraWidth;
		MaxWidth = transform.position.x + (textureWidthUnit / 2 ) - cameraWidth;
		minHight = transform.position.y - (textureHightUnit / 2 ) + cameraHight;
		maxHight = transform.position.y + (textureHightUnit / 2 ) - cameraHight;
		//Vector3 pos = transform.position;

//		Debug.Log (GetComponent<SpriteRenderer> ().sprite.pixelsPerUnit);
//		Debug.Log (GetComponent<SpriteRenderer> ().sprite.texture.width);
	}
	
	// Update is called once per frame
	void Update () {
		float horazital = CrossPlatformInputManager.GetAxis("JoyStickX");
		float vertical = CrossPlatformInputManager.GetAxis("JoyStickY");
		
		Vector3 newPos = transform.position - new Vector3(horazital, vertical, 0) * smoothRatio;
		if(useRestriction)
		{
			newPos = new Vector3(Mathf.Clamp(newPos.x, minWidth, MaxWidth), Mathf.Clamp(newPos.y, minHight, maxHight), newPos.z);
		}
		transform.position = newPos;
	}
}
