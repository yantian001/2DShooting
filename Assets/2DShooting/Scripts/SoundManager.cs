using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {


    public enum SoundType
    {
        HeadShot,
        TwoKill,
        ThreeKill,
        FourKill,
        FiveKill,
        SixKill,
        GameStart,
        GameLoaded,
        GameSuccess,
        GameFailed,
        GetLife,
        PlayerDie,
        WaveCountDown,
        WaveSuccess,
        WeaponUpgrade,
        WeaponEqiuped,
        WeaponBought,
        ButtonClicked
    }
    /*--
    **/
    public AudioClip headShotAudio;
    public AudioClip twoKillAudio;
    public AudioClip threeKillAudio;
    public AudioClip fourKillAudio;
    public AudioClip fiveKillAudio;
    public AudioClip sixKillAudio;
    public AudioClip gameStartAudio;
    public AudioClip gameLoadedAudio;
    public AudioClip gameSuccessAudio;
    public AudioClip gameFailedAudio;
    public AudioClip getLifeAudio;
    public AudioClip playerDie;
    public AudioClip waveCountDown;
    public AudioClip waveSuccess;
    public AudioClip weaponUpgrade;
    public AudioClip weaponEqiuped;
    public AudioClip weaponBought;
    public AudioClip buttonClicked;

    public static SoundManager _instance;

    public static SoundManager Instance
    {
        get
        {
            _instance = FindObjectOfType<SoundManager>();
            if (_instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = "SoundManagerContainer";
                _instance = obj.AddComponent<SoundManager>();
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }
    

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void PlaySound(SoundType st,float volume = 1.0f, float delay = 0.0f)
    {
        switch(st)
        {
            case SoundType.HeadShot:
                if (headShotAudio)
                    PlaySound(headShotAudio, volume, delay);
                break;
            case SoundType.TwoKill:
                if (twoKillAudio)
                {
                    PlaySound(twoKillAudio, volume, delay);
                }
                break;
            case SoundType.ThreeKill:
                if (threeKillAudio)
                {
                    PlaySound(threeKillAudio, volume, delay);
                }
                break;
            case SoundType.FourKill:
                if (fourKillAudio)
                {
                    PlaySound(fourKillAudio, volume, delay);
                }
                break;
            case SoundType.FiveKill:
                if (fiveKillAudio)
                {
                    PlaySound(fiveKillAudio, volume, delay);
                }
                break;
            case SoundType.SixKill:
                if (sixKillAudio)
                {
                    PlaySound(sixKillAudio, volume, delay);
                }
                break;
            case SoundType.GameStart:
                if(gameStartAudio)
                {
                    PlaySound(gameStartAudio, volume, delay);
                }
                break;
            case SoundType.GameLoaded:
                if(gameLoadedAudio)
                {
                    PlaySound(gameLoadedAudio, volume, delay);
                        
                }
                break;
            case SoundType.GameSuccess:
                if(gameSuccessAudio)
                {
                    PlaySound(gameSuccessAudio, volume, delay);
                }
                break;
            case SoundType.GameFailed:
                if (gameFailedAudio)
                {
                    PlaySound(gameFailedAudio, volume, delay);
                }
                break;
            case SoundType.GetLife:
                if(getLifeAudio)
                {
                    PlaySound(getLifeAudio, volume, delay);
                }
                break;
            case SoundType.PlayerDie:
                if(playerDie)
                {
                    PlaySound(playerDie, volume, delay);
                }
                break;
            case SoundType.WaveSuccess:
                if(waveSuccess)
                {
                    PlaySound(waveSuccess, volume, delay);
                }
                break;
            case SoundType.WaveCountDown:
                if(waveCountDown)
                {
                    PlaySound(waveCountDown, volume, delay);
                }
                break;
            case SoundType.WeaponUpgrade:
                if(weaponUpgrade)
                {
                    PlaySound(weaponUpgrade, volume, delay);

                }
                break;
            case SoundType.WeaponEqiuped:
                if(weaponEqiuped)
                {
                    PlaySound(weaponEqiuped, volume, delay);

                }
                break;
            case SoundType.WeaponBought:
                if(weaponBought)
                {
                    PlaySound(weaponBought, volume, delay);
                }
                break;
            case SoundType.ButtonClicked:
                if(buttonClicked)
                {
                    PlaySound(buttonClicked, volume, delay);
                }
                break;
            default:
                break;
        }
    }

    void PlaySound(AudioClip ac ,float volume ,float delay)
    {
        iTween.Stab(gameObject, iTween.Hash("audioclip",ac, "volume",volume, "delay", delay));
    }

    public static void PlayAduio(GameObject obj,AudioClip ac ,float volume = 1.0f, float delay = 0f)
    {
        iTween.Stab(obj, iTween.Hash("audioclip", ac, "volume", volume, "delay", delay));
    }

    /// <summary>
    /// 播放连杀音效
    /// </summary>
    /// <param name="combo"></param>
    /// <param name="headShot"></param>
    public void PlayComboSound(int combo,bool headShot = false)
    {
        if(headShot)
        {
            PlaySound(SoundType.HeadShot);
        }
        else if(combo > 1)
        {
            if(combo == 2)
            {
                PlaySound(SoundType.TwoKill);
            }
            else if(combo == 3)
            {
                PlaySound(SoundType.ThreeKill);
            }
            else if (combo == 4)
            {
                PlaySound(SoundType.FourKill);
            }
            else if (combo == 5)
            {
                PlaySound(SoundType.FiveKill);
            }
            else
            {
                PlaySound(SoundType.SixKill);
            }
        }
    }
}
