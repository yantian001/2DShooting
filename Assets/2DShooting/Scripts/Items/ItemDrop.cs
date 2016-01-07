using UnityEngine;
using System.Collections;

public class ItemDrop : MonoBehaviour {

    // Use this for initialization
    int i = 0;

    public float aliveTime = 10f;

    public float dropSpeed = 0.00f;
	void Start () {
        MoveX();
        LeanTween.moveY(gameObject, transform.position.y + dropSpeed * aliveTime, aliveTime).setDestroyOnComplete(true);
	}

    void MoveX()
    {
        float x = 0.1f;
        if (i % 2 == 0)
            x = -x;
        i++;
        LeanTween.moveX(gameObject, transform.position.x + x, 1f).setOnComplete(MoveX);
    }



	// Update is called once per frame
	void Update () {
       // gameObject.transform.position = gameObject.transform.position+ new Vector3(0, -dropSpeed * Time.deltaTime,0);
	}
}
