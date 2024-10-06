using Game.common.characters.classes;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.characters.skills {
    [RegisteredType(nameof(ClassWeight), "", nameof(Resource)), GlobalClass]
    public partial class ClassWeight : Resource {
        [Export] private Class Class { set; get; }
        [Export] private int Weight { set; get; }

        public override string ToString() {
            return $"{this.Class}: {this.Weight}";
        }
    }
}