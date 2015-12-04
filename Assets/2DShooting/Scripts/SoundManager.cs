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
        SixKill
    }
    /*--
    **/
    public AudioClip headShotAudio;
    public AudioClip twoKillAudio;
    public AudioClip threeKillAudio;
    public AudioClip fourKillAudio;
    public AudioClip fiveKillAudio;
    public AudioClip sixKillAudio;

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
            default:
                break;
        }
    }

    void PlaySound(AudioClip ac ,float volume ,float delay)
    {
        iTween.Stab(gameObject, iTween.Hash("audioclip",ac, "volume",volume, "delay", delay));
    }

    public void PlayComboSound(int combo)
    {
        if(combo > 1)
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
