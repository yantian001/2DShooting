using UnityEngine;
using System.Collections;
using Newtonsoft.Json;


public class WeaponManager : MonoBehaviour
{

    private static WeaponManager _instance;

    public static WeaponManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<WeaponManager>();
                if (_instance == null)
                {
                    var container = new GameObject();
                    container.name = "weaponManagerContainer";
                    _instance = container.AddComponent<WeaponManager>();
                }

            }
            return _instance;
        }
    }

    private WeaponItem[] Weapons;

    #region Monobehaviour
    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnEnable()
    {
        InitWeapon();
    }

    void InitWeapon()
    {
        string weaponStr = ((TextAsset)Resources.Load("GameData/weapon")).text;
        //Debug.Log(weaponStr);
        Weapons = JsonConvert.DeserializeObject<WeaponItem[]>(weaponStr);

        for (int i = 0; i < Weapons.Length; i++)
        {
            //Debug.Log(Weapons[i]);
            PlayerWeaponInfo pw = Player.CurrentPlayer.GetWeaponInfoById(Weapons[i].Id);
            if(pw != null)
            {
                Weapons[i].IsEnabled = pw.IsUnlocked;
                Weapons[i].Level = pw.Level;
            }
            else
            {
                if(Weapons[i].IsDefault)
                {
                    Player.CurrentPlayer.UnlockWeapon(Weapons[i].Id);
                }
            }
        }
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion
}
