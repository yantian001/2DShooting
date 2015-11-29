using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class GameCamera : MonoBehaviour {

    // Use this for initialization
    public float devHeight = 640f;
    public float devWidth = 960f;

    public float fixelsPerUnit = 100f;
	void Start () {
        devHeight = devHeight / fixelsPerUnit;
        devWidth = devWidth / fixelsPerUnit;
        float screenHight = Screen.height;
        Debug.Log("Screen Height =" + screenHight);
        float orthgraphicSize = GetComponent<Camera>().orthographicSize;
        float aspetRatio = Screen.width * 1.0f / Screen.height;
        float cameraWidth = orthgraphicSize * 2 * aspetRatio;
        Debug.Log("Carema With =" + cameraWidth * fixelsPerUnit);
        if(cameraWidth < devWidth)
        {
            orthgraphicSize = devWidth / (2 * aspetRatio);
            Debug.Log("new orthographic size =" + orthgraphicSize);
            this.GetComponent<Camera>().orthographicSize = orthgraphicSize;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
