using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private const string PrefabPath = "Audio/";

    private ObjectPool<PooledAudioSource> _audioPool;
    
    public void Play(AudioClip clip, bool isLoop)
    {
        if (_audioPool == null)
        {
            PooledAudioSource prefab = Resources.Load<PooledAudioSource>($"{PrefabPath}{nameof(PooledAudioSource)}");

            _audioPool = PoolManager.Instance.CreatePool(prefab);
        }

        PooledAudioSource source = _audioPool.Spawn(null);
        
        source.OnPlay(clip, isLoop);
    }
}
