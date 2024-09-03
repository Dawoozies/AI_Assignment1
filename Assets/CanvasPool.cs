using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uPools;
public class CanvasPool : MonoBehaviour
{
    public static CanvasPool ins;
    void Awake()
    {
        ins = this;
    }
    public void Rent()
    {

    }
}