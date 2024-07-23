using System;
// using Game.util.errors;
using Godot;

namespace Game.util.fsm {
    [GlobalClass]
    public abstract partial class State : Node {
        public enum Type {
            HeroCardNormal,
            HeroCardClicked,
            HeroCardDragging,
            HeroCardReleased,
            SkillNormal,
            SkillReady,
            SkillDisabled,
            CharacterIdle,
            CharacterActive,
            CharacterMoving,
            CharacterInactive    
        }

        private readonly Type type;
        protected StateMachine fsm;

        public Type StateType => type;

        public StateMachine Fsm { 
            set {
                /* if (value != null && value != this.GetParent<StateMachine>()) {
                    throw new NoSuchParentException(
                        $"{value} is not the StateMachine owning this {this.type} State."
                    );
                } */
                fsm = value; 
            } 
        }

        public State(Type type) {
            this.type = type;
        }

        public bool OwnedBy(StateMachine sm) {
            return sm == this.fsm;
        }
        public virtual void Enter() {}
        public virtual void Exit() {
            this.UnsubscribeAllEvents();
        }  
        public virtual void OnInput(InputEvent e) {}
        public virtual void Handle(object sender, EventArgs @event) {}
    }
}