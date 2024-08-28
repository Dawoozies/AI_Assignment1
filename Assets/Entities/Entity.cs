using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int health;
    public int lowHealthThreshold;
    public int ammo;
    public int lowAmmoThreshold;
    public ObjectLookUp.ObjectID targetWhenHealthLow;
    public ObjectLookUp.ObjectID targetWhenAmmoLow;
    void Start()
    {
        
    }
    void Update()
    {
    }
}