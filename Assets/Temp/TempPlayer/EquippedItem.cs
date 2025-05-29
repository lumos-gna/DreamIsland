
using System;
using UnityEngine;

public abstract class EquippedItem : MonoBehaviour
{
    protected Animator _animator;
    protected Camera _camera;

    protected void Awake()
    {
        _animator = GetComponent<Animator>();
        _camera = Camera.main;
    }

    public abstract void Init(ItemDataSO itemDataSO);
    
    public abstract void Use();
    
}
