using UnityEngine;

public class WorldObject : MonoBehaviour
{
    public ObjectLookUp.ObjectID id;
    public ObjectLookUp.ObjectID normalID;
    public ObjectLookUp.ObjectID usedID;
    public float regenTime;
    float _regenTime;
    public MeshRenderer meshRenderer;
    public Material canBeUsed, depleted;
    private void Start()
    {
        ObjectLookUp.ins.AddObject(this);
        meshRenderer = GetComponent<MeshRenderer>();
        _regenTime = regenTime;
    }
    private void Update()
    {
        if (_regenTime < regenTime)
        {
            _regenTime += Time.deltaTime;
            meshRenderer.material = depleted;
        }
        else
        {
            ObjectLookUp.ins.ChangeState(this, normalID);
            meshRenderer.material = canBeUsed;
        }
    }
    public bool TryInteract()
    {
        if(id == ObjectLookUp.ObjectID.HealthPackDepleted)
        {
            _regenTime = regenTime;
            return true;
        }
        if (_regenTime >= regenTime)
        {
            _regenTime = 0f;
            if (usedID != ObjectLookUp.ObjectID.Town)
            {
                ObjectLookUp.ins.ChangeState(this, usedID);
            }
            return true;
        }

        return false;
    }
}
