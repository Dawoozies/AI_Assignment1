using System;
using UnityEngine;
using UnityHFSM;
public abstract class Entity : MonoBehaviour
{
    protected NavigationSystem navigationSystem;
    protected StateMachine<State> entityStateMachine;
    public EntityType entityType;
    public int maxHealth;
    public int health;
    public float respawnTime;
    protected float _respawnTime;
    public State state;
    public ObjectLookUp.ObjectID collectableID;
    public WorldObject collectable;
    public MeshRenderer meshRenderer;
    public Material aliveMaterial, deadMaterial;
    public float pickupDistance;
    protected Action<ObjectLookUp.ObjectID> onCollectablePickup;
    public float safetyValue;
    protected virtual void Start()
    {
        navigationSystem = GetComponent<NavigationSystem>();
        meshRenderer = GetComponent<MeshRenderer>();
        entityStateMachine = new StateMachine<State>();

        entityStateMachine.AddState(State.Null);
        entityStateMachine.AddState(State.RandomWalk, onEnter: state => { navigationSystem.ChangeActiveNavigator(0); }, 
            onLogic: state => { 
                if(ObjectLookUp.ins.TryGetClosestObjectWithID(transform.position, collectableID, out collectable))
                {
                    entityStateMachine.RequestStateChange(State.MoveToCollectable);
                }
            });
        entityStateMachine.AddState(State.FindCollectable,
            onLogic: state =>
            {
                if (ObjectLookUp.ins.TryGetClosestObjectWithID(transform.position, collectableID, out collectable))
                {
                    entityStateMachine.RequestStateChange(State.MoveToCollectable);
                }
            });
        entityStateMachine.AddState(State.MoveToCollectable,
            onEnter: state => { navigationSystem.ChangeActiveNavigator(1); },
            onLogic: state =>
            {
                if(ObjectLookUp.ins.TryGetClosestObjectWithID(transform.position, collectableID, out collectable))
                {
                    navigationSystem.ChangePoint(collectable.transform.position);
                    if (Vector3.Distance(transform.position, collectable.transform.position) < pickupDistance)
                    {
                        ObjectLookUp.ObjectID oldObjectID = collectable.id;
                        if (collectable.TryInteract())
                        {
                            onCollectablePickup?.Invoke(oldObjectID);
                        }
                    }
                }
                else
                {
                    entityStateMachine.RequestStateChange(State.RandomWalk);
                }
            });

        entityStateMachine.AddState(State.Dead, OnEnterDeath, OnLogicDeath, OnExitDeath);

        entityStateMachine.Init();

        entityStateMachine.StateChanged += (state) => { this.state = state.name; };

        entityStateMachine.RequestStateChange(State.FindCollectable);

        ObjectLookUp.ins.AddEntity(this);
    }
    protected virtual void Update()
    {
        entityStateMachine.OnLogic();
    }
    public virtual bool Damage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            entityStateMachine.RequestStateChange(State.Dead);
            return true;
        }
        return false;
    }
    protected virtual void OnEnterDeath(State<State, string> state)
    {
        navigationSystem.Disable(true);
        meshRenderer.material = deadMaterial;
        _respawnTime = 0f;
    }
    protected virtual void OnLogicDeath(State<State, string> state)
    {
        if (_respawnTime < respawnTime)
        {
            _respawnTime += Time.deltaTime;
        }
        else
        {
            entityStateMachine.RequestStateChange(State.FindCollectable);
        }
    }
    protected virtual void OnExitDeath(State<State, string> state)
    {
        navigationSystem.Disable(false);
        meshRenderer.material = aliveMaterial;
        health = maxHealth;
    }
}
public enum State
{
    Null,
    RandomWalk,
    FindSafeArea,
    MoveToSafeArea,
    FindTarget,
    MoveToTarget,
    FindCollectable,
    MoveToCollectable,
    Dead,
}
public enum EntityType
{
    Healer,
    Vampire,
    Hunter
}