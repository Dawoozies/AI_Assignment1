using UnityEngine;
using UnityHFSM;

public class Hunter : Entity
{
    public EntityType targetType;
    public Entity target;
    public int ammo;
    public float attackDistance;
    public float attackTime;
    float _attackTime;
    public int healthFleeThreshold;
    protected override void Start()
    {
        base.Start();
        //add states for fleeing and killing
        entityStateMachine.AddState(State.FindTarget,
            onLogic: state =>
            {
                if (ObjectLookUp.ins.TryGetClosestEntityOfType(transform.position, targetType, out target))
                {
                    entityStateMachine.RequestStateChange(State.MoveToTarget);
                }
            });
        entityStateMachine.AddState(State.MoveToTarget,
            onEnter: state => { navigationSystem.ChangeActiveNavigator(1); },
            onLogic: state =>
            {
                navigationSystem.ChangePoint(target.transform.position);
                if (_attackTime < attackTime)
                {
                    _attackTime += Time.deltaTime;
                }
                if (_attackTime >= attackTime && Vector3.Distance(transform.position, target.transform.position) < attackDistance)
                {
                    _attackTime = 0f;
                    ammo--;
                    if (target.Damage(1))
                    {
                        target = null;
                        entityStateMachine.RequestStateChange(State.FindTarget);
                    }
                }
            }
            );
        entityStateMachine.AddTransition(State.MoveToCollectable, State.FindTarget, state => ammo > 0 && health > healthFleeThreshold);
        entityStateMachine.AddTransition(State.FindTarget, State.FindCollectable, state => ammo <= 0);
        entityStateMachine.AddTransition(State.MoveToTarget, State.FindCollectable, state => ammo <= 0);
        onCollectablePickup = (ObjectLookUp.ObjectID pickedUpId) => { 
            ammo = 5;
            collectable = null;
        };
    }
}
