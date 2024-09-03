using UnityHFSM;
using UnityEngine;
public class Healer : Entity
{
    WorldObject town;
    protected override void Start()
    {
        base.Start();
        entityStateMachine.AddState(State.Dead,
            onEnter: state =>
            {
                navigationSystem.Disable(true);
                meshRenderer.material = deadMaterial;
                _respawnTime = 0f;
            },
            onLogic: state =>
            {
            },
            onExit: state =>
            {
                navigationSystem.Disable(false);
                meshRenderer.material = aliveMaterial;
                health = maxHealth;
            }
            );
        entityStateMachine.AddState(State.MoveToSafeArea,
            onEnter: state => {
                navigationSystem.ChangeActiveNavigator(1);
                safetyValue = 0f;
            },
            onLogic: state => { 
                if(ObjectLookUp.ins.TryGetClosestObjectWithID(transform.position, ObjectLookUp.ObjectID.Town, out town))
                {
                    navigationSystem.ChangePoint(town.transform.position);
                    if(Vector3.Distance(transform.position, town.transform.position) < pickupDistance)
                    {
                        safetyValue += Time.deltaTime;
                        if(safetyValue > 1f)
                        {
                            entityStateMachine.RequestStateChange(State.MoveToCollectable);
                            safetyValue = 0f;
                        }
                    }
                }
            }
            );
    }
    public override bool Damage(int damage)
    {
        if(!base.Damage(damage))
        {
            entityStateMachine.RequestStateChange(State.MoveToSafeArea);
            return false;
        }
        return true;
    }
}