using System;
using System.Threading.Tasks;
using Game.common.autoload;
using Game.common.characters;
using Game.common.effects;
using Godot;

namespace Game.common.characters {
    public abstract partial class CharacterCard : TextureButton, IEffectEmitter, IEffectReceiver {
        protected Character character;
        [Export] protected NodePath Bars { set; get; }
        [Export] protected NodePath Avatar { set; get; }
        [Export] protected NodePath NameLabel { set; get; }
        [Export] public NodePath EoTManager { set; get;}
        [Export] public NodePath Reticule { set; get; }
        [Export] public NodePath Line { set; get; }

        public Character Character => this.character;

        public override void _Ready() {
            this.GetNode<TextureRect>(this.Avatar).Texture = this.character.Avatar;
            this.GetNode<Label>(this.NameLabel).Text = this.character.Name;
        }

        public int Emit(Effect.Type effect, int value) {
            return Math.Max(0, this.character.Modifier.ModifyOnEmit(effect, value));
        }

        public int Receive(Effect.Type effect, int value) {
            return Math.Max(0, this.character.Modifier.ModifyOnReceive(effect, value));
        }

        public async Task Activate() {
            await AnimationManager.Animate(
                this.GetNode<Line2D>(this.Line), "modulate:a", 1, 0.25, Tween.EaseType.Out
            );
        }

        public async Task Deactivate() {
            await AnimationManager.Animate(
                this.GetNode<Line2D>(this.Line), "modulate:a", 0, 0.25, Tween.EaseType.Out
            );
        }

        public async Task LockedOn() {
            this.Disabled = false;
            await AnimationManager.Animate(
                this.GetNode<TextureRect>(this.Reticule), "modulate:a", 1, 0.25, Tween.EaseType.Out
            );
        }

        public async Task Release() {
            this.Disabled = true;
            if (this.Modulate.A != 0) {
                await AnimationManager.Animate(
                    this.GetNode<TextureRect>(this.Reticule), "modulate:a", 0, 0.25, Tween.EaseType.Out
                );
            }
        }
    }
}
