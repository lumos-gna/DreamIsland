using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem eating;

    public void StartEating()
    {
        if (eating != null)
        {
            eating.transform.position = gameObject.transform.position;
            eating.Play();
            Debug.Log("��� ��ƼŬ ����");
        }
    }

    public void EndEating()
    {
        if (eating != null)
        {
            eating.Stop();
        }
    }
}
