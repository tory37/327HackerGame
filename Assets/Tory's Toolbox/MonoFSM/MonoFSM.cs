using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Implement this to have a functioning finite state machine that is also a monobehaviour.
/// This class, when implemented, provides a Dictionary of Enum to class State, and calls 
/// the update methods of the states in their corresponding Unity functions.
/// This also provides a method for transitioning between states correctly.
/// </summary>
public abstract class MonoFSM : MonoBehaviour {

	/// <summary>
	/// This is a list of states in the fsm
	/// Each state can easily be referenced by their corresponding enum value.
	/// Best Practice:
	///		Declare the enum within the state machine
	/// </summary>
	public Dictionary<Enum, State> states;

	public List<FSMTransition> validTransitions;

	/// <summary>
	/// This represents what state the machine is currently in
	/// </summary>
	State currentState = null;

	/// <summary>
	/// Calls SetStates, SetTransitions, Initialize, then calls initialize on each state in the fsm. 
	/// </summary>
	protected virtual void Start()
	{
		SetStates();

		SetTransitions();

		Initialize();

		foreach ( State state in states.Values )
		{
			state.Initialize(this);
		}

		currentState = states.Values.ElementAt( 0 );
		currentState.OnEnter();
	}

	/// <summary>
	/// Initialize the dictionary of Enum to States in this function.
	/// This gets called first before the initilazation of the machine itself
	/// </summary>
	protected abstract void SetStates();

	/// <summary>
	/// Use this function to initilize the dictionary of State to State
	/// that provides a list of valid functions
	/// </summary>
	protected abstract void SetTransitions();

	/// <summary>
	/// This is used for anything that would need to get done at the start of 
	/// this machine
	/// </summary>
	protected virtual void Initialize() { }

	/// <summary>
	/// Gets called once per frame
	/// </summary>
	protected void Update()
	{
		currentState.OnUpdate();
	} 

	/// <summary>
	/// Gets called once per frame on a fixed timescale
	/// </summary>
	protected void FixedUpdate()
	{
		currentState.OnFixedUpdate();
	} 

	/// <summary>
	/// Gets called at the end of every frame
	/// </summary>
	protected void LateUpdate()
	{
		currentState.OnLateUpdate();
		currentState.CheckTransitions();
	}

	/// <summary>
	/// Transitions the FSM to the passed state.
	/// </summary>
	/// <param name="toState"></param>
	protected void Transition(Enum toState)
	{
		currentState.OnExit();
		if ( states.TryGetValue( toState, out currentState ) )
		{
			currentState.OnEnter();
		}
		else
			throw new Exception( "State " + toState + " is not defined, check if your string is correct." );
	}

	/// <summary>
	/// This checks to see if the  
	/// </summary>
	/// <param name="from"></param>
	/// <param name="to"></param>
	/// <returns></returns>
	public bool ContainsTransition(State from, State to)
	{
		return validTransitions.Contains(new FSMTransition(from, to));
	}

	/// <summary>
	/// Use this function as a proxy to do any checks for outside sources 
	/// to makes a transition in the FSM.
	/// </summary>
	public virtual void AttemptTransition(Enum to)
	{
		if ( states.ContainsKey( to ) && ContainsTransition( currentState, states[to] ) )
			Transition( to );
		else
			Debug.Log( "FSM does not cannot transition to state " + to.ToString() + " from current state: " + currentState.Identifier );
	}

}

/// <summary>
/// Provides a way to keep track of valid transtions int he FSM
/// </summary>
public class FSMTransition : IEquatable<FSMTransition>
{
	public State From { get; private set; }
	public State To { get; private set; }

	public FSMTransition(State from, State to)
	{
		this.From = from;
		this.To = to;
	}

	public bool Equals( FSMTransition other )
	{
		return this.From == other.From && this.To == other.To;
	}

	public static bool operator ==(FSMTransition left, FSMTransition right)
	{
		return left.Equals( right );
	}

	public static bool operator !=(FSMTransition left, FSMTransition right)
	{
		return !left.Equals( right );
	}
}
