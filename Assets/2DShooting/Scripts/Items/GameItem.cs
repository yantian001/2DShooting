using UnityEngine;
using System.Collections;

public class GameItem : MonoBehaviour
{

    /// <summary>
    /// 道具类型
    /// </summary>
    [Tooltip("道具类型")]
    public GameItemType itemType;

    /// <summary>
    /// 道具的生命值
    /// </summary>
    [Tooltip("道具的生命值")]
    public float _HP = 1.0f;
    /// <summary>
    /// 道具效果
    /// </summary>
    [Tooltip("道具效果")]
    public float itemEffectValue;
    /// <summary>
    /// 是否现在存在时间,
    /// </summary>
    [Tooltip("是否限制时间")]
    public bool isLimitTime = false;

    /// <summary>
    /// 存在时间
    /// </summary>
    [Tooltip("存在时间")]
    public float lifeSpan = 0.0f;
    /// <summary>
    /// 是否显示倒计时条
    /// </summary>
    [Tooltip("是否显示倒计时条")]
    public bool isDisplayTimeRemain = false;
    /// <summary>
    /// 倒计时背景
    /// </summary>
    [Tooltip("倒计时背景图片")]
    public Texture2D timeRemainBackground;
    [Tooltip("倒计时填充图片")]
    public Texture2D timeRemainFill;
    /// <summary>
    /// 破碎的声音
    /// </summary>
    [Tooltip("破碎的声音")]
    public AudioClip brokenAc;

    private bool isDie = false;
    float currentTime = 0;
    void Start()
    {
        isDie = false;
        //如果限制时间 
        if(isLimitTime)
        {
            Destroy(gameObject, lifeSpan);
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
    }

    public void OnGUI()
    {
       
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider == null)
            return;
        Vector3 worldPos = new Vector3(transform.position.x, transform.position.y + collider.bounds.size.y, transform.position.z);
        //得到屏幕坐标
        Vector2 position = Camera.main.WorldToScreenPoint(worldPos);
        //得到真实的坐标
        position = new Vector2(position.x, Screen.height - position.y);
        Vector3 scale = transform.localScale;
        Vector2 silderSize = GUI.skin.label.CalcSize(new GUIContent(timeRemainFill));
        silderSize = new Vector2(silderSize.x * scale.x, silderSize.y * scale.y);
        float fillWidth = (timeRemainFill.width  * (lifeSpan - currentTime) / lifeSpan)*scale.x ;

        GUI.DrawTexture(new Rect(position.x - silderSize.x / 2, position.y - silderSize.y / 2, silderSize.x, silderSize.y), timeRemainBackground);
        GUI.DrawTexture(new Rect(position.x - silderSize.x / 2, position.y - silderSize.y / 2, fillWidth, silderSize.y), timeRemainFill);
    }

    public void TakeDamage(float attck)
    {
        if (IsDie())
            return;

        _HP -= attck;
        if(_HP <= 0)
        {
            Die();
        }
    }
    /// <summary>
    /// 是否已死亡
    /// </summary>
    /// <returns></returns>
    public bool IsDie()
    {
        return isDie;
    }

    void Die()
    {
        int eventId = -1;
        switch(itemType)
        {
            case GameItemType.ItemMedkit:
                eventId = (int)Events.ITEMMEDKITHIT;
                break;
            case GameItemType.ItemShield:
                eventId = (int)Events.ITEMSHIELDHIT;
                break;
        }
        if(eventId != -1)
        {
            LeanTween.dispatchEvent(eventId,itemEffectValue);
        }

        if(brokenAc)
        {
            SoundManager.PlayAduio(gameObject, brokenAc);
        }

        Destroy(gameObject, 0.1f);
    }
}
