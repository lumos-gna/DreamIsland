using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float DestoryTime = 3f;
    // Start is called before the first frame update
    public void CloseFloatingText()
    {
       this.gameObject.SetActive(false);
    }
}
