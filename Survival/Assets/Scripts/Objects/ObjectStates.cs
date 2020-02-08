using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[DisallowMultipleComponent]
public class ObjectStates : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private IState[] states;
    public StateMachine stateMachine;

    private float timer;
#pragma warning restore 0649


    // Start is called before the first frame update
    void Start()
    {
        states = new IState[]
        {
            new ItemDefaultState(),
            new ItemTakingState()
        };
        stateMachine = new StateMachine(states);
        stateMachine.ChangeStateBrute(new ItemDefaultState());
        stateMachine.RequestCurrent(this);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 3f && stateMachine.IsCurrentState(new ItemTakingState()))
        {
            Debug.Log("state");
            timer = 0.0f;
            stateMachine.ChangeState(new ItemTakingState());
            stateMachine.RequestCurrent(this);
        }
    }
}







public class StateMachine
{
#pragma warning disable 0649
    private IState[] states;
    private IState currentState;
#pragma warning disable 0649

    public StateMachine(IState[] states)
    {
        this.states = states;
    }

    public IState GetCurrentState()
    {
        return this.currentState;
    }

    public bool IsCurrentState(IState state)
    {
        return ReferenceEquals(this.currentState, state);
    }

    public bool ChangeStateBrute(IState state)
    {
        if (state is null)
            return false;

        this.currentState = states.FirstOrDefault((item) => item == state);

        if (currentState is null)
            return false;

        return true;
    }

    public void RequestCurrent(ObjectStates instance)
    {
        this.currentState.Handle(instance);
    }

    public bool ChangeState(IState state)
    {
        if (state is null)
            return false;

        this.currentState = currentState.NextStates.FirstOrDefault(item => ReferenceEquals(item, state));

        if (currentState is null)
            return false;

        return true;
    }
}

public interface IState
{
    void Handle(ObjectStates context);
    IState[] NextStates { get; set; }
}

[System.Serializable]
class ItemTakingState : IState
{
    public IState[] NextStates { get; set; }

    public ItemTakingState() // set state transitions
    {
        NextStates = new IState[]
        {
            new ItemDefaultState()
        };
    }

    public void Handle(ObjectStates context)
    {
        //context.gameObject.SetActive(false);
    }
}

[System.Serializable]
class ItemDefaultState : IState
{
    public IState[] NextStates { get; set; }

    public ItemDefaultState()
    {
        NextStates = new IState[]
        {
            new ItemTakingState()
        };
    }

    public void Handle(ObjectStates context)
    {
        //context.gameObject.SetActive(true);
    }
}