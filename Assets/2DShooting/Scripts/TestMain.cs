using UnityEngine;
using System.Collections;

public class TestMain : MonoBehaviour {

    // Use this for initialization
    public Transform fireTransform;
	void Start () {
	    if(fireTransform == null)
        {
            fireTransform = GameObject.Find("fire").transform;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint( Input.mousePosition);
            
            float angle = Vector3.Angle(fireTransform.position, mousePos);
            Debug.Log("angle :" + angle.ToString());
            Debug.Log("euler angler:" + fireTransform.eulerAngles.ToString());
            Debug.DrawLine(fireTransform.position, mousePos, Color.blue, 1, true);
          //  Debug.DrawRay(fireTransform.position, mousePos, Color.blue, 1, false);
        }
	}
}
