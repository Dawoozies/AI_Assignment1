using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;
public abstract class Entity : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected StateMachine<State> entityStateMachine;
    public State state;
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        entityStateMachine = new StateMachine<State>();

        entityStateMachine.AddState(State.Null);

        entityStateMachine.Init();
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
    FleeToSafeArea,
    MoveToOtherEntity
}