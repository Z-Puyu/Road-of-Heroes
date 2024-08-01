using Game.common.autoload;
using Game.common.characters;
using Game.common.fsm;
using Godot;

public partial class CharacterAvatar : TextureRect {
	private readonly static PackedScene scene = GD.Load<PackedScene>("res://assets/interface/CharacterAvatar.tscn");
	private PlayerCharacter character;
	[Export] private NodePath Avatar { set; get; }
	[Export] private NodePath FSMPath { set; get; }
    private TextureRect portrait;
	private StateMachine fsm;
	private int index;

	public PlayerCharacter Character => character;

    public override async void _Ready() {
		this.portrait = this.GetNode<TextureRect>(this.Avatar);
		this.fsm = this.GetNode<StateMachine>(this.FSMPath);
		Node parent = this.GetParent();
		await this.ToSignal(parent, Node.SignalName.Ready);
		this.index = parent.GetChildren().IndexOf(this);
    }

    public void Set(PlayerCharacter character) {
		this.character = character;
		this.portrait.Texture = character.Avatar;
		this.portrait.Show();
	}

	public CharacterAvatar Duplicate() {
		CharacterAvatar avatar = CharacterAvatar.scene.Instantiate<CharacterAvatar>();
		avatar.character = this.character;
		avatar.GetNode<TextureRect>(avatar.Avatar).Texture = this.portrait.Texture;
		return avatar;
	}

	public static CharacterAvatar Blank() {
		CharacterAvatar avatar = CharacterAvatar.scene.Instantiate<CharacterAvatar>();
		return avatar;
	}

    public override bool _CanDropData(Vector2 atPosition, Variant data) {
		return data.As<CharacterAvatar>() != null;
    }

    public override void _DropData(Vector2 atPosition, Variant data) {
		CharacterAvatar src = data.As<CharacterAvatar>();
		PlayerCharacter thisCharacter = this.character;
		PlayerCharacter otherCharacter = src.character;
		if (!this.portrait.Visible) {
			this.Set(otherCharacter);
			src.character = null;
			src.portrait.Texture = null;
		} else {
			this.Set(otherCharacter);
			src.Set(thisCharacter);
		}
		PlayerManager.Combatants[src.index] = src.character;
		PlayerManager.Combatants[this.index] = this.character;
    }

    public override Variant _GetDragData(Vector2 atPosition) {
		if (this.character == null) {
			return Variant.From<GodotObject>(null);
		}
		this.SetDragPreview(this.Duplicate());
		this.portrait.Hide();
		return this;
    }

    public override void _Notification(int what) {
        if (what == Node.NotificationDragEnd && !this.GetViewport().GuiIsDragSuccessful()) {
			this.portrait.Show();
		}
    }

    public override void _GuiInput(InputEvent @event) {
        if (this.character != null) {			
			this.fsm.OnInput(@event);
		}
    }
}
