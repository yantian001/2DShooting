using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CommonUtils
{

    /// <summary>
    /// 是否能正常联网
    /// </summary>
    /// <returns></returns>
    public static bool IsNetworkOk()
    {
        return !(Application.internetReachability == NetworkReachability.NotReachable);
    }

    /// <summary>
    /// 设置子节点的文本值
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    /// <param name="text"></param>
    public static void SetChildText(RectTransform parent, string child, string text)
    {
        var childNode = parent.FindChild(child);
        if (childNode != null)
        {
            var textNode = childNode.GetComponent<Text>();
            if (textNode)
            {
                textNode.text = text;
            }
        }
    }

    /// <summary>
    /// 设置RawImage子节点的Texture2d值
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    /// <param name="texture"></param>
    public static void SetChildRawImage(RectTransform parent, string child, Texture2D texture)
    {
        if (parent == null || texture == null || string.IsNullOrEmpty(child))
            return;
        var childNode = parent.FindChild(child);
        if (childNode != null)
        {
            var img = childNode.GetComponent<RawImage>();
            if (img != null)
            {
                img.texture = texture;
            }
        }
    }
    /// <summary>
    /// 设置子节点是否可见
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    /// <param name="isActive"></param>
    public static void SetChildActive(RectTransform parent, string child, bool isActive)
    {
        if (parent == null || string.IsNullOrEmpty(child))
            return;
        var childNode = parent.FindChild(child);
        if (childNode != null)
        {
            childNode.gameObject.SetActive(isActive);
        }
    }

    /// <summary>
    /// 设置子对象的Slider值
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    /// <param name="value"></param>
    public static void SetChildSliderValue(RectTransform parent, string child, float value)
    {
        var childTran = parent.FindChild(child);
        if (childTran)
        {
            Slider slider = childTran.GetComponentInChildren<Slider>();
            if (slider)
            {
                slider.value = value;
            }
        }
    }

    public static void SetChildButtonActive(RectTransform parent, string child, bool b)
    {
        if (parent == null || string.IsNullOrEmpty(child))
            return;

        var childNode = parent.FindChild(child);
        if (childNode)
        {
            var button = childNode.GetComponent<Button>();
            button.interactable = b;
        }

    }
}
