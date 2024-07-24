using Godot;
using System;

namespace Game.common.player {
	public partial class Player : Sprite2D {
		public static PackedScene Scene => GD.Load<PackedScene>("res://assets/player/Player.tscn");
    }	
}
