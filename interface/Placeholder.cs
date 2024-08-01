using System;
using Game.util;
using Godot;

namespace Game.ui {
	public partial class Placeholder<C> : Container where C : Control {
		private readonly static PackedScene scene = GD.Load<PackedScene>("res://assets/interface/Placeholder.tscn");
		private C ui;

		public static Placeholder<C> Of(C ui) {
			Placeholder<C> placeholder = Placeholder<C>.scene.Instantiate<Placeholder<C>>();
			placeholder.ui = ui;
			foreach (Node node in ui.GetSubTree()) {
				if (node is Control control) {
					control.MouseFilter = MouseFilterEnum.Ignore;
				}
			}
			return placeholder;
		}

		public C UI => ui;

		public override void _Ready() {
			this.AddChild(this.ui);
			this.ZIndex = 99;
			this.GlobalPosition = this.GetGlobalMousePosition() - this.ui.Size / 2;
		}

        /* public override void _Process(double delta) {
            this.GlobalPosition = this.GetGlobalMousePosition() - this.ui.Size / 2;
        } */
    }
}
