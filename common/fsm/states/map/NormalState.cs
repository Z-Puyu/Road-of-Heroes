using Game.common.map;
using Godot;

namespace Game.common.fsm.states {
	public partial class NormalState : State {
		private bool ready = false;

		public NormalState() : base(Type.GameBoardNormal) {}

        public override async void Enter() {
            base.Enter();
			if (this.FSM.Root is GameBoard gb) {
				gb.ClearLayer((int)GameBoard.Layer.UI);
				await gb.SnapMap();
				this.ready = true;
			}
        }

		public override void Exit() {
			base.Exit();
			this.ready = false;
		}

        public override void OnInput(InputEvent @event) {
			if (this.FSM.Root is not GameBoard gb || !this.ready) {
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
