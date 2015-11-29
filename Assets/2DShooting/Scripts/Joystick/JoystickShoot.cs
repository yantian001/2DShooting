using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class JoystickShoot : MonoBehaviour {


	public Weapon weapon;

	bool isPressed = false;
	bool isCombo = false;
	// Use this for initialization
	void Start () {
		if (weapon == null) {
			weapon = GetComponent<Weapon>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (weapon == null)
			return;
		if (CrossPlatformInputManager.GetButtonDown ("Fire")) {
			Debug.Log(3);
			//weapon.Fire();
			//weapon.Fire(isPressed);
			isPressed = true;
		}

		if (CrossPlatformInputManager.GetButtonUp ("Fire")) {
			Debug.Log(2);
			isPressed = false;
			isCombo =false;
		}

		if (isPressed) {
			weapon.Fire(isCombo);
			isCombo = true;
		}
	}
}
