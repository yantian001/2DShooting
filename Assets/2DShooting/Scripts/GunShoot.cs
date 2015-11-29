using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
public class GunShoot : MonoBehaviour {

	public Camera backCamera;
	public Camera curCamera;
	public Transform signTransform;
	public Transform fireTreansform;
	bool isShooting = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (CrossPlatformInputManager.GetButtonDown ("Shoot")) {
			if(!isShooting)
			{
				isShooting = true;
				if(backCamera == null || curCamera ==null)
					return;

				Debug.Log(signTransform.position);
				Vector3 screenPoint = curCamera.WorldToScreenPoint(signTransform.position);

				RaycastHit2D raycastHit = Physics2D.Raycast(backCamera.ScreenToWorldPoint(screenPoint),Vector2.zero);
				if(raycastHit!=null && raycastHit.collider != null){
					Debug.Log( raycastHit.collider.name);
				}
			}
		}

		if (CrossPlatformInputManager.GetButtonUp ("Shoot")) {
			isShooting = false;
		}
	}
}
