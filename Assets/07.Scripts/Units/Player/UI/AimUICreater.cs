using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimUICreater : MonoBehaviour
{
    void Start()
    {
        UIManager.Instance.Create<AimUI>();
    }
}
