using System;
using Game.common.characters;
using Game.common.effects;
using Game.common.tokens;
using Game.UI.characters;
using Godot;

public abstract partial class CharacterCard : TextureButton, IEffectEmitter, IEffectReceiver {
	private Character character;
    [Export] protected NodePath Bars { set; get; }
	[Export] protected NodePath Avatar { set; get; }
	[Export] protected NodePath NameLabel { set; get; }
	[Export] public NodePath EoTManager { set; get;}

	public Character Character => character;

    public int Emit(Effect.Type effect, int value) {
		return Math.Max(0, this.character.Modifier.ModifyOnEmit(effect, value));
    }

    public int Receive(Effect.Type effect, int value) {
        return Math.Max(0, this.character.Modifier.ModifyOnReceive(effect, value));
    }
}
