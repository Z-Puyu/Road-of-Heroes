using Game.common.map;
using Godot;

namespace Game.common.fsm.states {
	public partial class NormalState : State {
		public NormalState() : base(Type.GameBoardNormal) {}

        public override void Enter() {
            base.Enter();
			if (this.FSM.Root is GameBoard gb) {
				gb.ClearLayer((int)GameBoard.Layer.UI);
			}
        }

        public override void OnInput(InputEvent @event) {
			if (this.FSM.Root is not GameBoard gb) {
				return;
			}
            if (@event is InputEventMouseMotion) {
				gb.FindPath();
			} else if (@event.IsActionReleased("left_click")) {
				this.FSM.TransitionTo(State.Type.GameBoardPlayerMoving);
			}
        }
    }
}
