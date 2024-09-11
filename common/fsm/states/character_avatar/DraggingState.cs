using System;
using Game.common.autoload;
using Game.common.characters;
using Game.ui;
using Game.util;
using Game.util.events.party;
using Godot;

namespace Game.common.fsm.states {
	public partial class DraggingState : State {
		private Control control;
		private PlayerCharacter character;
		private CharacterAvatar target;
		private CharacterAvatar placeholder;
		[Export] private CanvasLayer TempLayer { set; get; }

        public DraggingState() : base(Type.AvatarDragging) {}

        public override void Enter() {
            if (this.FSM.Root is CharacterAvatar avatar) {
				this.control = avatar.GetParent<Control>();
				int idx = this.control.GetChildren().IndexOf(avatar);
				this.placeholder = CharacterAvatar.Blank();
				this.control.RemoveChild(avatar);
				this.control.AddChild(this.placeholder);
				this.control.MoveChild(this.placeholder, idx);
				GameManager.Instance.TempLayer.AddChild(avatar);
				avatar.Modulate = new Color(1, 1, 1, 0.5f);
			}
        }

        public override void Handle(object sender, EventArgs @event) {
			if (sender == this.placeholder) {
				return;
			}
            if (@event is HoverOverAvatarEvent) {
				this.target = (CharacterAvatar)sender;
			} else if (@event is LeftAvatarEvent) {
				this.target = null;
			}
        }

        public override void OnInput(InputEvent e) {
			
		}

		public override void _Process(double delta) {
			
        }
	}
}
