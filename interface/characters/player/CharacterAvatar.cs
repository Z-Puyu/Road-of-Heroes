using Game.common.characters;
using Game.util;
using Game.util.events.party;
using Godot;

public partial class CharacterAvatar : TextureRect {
	private PlayerCharacter character;
	[Export] private NodePath Avatar { set; get; }
	private TextureRect portrait;

    public override void _Ready() {
		this.portrait = this.GetNode<TextureRect>(this.Avatar);
    }

    public void Set(PlayerCharacter character) {
		this.character = character;
		this.portrait.Texture = character.Avatar;
	}

    public override void _GuiInput(InputEvent @event) {
        if (@event.IsActionReleased("left_click") && this.character != null) {
			this.Publish(new DisplayCharacterDetailEvent(this.character));
		}
    }
}
