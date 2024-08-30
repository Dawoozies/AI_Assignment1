using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;
public abstract class Entity : MonoBehaviour
{
    protected NavigationSystem navigationSystem;
    protected StateMachine<State> entityStateMachine;
    public State state;
    public ObjectLookUp.ObjectID collectableID;
    public WorldObject collectable;
    protected virtual void Start()
    {
        navigationSystem = GetComponent<NavigationSystem>();
        entityStateMachine = new StateMachine<State>();

        entityStateMachine.AddState(State.Null);
        entityStateMachine.AddState(State.FindCollectable,
            onLogic: state => {
                if (ObjectLookUp.ins.TryGetClosestObjectWithID(transform.position, collectableID, out collectable))
                {
                    entityStateMachine.RequestStateChange(State.MoveToCollectable);
                }
            });
        entityStateMachine.AddState(State.MoveToCollectable,
            onEnter: state => { navigationSystem.ChangeActiveNavigator(1); },
            onLogic: state => {
                navigationSystem.ChangePoint(collectable.transform.position);
            });
        entityStateMachine.Init();
        entityStateMachine.RequestStateChange(State.FindCollectable);
    }
    protected virtual void Update()
    {
        entityStateMachine.OnLogic();
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
    MoveToCollectable
}