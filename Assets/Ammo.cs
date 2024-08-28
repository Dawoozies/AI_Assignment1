using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public float regenTime;
    float _regenTime;
    private void Update()
    {
        if(_regenTime < regenTime)
        {
            _regenTime += Time.deltaTime;
        }
    }
    public bool TryUseAmmo()
    {
        if(_regenTime >= regenTime)
        {
            _regenTime = 0f;
            return true;
        }

        return false;
    }
}
