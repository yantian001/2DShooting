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

    /// <summary>
    /// 获取武器对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public WeaponItem GetWeaponItemById(int id)
    {
        WeaponItem wi = null;
        if(Weapons!= null && Weapons.Length > 0)
        {
            for(int i=0;i<Weapons.Length;i++)
            {
                if (Weapons[i].Id == id)
                {
                    wi = Weapons[i];
                    break;
                }
            }
        }
        return wi;
    }
    /// <summary>
    /// 获取武器等级信息
    /// </summary>
    /// <param name="id">武器ID</param>
    /// <param name="level">武器等级</param>
    /// <returns></returns>
    public WeaponProperty GetPropertyById(int id,int level)
    {
        WeaponProperty wp = null;
        WeaponItem wi = GetWeaponItemById(id);
        if(wi != null)
        {
            //当level = -1时获取当前武器信息
            if(level == -1)
            {
                wp = wi.Levels[wi.Level];
            }
            else if (level < wi.Levels.Length && level >= 0)
                wp = wi.Levels[level];
        }
        return wp;
    }
    /// <summary>
    /// 获取武器当前的等级信息
    /// </summary>
    /// <param name="id">武器Id</param>
    /// <returns></returns>
    public WeaponProperty GetCurrentPropertyById(int id)
    {
        return GetPropertyById(id, -1);
    }
    /// <summary>
    /// 获取武器当前等级
    /// </summary>
    /// <param name="id">武器ID</param>
    /// <returns>武器等级 ,异常时返回 -2</returns>
    public int GetWeaponCurrentLevel(int id)
    {
        int level = -2;
        WeaponItem wi = GetWeaponItemById(id);
        if (wi != null)
            level = wi.Level;
        return level;
    }

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
