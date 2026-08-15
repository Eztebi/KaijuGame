using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState: Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();
    protected bool IsTransitioningState = false;
    public BaseState<EState> currentState { get; protected set; }

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
        currentState = States[stateKey];
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
    public void OnCollisionEnter(Collision collision){
        currentState.OnCollisionEnter(collision);
    }
    public void OnCollisionStay(Collision collision){
        currentState.OnCollisionStay(collision);
    }
    public void OnCollisionExit(Collision collision){
        currentState.OnCollisionExit(collision);
    }
}
