using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class WaveDataCreator  {

#if UNITY_EDITOR
    [MenuItem("2DShooting/Create WaveData")]
    public static void Execute()
    {
        WaveData data = ScriptableObject.CreateInstance("WaveData") as WaveData;
        AssetDatabase.CreateAsset(data, "Assets/2DShooting/Resources/GameData/Wave1.asset");
        AssetDatabase.Refresh();
    }
#endif
}
