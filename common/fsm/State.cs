using System;
using Game.util;
using Game.util.events;

using Godot;

namespace Game.common.fsm {
    [GlobalClass]
    public abstract partial class State : Node {
        public enum Type {
            GameBoardNormal,
            GameBoardPlayerMoving,
            AvatarIdle,
            AvatarClicked,
            AvatarDragging,
            AvatarReleased
        }

        private readonly Type type;
        [Export] protected StateMachine FSM { set; get; }

        public Type StateType => type;

        public State(Type type) {
            this.type = type;
        }

        public override async void _Ready() {
            await this.FSM.ToSignal(this.FSM, Node.SignalName.Ready);
        }

        public bool OwnedBy(StateMachine sm) {
            return sm == this.FSM;
        }

        public virtual void Enter() {
            this.ReconnectAll();
        }
        public virtual void Exit() {
            this.MuteAll();
        }  

        public virtual void OnInput(InputEvent @event) {}
        public virtual void Handle(EventArgs @event) {}
    }
}