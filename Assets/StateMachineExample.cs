using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;
public abstract class StateMachineExample : MonoBehaviour
{
    public enum State
    {
        A,B
    }
    protected StateMachine<State> stateMachine;
    protected State state;
    protected virtual void Start()
    {
        //Initialise state machine
        stateMachine = new StateMachine<State>();
        //Add states
        //Definition of state METHOD 1: Default way of defining state using Lambda expressions
        //METHOD 1 Does not allow for sub classes to overwrite
        stateMachine.AddState(State.A, 
            onEnter: state => { Debug.Log($"OnEnter {state.name}"); }, //On state entered
            onLogic: state => { Debug.Log($"OnLogic {state.name}"); }, //Every frame
            onExit: state => { Debug.Log($"OnExit {state.name}"); } //On state exited
            );

        //Definition of state METHOD 2: Methods in object scope
        //METHOD 2 Allows for subclasses to overwrite state behaviour by overriding methods
        //StateB_OnEnter, StateB_OnLogic, StateB_OnExit
        stateMachine.AddState(State.B, StateB_OnEnter, StateB_OnLogic, StateB_OnExit);

        //Initializes the state machine
        stateMachine.Init();
    }
    protected virtual void StateB_OnEnter (StateBase<State> state)
    {
        Debug.Log($"OnEnter {state.name}");
    }
    protected virtual void StateB_OnLogic(StateBase<State> state)
    {
        Debug.Log($"OnLogic {state.name}");
    }
    protected virtual void StateB_OnExit(StateBase<State> state)
    {
        Debug.Log($"OnExit {state.name}");
    }
    protected virtual void Update()
    {
        //Runs the onLogic / update function
        stateMachine.OnLogic();
    }
}
