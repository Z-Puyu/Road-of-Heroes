using System;
using System.Threading.Tasks;
using Game.common.autoload;
using Game.common.effects;
using Game.util;
using Game.util.events.battle;
using Godot;

namespace Game.common.characters {
    public abstract partial class CharacterCard : Panel, IEffectEmitter, IEffectReceiver {
        protected Character character;
        [Export] protected NodePath Bars { set; get; }
        [Export] protected NodePath Avatar { set; get; }
        [Export] protected NodePath NameLabel { set; get; }
        [Export] public NodePath EoTManager { set; get;}
        [Export] public NodePath Reticule { set; get; }
        [Export] public NodePath Line { set; get; }
        [ExportGroup("Colours")]
        [Export] private Color ModulationOnHover { set; get; } = new Color(1, 1, 1);
        [Export] private Color ModulationOnPress { set; get; } = new Color(1, 1, 1);
        private Color modulation;

        public Character Character => this.character;

        public override void _Ready() {
            this.modulation = this.Modulate;
            this.GetNode<TextureRect>(this.Avatar).Texture = this.character.Avatar;
            this.GetNode<Label>(this.NameLabel).Text = this.character.Name;
        }

        public async void Focus(bool aoe = false) {
            if (this.IsInGroup("potential_targets")) {
                if (aoe) {
                    foreach (Node node in this.GetTree().GetNodesInGroup("potential_targets")) {
                        if (node is CharacterCard card) {
                            card.Focus();
                        }
                    }
                } else {
                    await AnimationManager.Animate(
                        this, "modulate", this.ModulationOnHover, 0.25, Tween.EaseType.Out
                    );
                }
            }
        }

        public async void LoseFocus() {
            if (this.IsInGroup("potential_targets")) {
                foreach (Node node in this.GetTree().GetNodesInGroup("potential_targets")) {
                    if (node is CharacterCard card) {
                        await AnimationManager.Animate(
                            this, "modulate", this.modulation, 0.25, Tween.EaseType.Out
                        );
                    }
                }
            }
        }

        public void Press(bool aoe = false) {
            if (this.IsInGroup("potential_targets")) {
                if (aoe) {
                    foreach (Node node in this.GetTree().GetNodesInGroup("potential_targets")) {
                        if (node is CharacterCard card) {
                            card.Press();
                        }
                    }
                } else {
                    this.Modulate = this.ModulationOnPress;
                    this.AddToGroup("selected_targets");
                }
            }            
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
            this.AddToGroup("potential_targets");
            await AnimationManager.Animate(
                this.GetNode<TextureRect>(this.Reticule), "modulate:a", 1, 0.25, Tween.EaseType.Out
            );
        }

        public async Task Release() {
            this.RemoveFromGroup("potential_targets");
            if (this.Modulate.A != 0) {
                await AnimationManager.Animate(
                    this.GetNode<TextureRect>(this.Reticule), "modulate:a", 0, 0.25, Tween.EaseType.Out
                );
            }
        }

        public override void _GuiInput(InputEvent @event) {
            if (@event.IsActionPressed("left_click")) {
                if (this.GetTree().HasGroup("selected_targets")) {
                    foreach (Node node in this.GetTree().GetNodesInGroup("selected_targets")) {
                        node.RemoveFromGroup("selected_targets");
                    }
                }
                this.Press();
            } else if (@event.IsActionReleased("left_click")) {
                this.Modulate = this.modulation;
                this.Publish(new SkillTargetSelectedEvent());
            }
        }
    }
}
