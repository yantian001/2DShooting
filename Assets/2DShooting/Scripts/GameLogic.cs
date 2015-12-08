using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
        LeanTween.addListener((int)Events.GAMERESTART, OnGameRestart);
    }

    void OnGameRestart(LTEvent evt)
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDestroy()
    {
       // Debug.Log("OnDestroy");
    }

    public void OnDisable()
    {
        // Debug.Log("OnDisable");
        LeanTween.removeListener((int)Events.GAMERESTART, OnGameRestart);
    }
}
