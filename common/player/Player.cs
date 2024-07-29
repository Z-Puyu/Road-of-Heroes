using Game.common.player.modules;
using Godot;

namespace Game.common.player {
	[GlobalClass]
	public partial class Player : Sprite2D {
		public static PackedScene Scene => GD.Load<PackedScene>("res://assets/player/Player.tscn");

        [Export] private readonly Party party = new Party();
        public Vector2[] Path { get; set; }
		[Export] private MovementModule MovementModule { set; get; }

        public override void _Ready() {
			this.party.Initialise();
        }

        public void Move() {
			this.MovementModule.Work();
		}
    }	
}
