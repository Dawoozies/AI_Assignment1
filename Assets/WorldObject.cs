using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    public ObjectLookUp.ObjectID id;
    public ObjectLookUp.ObjectID normalID;
    public ObjectLookUp.ObjectID usedID;
    public float regenTime;
    float _regenTime;
    private void Update()
    {
        if (_regenTime < regenTime)
        {
            _regenTime += Time.deltaTime;
        }
        else
        {
            ObjectLookUp.ins.ChangeState(this, normalID);
        }
    }
    public bool TryInteract()
    {
        if (_regenTime >= regenTime)
        {
            _regenTime = 0f;
            if(usedID != ObjectLookUp.ObjectID.Town)
            {
                ObjectLookUp.ins.ChangeState(this, usedID);
            }
            return true;
        }

        return false;
    }
}
