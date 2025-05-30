using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Spatial SFX Settings")]
    // 볼륨 거리 설정 멀어질수로 0으로 감소
    public float maxSfxDistance = 10f;
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Background Music Clips (mp3)")]
    public AudioClip[] bgmClips;
    [Header("Sound Effect Clips (ogg)")]
    public AudioClip[] sfxClips;

    [Header("Volume Settings")]
    public float fixedBgmVolume = 0.08f;  

    [Header("Sound Effect Volumes")]
    [Range(0f, 1f)]
    public float[] sfxVolumes;

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

    public void PlaySFXAtPoint(int index, Vector3 sourcePosition)
    {
        if (index < 0 || index >= sfxClips.Length) return;

        // 기본 볼륨 (인스펙터에서 설정한 sfxVolumes)
        float baseVol = (sfxVolumes != null && index < sfxVolumes.Length)
                        ? sfxVolumes[index]
                        : 1f;

        // 청취자(카메라) 위치와 거리 계산
        Vector3 listenerPos = Camera.main.transform.position;
        float dist = Vector3.Distance(listenerPos, sourcePosition);

        // 거리 감쇠 계수 (0~1)
        float attenuation = Mathf.Clamp01(1f - (dist / maxSfxDistance));

        // 최종 볼륨
        float finalVol = baseVol * attenuation;

        // 3D 공간에 클립 재생
        AudioSource.PlayClipAtPoint(sfxClips[index], sourcePosition, finalVol);
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

        // 볼륨 배열이 설정돼 있으면 해당 인덱스, 아니면 1.0f
        float vol = (sfxVolumes != null && index < sfxVolumes.Length)
                    ? sfxVolumes[index]
                    : 1f;

        sfxSource.PlayOneShot(sfxClips[index], vol);
    }

    /// BGM 볼륨 설정
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp01(volume);
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

    /// BGM 볼륨 설정 전용매서드
    public static void SetBackgroundVolume(float volume)
    {
        Instance?.SetBGMVolume(volume);
    }

}
