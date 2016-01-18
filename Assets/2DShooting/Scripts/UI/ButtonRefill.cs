using UnityEngine;
using System.Collections;

public class ButtonRefill : MonoBehaviour {

    public GameObject moneyPerfab;

    public Transform moneyCreatePosition;

    public void OnButtonRefillClick()
    {
        if(Player.CurrentPlayer.Money >= 50)
        {
            if (moneyPerfab && moneyCreatePosition)
            {
                var createObj = (GameObject)Instantiate(moneyPerfab, moneyCreatePosition.position, Quaternion.identity);
                createObj.transform.SetParent(moneyCreatePosition.parent);
                createObj.transform.localScale = new Vector3(1f, 1f, 1f);
                LeanTween.moveY(createObj, createObj.transform.position.y - 50, 1f).setDestroyOnComplete(true);
            }
            Player.CurrentPlayer.UseMoney(50);
            LeanTween.dispatchEvent((int)Events.CLIPREFILL);
            SoundManager.Instance.PlaySound(SoundManager.SoundType.WeaponEqiuped);
        }
       else
        {
            Message.PopupMessage("NOT ENOUGH CASH!", 1f);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
