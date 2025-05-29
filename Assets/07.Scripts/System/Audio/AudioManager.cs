using UnityEngine;

/// <summary>
/// 싱글톤 오디오 매니저
/// BGM과 효과음을 관리합니다.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Background Music Clips (mp3)")]
    public AudioClip[] bgmClips; // 4개

    [Header("Sound Effect Clips (ogg)")]
    public AudioClip[] sfxClips; // 14개

    [Header("Volume Settings")]
    public float fixedBgmVolume = 0.08f;   // ◀  추가

    private void Awake()
    {
        var sources = GetComponents<AudioSource>();

        if (sources.Length == 0)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        else if (sources.Length == 1)
        {
            bgmSource = sources[0];
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        else
        {
            bgmSource = sources[0];
            sfxSource = sources[1];
        }

        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }


    /// BGM 트랙 재생
    public void PlayBGM(int index, bool loop = true)
    {
        if (index < 0 || index >= bgmClips.Length) return;
        bgmSource.clip = bgmClips[index];
        bgmSource.loop = loop;
        bgmSource.volume = fixedBgmVolume; // bgm 볼륨 고정 
        bgmSource.Play();
    }

    /// BGM 정지
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    /// 효과음 재생
    public void PlaySFX(int index)
    {
        if (index < 0 || index >= sfxClips.Length) return;
        sfxSource.PlayOneShot(sfxClips[index]);
    }

    /// BGM 볼륨 설정
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp01(volume);
    }


    /// 효과음 볼륨 설정
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }


    /// 배경음악 재생 전용매서드
    public static void PlayBackgroundMusic(int index, bool loop = true)
    {
        Instance?.PlayBGM(index, loop);
    }

    /// 배경음악 정지 전용매서드 배경음악 정지
    public static void StopBackgroundMusic()
    {
        Instance?.StopBGM();
    }


    /// 효과음 재생 전용매서드
    public static void PlayEffectSound(int index)
    {
        Instance?.PlaySFX(index);
    }


    /// BGM 볼륨 설정 전용매서드
    public static void SetBackgroundVolume(float volume)
    {
        Instance?.SetBGMVolume(volume);
    }


    /// 효과음 볼륨 설정 전용매서드
    public static void SetEffectVolume(float volume)
    {
        Instance?.SetSFXVolume(volume);
    }
}
