using UnityEngine;
using System.Collections;

/// <summary>
/// Implement this to make a class a state
/// </summary>
public abstract class State
{
	public string Identifier { get; private set; }

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="identifier">A description or name of the state for referencing</param>
	public State (string identifier)
	{
		Identifier = identifier;
	}

	/// <summary>
	/// Use this function to do anything you would need to do in
	/// start for this state, and also to set a reference to the 
	/// FSM if you need.
	/// </summary>
	/// <param name="callingfsm"></param>
    public virtual void Initialize(MonoFSM callingfsm) { }

	/// <summary>
	/// Gets called when the machine transitions into this state
	/// </summary>
    public virtual void OnEnter() { }

	/// <summary>
	/// Gets called every Unity Update
	/// </summary>
    public virtual void OnUpdate() { }

	/// <summary>
	/// Get called every Unity FiedUpdate
	/// </summary>
    public virtual void OnFixedUpdate() { }

	/// <summary>
	/// Get called ever Unity LateUpdate
	/// </summary>
    public virtual void OnLateUpdate() { }

	/// <summary>
	/// Get called when an fsm leaves this state
	/// </summary>
    public virtual void OnExit() { }

	/// <summary>
	/// Gets called at the end of LateUpdate,
	/// this is where you put your if else statements for transtioning
	/// </summary>
    public virtual void CheckTransitions() { }
}
