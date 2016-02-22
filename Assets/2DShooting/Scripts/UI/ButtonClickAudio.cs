using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonClickAudio : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        var buttons = FindObjectsOfType<Button>();
        if (buttons != null)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].onClick.AddListener(OnButtonClick);
            }
        }

    }

    void OnButtonClick()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundType.ButtonClicked);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
