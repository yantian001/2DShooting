using UnityEngine;
using System.Collections;

public class UIBlood : MonoBehaviour
{

    float maxY;

    float maxX;

    public GameObject[] bloods;
    // Use this for initialization
    void Start()
    {
        maxY = Camera.main.orthographicSize;
        maxX = maxY * Camera.main.aspect;
    }

    public void OnEnable()
    {
        LeanTween.addListener((int)Events.CREATEBLOOD, CreateBlood);
    }

    public void OnDisable()
    {
        LeanTween.removeListener((int)Events.CREATEBLOOD, CreateBlood);
    }

    void CreateBlood(LTEvent evt)
    {
        if (bloods == null)
            return;

        GameObject blood = bloods[Random.Range(0, bloods.Length)];
        if (blood != null)
        {
            Vector3 createPos = new Vector3(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY), 1);
            var created = (GameObject)Instantiate(blood, createPos, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            float scale = Random.Range(5f, 8f);
            created.transform.localScale = new Vector3(scale, scale, 1);
            created.transform.SetParent(Camera.main.transform);
        }

    }
}
