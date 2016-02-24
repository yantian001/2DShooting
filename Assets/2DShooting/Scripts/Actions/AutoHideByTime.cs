using UnityEngine;
using System.Collections;

public class AutoHideByTime : MonoBehaviour
{
    /// <summary>
    /// 自动隐藏时间
    /// </summary>
    [Tooltip("自动隐藏时间")]
    public float hideTime = 2.0f;

    public void OnEnable()
    {
        Invoke("Hide", hideTime);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
