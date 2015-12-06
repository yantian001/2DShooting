using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{



    #region 连杀UI
    public RectTransform Combo;
    public Text comboText;
    bool isComboShow = false;
    public void ShowCombo(int icombo)
    {
        if (!isComboShow)
        {
            LeanTween.move(Combo, new Vector3(10f, -31.5f, 0), 0.1f);
            isComboShow = true;
        }
        UpdateComboText(icombo);
    }
    void UpdateComboText(int icombo)
    {
        RectTransform rect = comboText.GetComponent<RectTransform>();
        Vector3 loclScale = rect.localScale;
        LeanTween.scale(rect, loclScale * 1.5f, 0.05f).setOnComplete(() =>
        {
            comboText.text = icombo.ToString();
            LeanTween.scale(rect, loclScale, 0.05f);
        });
        comboText.text = icombo.ToString();
    }

    public void HideCombo()
    {
        if (isComboShow)
        {
            LeanTween.move(Combo.GetComponent<RectTransform>(), new Vector3(250f, -31.5f, 0), 0.1f);
            isComboShow = false;
        }
    }
    #endregion

    #region 弹夹
    public Text bulletClipText;

    public void UpdateBulletDisplay(LTEvent evt)
    {
        if (bulletClipText != null && evt.data != null)
        {
            bulletClipText.text = evt.data.ToString();
        }
    }

    public void OnClipButtonClick()
    {
        LeanTween.dispatchEvent((int)Events.RELOAD);
    }
    #endregion

    #region 分数显示
    [Tooltip("连击分数预制")]
    public GameObject comboPoint;
    [Tooltip("爆头分数预制")]
    public GameObject headShotPoint;
    [Tooltip("显示分数的位置")]
    public RectTransform pointTransform;

    public void ShowPoint(int score , bool isHeadShot)
    {
        if(pointTransform == null)
        {
            return;
        }
        GameObject createPrefab = comboPoint;
        if(isHeadShot && headShotPoint)
        {
            createPrefab = headShotPoint;
        }
        if (!createPrefab)
            return;
        GameObject created = Instantiate(createPrefab);
        RectTransform createdRectTransform = created.GetComponent<RectTransform>();
        //createdRectTransform. = pointTransform;
        createdRectTransform.SetParent(pointTransform.parent);
        createdRectTransform.anchoredPosition = pointTransform.anchoredPosition;
        createdRectTransform.anchorMax = pointTransform.anchorMax;
        createdRectTransform.anchorMin = pointTransform.anchorMin;
        createdRectTransform.pivot = pointTransform.pivot;
        //createdRectTransform.
        //更改分数显示
        Text pointTxt = created.GetComponent<Text>();
        if(pointTxt)
        {
            pointTxt.text = string.Format(pointTxt.text, score);
        }
        
    }

    #endregion

    #region 单例模式
    public static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "UIManagerContainer";
                    _instance = obj.AddComponent<UIManager>();
                }
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }

    }
    #endregion


    public void OnEnable()
    {
       
    }

    public void Awake()
    {
        //添加子弹数量变化事件
        LeanTween.addListener(gameObject, (int)Events.BULLETCHANGED, UpdateBulletDisplay);
        Debug.Log("UIManager Inited");
    }

    public void OnDisable()
    {
        //移除事件
        LeanTween.removeListener((int)Events.BULLETCHANGED, UpdateBulletDisplay);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
