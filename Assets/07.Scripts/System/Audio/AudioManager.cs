using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Spatial SFX Settings")]
    // ���� �Ÿ� ���� �־������� 0���� ����
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

        // �⺻ ���� (�ν����Ϳ��� ������ sfxVolumes)
        float baseVol = (sfxVolumes != null && index < sfxVolumes.Length)
                        ? sfxVolumes[index]
                        : 1f;

        // û����(ī�޶�) ��ġ�� �Ÿ� ���
        Vector3 listenerPos = Camera.main.transform.position;
        float dist = Vector3.Distance(listenerPos, sourcePosition);

        // �Ÿ� ���� ��� (0~1)
        float attenuation = Mathf.Clamp01(1f - (dist / maxSfxDistance));

        // ���� ����
        float finalVol = baseVol * attenuation;

        // 3D ������ Ŭ�� ���
        AudioSource.PlayClipAtPoint(sfxClips[index], sourcePosition, finalVol);
    }

    /// BGM Ʈ�� ���
    public void PlayBGM(int index, bool loop = true)
    {
        if (index < 0 || index >= bgmClips.Length) return;
        bgmSource.clip = bgmClips[index];
        bgmSource.loop = loop;
        bgmSource.volume = fixedBgmVolume; // bgm ���� ���� 
        bgmSource.Play();
    }

    /// BGM ����
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    /// ȿ���� ���
    public void PlaySFX(int index)
    {
        if (index < 0 || index >= sfxClips.Length) return;

        // ���� �迭�� ������ ������ �ش� �ε���, �ƴϸ� 1.0f
        float vol = (sfxVolumes != null && index < sfxVolumes.Length)
                    ? sfxVolumes[index]
                    : 1f;

        sfxSource.PlayOneShot(sfxClips[index], vol);
    }

    /// BGM ���� ����
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp01(volume);
    }

    /// ������� ��� ����ż���
    public static void PlayBackgroundMusic(int index, bool loop = true)
    {
        Instance?.PlayBGM(index, loop);
    }

    /// ������� ���� ����ż��� ������� ����
    public static void StopBackgroundMusic()
    {
        Instance?.StopBGM();
    }

    /// BGM ���� ���� ����ż���
    public static void SetBackgroundVolume(float volume)
    {
        Instance?.SetBGMVolume(volume);
    }
}
