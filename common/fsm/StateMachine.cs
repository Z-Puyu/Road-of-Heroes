using System;
using System.Collections.Generic;
using Godot;

namespace Game.util.fsm {
	[GlobalClass]
	public partial class StateMachine : Node {
		[Export] public NodePath InitialState { set; get; }

		private readonly Dictionary<State.Type, State> states = [];
		private State currState;
		private Node parent;

		public Node Parent { get => parent; set => parent = value; }

		public override async void _Ready() {
			await this.ToSignal(this.Parent, Node.SignalName.Ready);
			foreach (Node node in this.GetChildren()) {
				if (node is State s) {
					this.states[s.StateType] = s;
					s.Fsm = this;
					s.Exit(); // Reset the state.
				}
			}
			this.currState = this.GetNode<State>(this.InitialState);
			this.currState.Enter(); // Activate the initial state.
		}

		/// <summary>
		/// Checks if the state of type <paramref name="t"/> is a valid next state for this FSM.
		/// </summary>
		/// <param name="t"> The type of the state to be checked. </param>
		/// <param name="s"> The next state to transition to. </param>
		/// <returns> <c>true</c> if the state type <paramref name="t"/> exists in this FSM 
		/// and is different from the type of the current state. </returns>
		private bool IsValidState(State.Type t, out State s) {
			return this.states.TryGetValue(t, out s) && s != this.currState;
		}

		public void TransitionTo(State.Type t) {
			if (this.IsValidState(t, out State s)) {
				this.currState.Exit();
				this.currState = s;
				this.currState.Enter();
			}
		}

		public void OnInput(InputEvent e) {
			this.currState.OnInput(e);
		}

		public void Handle(object sender, EventArgs e) {
			this.currState.Handle(sender, e);
		}
	}
}
