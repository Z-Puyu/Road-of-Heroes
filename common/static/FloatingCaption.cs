using System;
using System.Threading.Tasks;
using Game.common.characters;
using Game.common.effects.eot;
using Godot;

namespace Game.util {
	[GlobalClass]
	public partial class FloatingCaption : Node {
		private Vector2 size;
		private Label caption;
		private static FloatingCaption node;

		[ExportGroup("Colours")]
		[ExportSubgroup("Damage Number")]
		[Export] private Color Damage { set; get; }
		[Export] private Color Heal { set; get; }
		[Export] private Color Miss { set; get; }
		[ExportSubgroup("EoTs")]
		[Export] private Color Bleed { set; get; }
		[Export] private Color Blight { set; get; }
		[Export] private Color Burn { set; get; }
		[Export] private Color Frenzy { set; get; }
		[Export] private Color Poison { set; get; }
		[Export] private Color Stun { set; get; }

		public static FloatingCaption Node => node;


        public override void _Ready() {
			FloatingCaption.node = this.GetNode<FloatingCaption>("/root/FloatingCaption");
			this.caption = this.GetNode<Label>("./Caption");
			this.size = this.caption.Scale;
			this.caption.ZIndex = 99;
			this.caption.Hide();
		}

		private async Task Animate(Vector2 pos, double tTranslate, double tScale, double tDelay) {
			this.caption.PivotOffset = this.caption.Size / 2;
			this.caption.GlobalPosition = pos - new Vector2(32, 32) - this.caption.PivotOffset / 2;
			this.caption.Show();
			Tween tween = this.GetTree().CreateTween().SetParallel(false);
			tween.Pause();
			tween.TweenProperty(
				this.caption, "position:y", this.caption.Position.Y - 128, tTranslate
			).SetEase(Tween.EaseType.Out);
			tween.TweenProperty(
				this.caption, "scale", Vector2.Zero, tScale
			).SetEase(Tween.EaseType.Out).SetDelay(tDelay);
			tween.Play();
			await tween.ToSignal(tween, Tween.SignalName.Finished);
			this.caption.Scale = this.size;
			this.caption.Hide();
		}

		public async Task Display(Vector2 pos, EoT.Effect eot, bool failed = false) {
			this.caption.Text = eot.ToString();
			if (failed) {
				this.caption.Text = $"{this.caption.Text} Resisted!";
			}
			this.caption.LabelSettings.FontColor = eot switch {
				EoT.Effect.Bleed => this.Bleed,
				EoT.Effect.Blight => this.Blight,
				EoT.Effect.Burn => this.Burn,
				EoT.Effect.Poison => this.Poison,
				EoT.Effect.Frenzy => this.Frenzy,
				EoT.Effect.Stun => this.Stun,
				_ => this.Miss
			};
			await this.Animate(pos, 0.25, 0.25, 0.5);
		}

		public async Task Display(
			Stat.Category stat = Stat.Category.Health, int value = 0, 
			Vector2 pos = default, bool crit = false, bool heal = false, 
			bool miss = false
		) {
			string tag = stat switch {
				Stat.Category.Health => "HP",
				Stat.Category.Magicka => "Magicka",
				Stat.Category.Sanity => "SAN",
				Stat.Category.Fatigue => "Fatigue",
				_ => ""
			};
			if (!miss) {
				this.caption.Text = $"{value} {tag}";
				this.caption.LabelSettings.FontColor = this.Damage;
				if (heal) {
					value = Math.Max(value, 0);
					this.caption.Text = $"+{value} {tag}";
					this.caption.LabelSettings.FontColor = this.Heal;
				}
				if (crit) {
					this.caption.Text = $"Critical! {this.caption.Text}";
				}
			} else {
				this.caption.Text = "Miss!";
				this.caption.LabelSettings.FontColor = this.Miss;
			}
			await this.Animate(pos, 0.25, 0.25, 0.5);
		}
	}
}

