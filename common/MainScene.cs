using Game.common.autoload;
using Game.util;
using Godot;

namespace Game.common {
	public partial class MainScene : Sprite2D {
		[Export] private NodePath World { set; get; }
		[Export] private PackedScene BattleView { get; set; }
		private Node2D battleView;

        public override void _UnhandledInput(InputEvent @event) {
            if (@event.IsActionReleased("space")) {
				if (GameManager.World != null) {
					this.battleView.Recycle();
					this.AddChild(GameManager.World);
					GameManager.World = null;
				} else {
					GameManager.World = this.GetNode<Node2D>(this.World);
					this.RemoveChild(GameManager.World);
					this.battleView = this.BattleView.Instantiate<Node2D>();
					this.AddChild(this.battleView);
				}
			}
        }
    }
}
