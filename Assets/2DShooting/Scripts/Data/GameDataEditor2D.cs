using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameDataEditor2D  {
    #if UNITY_EDITOR
    [MenuItem("2DShooting/Create GameData")]
    public static void Execute()
    {
        GameData data = ScriptableObject.CreateInstance("GameData") as GameData;
        AssetDatabase.CreateAsset(data, "Assets/2DShooting/Resources/GameData/Level1.asset");
        AssetDatabase.Refresh();
    }
    #endif
}
