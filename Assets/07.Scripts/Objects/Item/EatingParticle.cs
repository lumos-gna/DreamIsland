using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingParticle : MonoBehaviour
{
    private ParticleSystem Eating;
    void Start()
    {
        TryGetComponent<ParticleSystem>(out Eating);
    }

    public void StartEating()
    {
        if(Eating != null)
        {
            Eating.transform.position = gameObject.transform.position;
            Eating.Play();
        }
    }

    public void EndEating()
    {
        if (Eating != null)
        {
            Eating.Stop();
        }
    }

}
