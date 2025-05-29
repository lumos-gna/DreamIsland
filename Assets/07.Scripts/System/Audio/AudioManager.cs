using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Background Music Clips (mp3)")]
    public AudioClip[] bgmClips; 

    [Header("Sound Effect Clips (ogg)")]
    public AudioClip[] sfxClips; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (bgmSource == null)
            bgmSource = gameObject.AddComponent<AudioSource>();
        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();
    }

    /// BGM 트랙을 재생합니다.
    public void PlayBGM(int index, bool loop = true)
    {
        if (index < 0 || index >= bgmClips.Length) return;
        bgmSource.clip = bgmClips[index];
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    /// 현재 BGM을 정지
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    /// 효과음을 재생
    public void PlaySFX(int index)
    {
        if (index < 0 || index >= sfxClips.Length) return;
        sfxSource.PlayOneShot(sfxClips[index]);
    }

    /// BGM 볼륨을 설정
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp01(volume);
    }

    /// 효과음 볼륨을 설정
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }
}
