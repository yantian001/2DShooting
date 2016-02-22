using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class MoneyHandler : MonoBehaviour
{

    public Text[] moneyDisplays;

    private int currentMoney;
    // Use this for initialization
    void Start()
    {
        currentMoney = Player.CurrentPlayer.Money;
        if (moneyDisplays != null && moneyDisplays.Length > 0)
        {
            for (int i = 0; i < moneyDisplays.Length; i++)
            {
                if (moneyDisplays[i] != null)
                    moneyDisplays[i].text = currentMoney.ToString();
            }
        }
    }

    public void OnEnable()
    {
        LeanTween.addListener((int)Events.MONEYCHANGED, OnMoneyChanged);
    }

    private void OnMoneyChanged(LTEvent obj)
    {
        //throw new NotImplementedException();
       StartCoroutine(DynamicDisplayMoeny(Player.CurrentPlayer.Money, currentMoney, 100));
    }


    IEnumerator DynamicDisplayMoeny(int to, int from = 0, int time = 50)
    {
        if (moneyDisplays != null)
        {
            int diff = to - from;
            int normal = Mathf.CeilToInt((float)diff / time);
            while (from != to)
            {
                if (Mathf.Abs(to - from) >= Mathf.Abs(normal))
                {
                    from += normal;
                }
                else
                    from = to;
                for (int i = 0; i < moneyDisplays.Length; i++)
                {
                    if (moneyDisplays[i] != null)
                        moneyDisplays[i].text = from.ToString();
                }
                yield return null;
            }
        }
        currentMoney = to;
    }

    public void OnDestroy()
    {
        LeanTween.removeListener((int)Events.MONEYCHANGED, OnMoneyChanged);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
