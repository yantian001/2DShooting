using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelMapObject : MonoBehaviour {

    /// <summary>
    /// 场景level
    /// </summary>
    [Tooltip("场景Level")]
    public int level = 1;
    [Tooltip("场景缩略图")]
    public Texture2D thumb;
    [Tooltip("排行榜ID")]
    public string LeardBoardID = "";

}
