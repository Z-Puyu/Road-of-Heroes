using Game.common.modifier;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.characters.enemies {
    [RegisteredType(nameof(Species), "", nameof(Resource)), GlobalClass]
    public partial class Species : Resource {
        private enum Type {
            Humanoid, Undead, Spirit, Beast
        }        

        [Export] private Type Name { set; get; }
        [Export] public Array<EffectModifier> Abilities { set; get; } = [];

        public Species() {}

        public override string ToString() {
            return this.Name.ToString();
        }
    }
}