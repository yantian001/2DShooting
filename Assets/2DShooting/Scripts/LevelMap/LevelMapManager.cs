using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelMapManager : MonoBehaviour {

    // Use this for initialization
    /// <summary>
    /// 滑动区域
    /// </summary>
    [Tooltip("滑动列表")]
    public GameObject ScrollGrid;
    /// <summary>
    /// 场景缩略图
    /// </summary>
    public Image sceneThumb;

    public Button playButton;

    private int selectScene = -1;
    private GameDifficulty selectDifficulty = GameDifficulty.Normal;

    public void Start()
    {
        //附加选择事件
        if(ScrollGrid != null)
        {
            Toggle[] toggles = ScrollGrid.GetComponentsInChildren<Toggle>();
            if(toggles != null && toggles.Length > 0)
            {
               for(int i= 0;i<toggles.Length;i++)
                {
                    Toggle tog = toggles[i];
                    tog.onValueChanged.AddListener((b) => {
                        OnToggleValueChange(b, tog.GetComponent<LevelMapObject>());
                    });
                }
            }
        }

        //开始按钮点击事件
        if(playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }
    }

    void OnPlayButtonClicked()
    {
        if (selectScene != -1)
        {
            GameLogic.s_CurrentScene = selectScene;
            GameLogic.s_CurrentDifficulty = selectDifficulty;
            GameLogic.Loading();
        }
    }

    void OnToggleValueChange(bool selected , LevelMapObject mapObj)
    {
        if(selected)
        {
            if(mapObj)
            {
                selectScene = mapObj.level;
                if(sceneThumb)
                {
                    Rect textureRect = new Rect(sceneThumb.GetComponent<RectTransform>().rect);
                    textureRect.position = new Vector2(0, 0);
                    Sprite sp = Sprite.Create(mapObj.thumb,textureRect,sceneThumb.sprite.pivot);
                    sceneThumb.sprite = sp;
                }
            }
        }
        else
        {
            selectScene = -1;
            playButton.enabled = false;
        }
    }
}
