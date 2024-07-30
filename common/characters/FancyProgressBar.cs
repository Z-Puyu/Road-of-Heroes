using System;
using Game.common.characters;
using Godot;

namespace Game.UI.characters {
	[GlobalClass]
	public partial class FancyProgressBar : ProgressBar {
		private ProgressBar lessBar;
		private ProgressBar moreBar;
		private Timer timer;
		[Export] public Stat.Category DataType { set; get; }

		public void Init(Stat stat) {
			this.MaxValue = stat.MaxValue;
			this.Value = stat.Value;
			this.lessBar.MaxValue = stat.MaxValue;
			this.lessBar.Value = stat.Value;
			this.moreBar.MaxValue = stat.MaxValue;
			this.moreBar.Value = stat.Value;
		}

		public override void _Ready() {
			this.lessBar = this.GetNode<ProgressBar>("./LessBar");
			this.moreBar = this.GetNode<ProgressBar>("./MoreBar");
			this.timer = this.GetNode<Timer>("./Timer");
			this.timer.Timeout += this.OnTimerTimeout;
		}

		public void SetValue(Stat.Category type, int val, int maxVal) {
			if (type != this.DataType) {
				throw new ArgumentException($"Progress bar expects a value of type {this.DataType}, but received {type}.");
			}
			if (maxVal != this.MaxValue) {
				this.MaxValue = maxVal;
			} 
			if (val > this.Value) {
				// Increase value.
				this.moreBar.Value = val;
			} else if (val < this.Value) {
				// Decrease value.
				this.Value = val;
				this.moreBar.Value = val;
			}
			this.timer.Start();
		}

		private void OnTimerTimeout() {
			if (this.Value < this.moreBar.Value) {
				this.Value = this.moreBar.Value;
				this.lessBar.Value = this.moreBar.Value;
			} else {
				this.lessBar.Value = this.Value;
			}
		}
	}
}
