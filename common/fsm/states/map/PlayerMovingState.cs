using Game.common.map;
using Game.util;
using Game.util.events;
using Godot;
using System;

namespace Game.common.fsm.states {
	[GlobalClass]
	public partial class PlayerMovingState : State {
		public PlayerMovingState() : base(Type.GameBoardPlayerMoving) {}

        public override void Enter() {
            base.Enter();
			if (this.FSM.Root is GameBoard gb) {
				if (!gb.MovePlayer()) {
					this.FSM.TransitionTo(State.Type.GameBoardNormal);
				}
			}
        }

        public override void Handle(object sender, EventArgs @event) {
            if (@event is PlayerReachedDestinationEvent) {
				this.FSM.TransitionTo(State.Type.GameBoardNormal);
			}
        }
    }
}
