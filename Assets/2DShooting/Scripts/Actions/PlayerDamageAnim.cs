using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerDamageAnim : MonoBehaviour {

    public Color flashColor;
    public float flashSpeed = 5f;
    public bool damaged = false;
    public Image damageImage;


    void PlayDamageEffect()
    {
        if(damaged)
        {
            damageImage.color = flashColor;
            damaged = false;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, Time.deltaTime * flashSpeed);
        }
    }

    void Update()
    {
        PlayDamageEffect();
    }
}
