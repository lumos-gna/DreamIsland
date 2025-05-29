using System.Collections;
using UnityEngine;

public class PooledAudioSource : MonoBehaviour, IPoolable
{
    [SerializeField] private AudioSource audioSource;

    private Coroutine _clipCoroutine;
    

    public void OnSpawn()
    {
        gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
        gameObject.SetActive(false);

        if (_clipCoroutine != null)
        {
            StopCoroutine(_clipCoroutine);
        }
    }

    public void OnPlay(AudioClip clip, bool isLoop)
    {
        audioSource.clip = clip;

        audioSource.loop = isLoop;
        
        audioSource.Play();
        
        _clipCoroutine = StartCoroutine(ClipEnumerator(audioSource.clip.length));
    }
    

    IEnumerator ClipEnumerator(float duration)
    {
        yield return new WaitForSeconds(duration);
        
        PoolManager.Instance.GetPool<PooledAudioSource>(nameof(PooledAudioSource)).Despawn(this);
    }
}
