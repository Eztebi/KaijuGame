using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState: Enum
{
    protected Dictionary<EState, BaseState<EState>> states = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> currentState;
    protected bool IsTransitioningState = false;
    private void Awake(){
        currentState.AwakeState();
    }
    private void Start(){
        currentState.EnterState();
    }
    private void Update(){
        EState nextStateKey = currentState.GetNextState();

        if (nextStateKey.Equals(currentState.StateKey))
        {
            currentState.UpdateState();
        }
        else {
            TransitionToState(nextStateKey);
        }
    }
    public void TransitionToState(EState stateKey)
    {
        IsTransitioningState = true;
        currentState.ExitState();
        currentState = states[stateKey];
        currentState.EnterState();
        IsTransitioningState=false;
    }
    private void OnTriggerEnter(Collider other){
        currentState.OnTriggerEnter(other);
    }
    private void OnTriggerStay(Collider other){
        currentState.OnTriggerStay(other);
    }
    private void OnTriggerExit(Collider other){
        currentState.OnTriggerExit(other);
    }
    private void OnCollisionEnter(Collision collision){
        currentState.OnCollisionEnter(collision);
    }
    private void OnCollisionStay(Collision collision){
        currentState.OnCollisionStay(collision);
    }
    private void OnCollisionExit(Collision collision){
        currentState.OnCollisionExit(collision);
    }
}
