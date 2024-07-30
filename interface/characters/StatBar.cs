using Game.common.characters;
using Game.UI.characters;
using Godot;

public partial class StatBar : HBoxContainer {
	private Stat.Category statType;
	[Export] private NodePath Bar { set; get; }
	[Export] private NodePath ValueLabel { set; get; }
    public Stat.Category StatType => statType;


    public override void _Ready() {
        this.statType = this.GetNode<FancyProgressBar>(this.Bar).DataType;
    }

    public void Init(Stat stat) {
		this.GetNode<FancyProgressBar>(this.Bar).Init(stat);
		this.GetNode<Label>(this.ValueLabel).Text = $"{stat.Value}/{stat.MaxValue}";
	}
}
