using System.Collections.Generic;
using UnityEngine;

public class AudioSfxHandler : MonoBehaviour
{
    [SerializeField] private List<Models.SerializableKeyValuePair<Enums.AudioSfxType, AudioSource>> audioTypeToSourceMapList = new();

    private readonly Dictionary<Enums.AudioSfxType, AudioSource> audioTypeToSourceMap = new();
    private int isVolumeOn = -1; // -1 Undefined, 0 Off, 1 On

    public static AudioSfxHandler Instance { get; private set; }

    public bool IsMute
    {
        get
        {
            if (isVolumeOn == -1)
            {
                isVolumeOn = PlayerPrefs.GetInt(Constants.SOUND_SFX_STATE, 1);
            }

            return isVolumeOn != 1;
        }
        set
        {
            isVolumeOn = value ? 0 : 1;
            PlayerPrefs.SetInt(Constants.SOUND_SFX_STATE, isVolumeOn);
            PlayerPrefs.Save();
        }
    }

    private void Awake()
    {
        Instance = this;

        foreach (var item in audioTypeToSourceMapList)
        {
            audioTypeToSourceMap.TryAdd(item.key, item.value);
        }
    }

    public void PlayAudio(Enums.AudioSfxType sfxType)
    {
        PerformAction(sfxType, Enums.AudioAction.Play);
    }

    public void PlayAudioOneShot(Enums.AudioSfxType sfxType)
    {
        PerformAction(sfxType, Enums.AudioAction.PlayOneShot);
    }

    public void StopAudio(Enums.AudioSfxType sfxType)
    {
        PerformAction(sfxType, Enums.AudioAction.Stop);
    }

    private void PerformAction(Enums.AudioSfxType sfxType, Enums.AudioAction action)
    {
        switch (action)
        {
            case Enums.AudioAction.Play:
                if (IsMute)
                {
                    return;
                }
                audioTypeToSourceMap[sfxType].Play();
                break;
            case Enums.AudioAction.PlayOneShot:
                if (IsMute)
                {
                    return;
                }
                var source = audioTypeToSourceMap[sfxType];
                source.PlayOneShot(source.clip);
                break;
            case Enums.AudioAction.Stop:
                if (audioTypeToSourceMap[sfxType].isPlaying)
                {
                    audioTypeToSourceMap[sfxType].Stop();
                }
                break;
            case Enums.AudioAction.Pause:
                if (audioTypeToSourceMap[sfxType].isPlaying)
                {
                    audioTypeToSourceMap[sfxType].Pause();
                }
                break;
            case Enums.AudioAction.Resume:
                if (IsMute)
                {
                    return;
                }
                if (!audioTypeToSourceMap[sfxType].isPlaying)
                {
                    audioTypeToSourceMap[sfxType].Play();
                }
                break;
            default:
                if (audioTypeToSourceMap[sfxType].isPlaying)
                {
                    audioTypeToSourceMap[sfxType].Stop();
                }
                break;
        }
    }
}
