using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class PointScript : MonoBehaviour
{
    [Tooltip("Scale的倍数")]
    public float scaleTo = 1.2f;
    [Tooltip("Scale的时间")]
    public float scaleTime = 0.2f;
    [Tooltip("移动的距离")]
    public Vector2 moveBy = new Vector2(0, 50f);
    [Tooltip("移动的时间")]
    public float moveByTime = 0.3f;
    [Tooltip("淡出的时间")]
    public float fadeOutTime = 0.3f;
    [Tooltip("移动完成后，自动销毁")]
    public bool destroyOnComplete = true;
    // Use this for initialization
    RectTransform rect;
    Vector3 originScale;

    void Start()
    {
        //将字体放大到指定倍数
        LeanTween.scale(rect, originScale * scaleTo, scaleTime).setOnComplete(
            //Scale完成后执行缩小，移动
            () =>
            {
                LeanTween.scale(rect, originScale, scaleTime).setOnComplete(
                    () =>
                    {
                        LeanTween.move(rect, rect.anchoredPosition + moveBy, 0.2f).setOnComplete(
                        //移动完成后，淡出
                        () =>
                        {
                            LeanTween.textAlpha(rect, 0f, fadeOutTime).setOnComplete(
                                    //淡出后自动销毁
                                    () =>
                                    {
                                        if (destroyOnComplete)
                                        {
                                            Destroy(gameObject);
                                        }
                                    }
                                );
                        }
                        );
                    }
                    );

            }
            );
    }

    public void OnEnable()
    {
        rect = GetComponent<RectTransform>();
        originScale = rect.localScale;
        //rect.localScale = Vector3.zero;
    }


}
