using Game.common.characters.@class;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.characters.skills {
    [RegisteredType(nameof(ClassWeight), "", nameof(Resource)), GlobalClass]
    public partial class ClassWeight : Resource {
        private Class Class { set; get; }
        private int Weight { set; get; }

        public override string ToString() {
            return $"{this.Class}: {this.Weight}";
        }
    }
}