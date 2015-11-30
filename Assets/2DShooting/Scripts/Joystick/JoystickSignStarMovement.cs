using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class JoystickSignStarMovement : MonoBehaviour {

	public float smoothRatio = 0.1f;
	//是否同等缩放移动
	public bool useRatio = false;
	//可移动区域的宽度
	public float movableWidth = 1.0f;
	//可移动区域的高度
	public float movableHight = 1.0f;

	float widthRatio,hightRatio;
	float maxMoveableWidth , minMoveableWidth ,maxMoveableHight ,minMoveableHight;
	// Use this for initialization
	void Start () {

		float aspectRatio = Camera.main.aspect;
		float orthGrapicSize = Camera.main.orthographicSize;
		float widthSize = aspectRatio * orthGrapicSize;
		if (useRatio) {
			minMoveableWidth = transform.position.x - movableWidth / 2;
			maxMoveableWidth = transform.position.x + movableWidth / 2;
			minMoveableHight = transform.position.y - movableHight / 2;
			maxMoveableHight = transform.position.y + movableHight / 2;
		} else {
			minMoveableWidth = transform.position.x - widthSize;
			maxMoveableWidth = transform.position.x + widthSize;
			minMoveableHight = transform.position.y - orthGrapicSize;
			maxMoveableHight = transform.position.y + orthGrapicSize;
		}
		widthRatio = movableWidth / widthSize;
		hightRatio = movableHight / orthGrapicSize;
	}
	
	// Update is called once per frame
	void Update () {
		float horizatal = CrossPlatformInputManager.GetAxis ("JoyStickX");
		float vertival = CrossPlatformInputManager.GetAxis ("JoyStickY");
	
		Vector3 moveBy = new Vector3 (horizatal, vertival, 0) * smoothRatio;
		if (useRatio) {
			moveBy = new Vector3(moveBy.x * widthRatio,moveBy.y * hightRatio,moveBy.z);
		}

		Vector3 newPosition = new Vector3 (Mathf.Clamp (transform.position.x + moveBy.x, minMoveableWidth, maxMoveableWidth),
		                                  Mathf.Clamp (transform.position.y + moveBy.y, minMoveableHight, maxMoveableHight),
		                                  transform.position.z + moveBy.z);

		transform.position = newPosition;
	}


}
