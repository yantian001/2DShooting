using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {



    #region 连杀UI
    public GameObject Combo;
    public Text comboText;
    bool isComboShow = false;
    public void ShowCombo(int icombo)
    {
        if(!isComboShow)
        {
            iTween.MoveTo(Combo, iTween.Hash( "position",new Vector3(10, -21, 0),"time", 0.1f,"islocal",true));
            isComboShow = true;
        }
        UpdateComboText(icombo);
    }
    void UpdateComboText(int icombo)
    {
        comboText.text = icombo.ToString();
    }

    public void HideCombo()
    {
        if(isComboShow)
        {
            iTween.MoveTo(Combo, iTween.Hash("position", new Vector3(210, -21, 0), "time", 0.1f, "islocal", true));
            isComboShow = false;
        }
    }
    #endregion

    #region 单例模式
    public static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
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
    // Update is called once per frame
    void Update () {
	
	}
}
