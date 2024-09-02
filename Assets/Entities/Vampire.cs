using UnityEngine;
using UnityHFSM;
public class Vampire : Entity
{
    public EntityType targetType;
    public Entity target;
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
                    if (target.Damage(1))
                    {
                        target = null;
                        entityStateMachine.RequestStateChange(State.FindTarget);
                    }
                }
            }
            );
        entityStateMachine.AddTransition(State.MoveToCollectable, State.FindTarget, state => health > healthFleeThreshold);
        entityStateMachine.AddTransition(State.Dead, State.FindTarget, state => health == maxHealth);
    }
    protected override void OnEnterDeath(State<State, string> state)
    {
        meshRenderer.material = deadMaterial;
        navigationSystem.SetSpeed(4f);
    }
    protected override void OnLogicDeath(State<State, string> state)
    {
        if(ObjectLookUp.ins.TryGetClosestObjectWithID(transform.position, ObjectLookUp.ObjectID.Blood, out collectable))
        {
            navigationSystem.ChangePoint(collectable.transform.position);
            if(Vector3.Distance(transform.position, collectable.transform.position) < pickupDistance)
            {
                if(collectable.TryInteract())
                {
                    //then use blood
                    health = maxHealth;
                    collectable = null;
                }
            }
        }
    }
    protected override void OnExitDeath(State<State, string> state)
    {
        base.OnExitDeath(state);
        navigationSystem.SetDefaultSpeeds();
        targetType = EntityType.Healer;
    }
    public override bool Damage(int damage)
    {
        if(!base.Damage(damage))
        {
            targetType = EntityType.Hunter;
            target = null;
            entityStateMachine.RequestStateChange(State.FindTarget);
            return false;
        }
        return true;
    }
}