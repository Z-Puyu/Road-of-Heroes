using Godot;

namespace Game.common.fsm.states {
	public partial class IdleState : State {
        public IdleState() : base(Type.AvatarIdle) {}

        public override void OnInput(InputEvent e) {
			if (e.IsActionPressed("left_click")) {
				this.FSM.TransitionTo(Type.AvatarClicked);
			}
		}
	}
}
