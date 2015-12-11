using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelMapManager : MonoBehaviour {

    // Use this for initialization
    /// <summary>
    /// 所有场景的父对象
    /// </summary>
    [Tooltip("所有场景的父对象")]
    public GameObject Scenes;
    /// <summary>
    /// 场景缩略图
    /// </summary>
    public Image sceneThumb;
    /// <summary>
    /// 玩家名称显示
    /// </summary>
    public Text playerNameText;

    /// <summary>
    /// 开始按钮
    /// </summary>
    public Button playButton;

    private int selectScene = -1;
    private GameDifficulty selectDifficulty = GameDifficulty.Normal;

    public void Start()
    {
        //更新名称显示

        //附加选择事件
        if(Scenes != null)
        {
            Toggle[] toggles = Scenes.GetComponentsInChildren<Toggle>();
            if(toggles != null && toggles.Length > 0)
            {
               for(int i= 0;i<toggles.Length;i++)
                {
                    Toggle tog = toggles[i];
                    tog.onValueChanged.AddListener((b) => {
                        OnToggleValueChange(b, tog.GetComponent<LevelMapObject>());
                    });
                    if(tog.isOn)
                    {
                        OnToggleValueChange(true, tog.GetComponent<LevelMapObject>());
                    }
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
                //获得当前的排名

            }
        }
        else
        {
            selectScene = -1;
            //playButton.enabled = false;
        }
    }
}
