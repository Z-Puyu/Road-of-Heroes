using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.characters.classes {
    [RegisteredType(nameof(Class), "", nameof(Resource)), GlobalClass]
    public partial class Class : Resource {
        public enum Name {
            Warrior,
            Spellsword,
            Ranger,
            Archer,
            Assassin,
            Mage,
            Alchemist,
            Necromancer,
            Healer,
            Bard,
            Occultist
        }

        [Export] public Name ClassName { set; get; }
        [Export] public string Description { set; get; }
        [Export] public Texture2D Icon { set; get; }

        public override string ToString() {
            return this.ClassName.ToString();
        }
    }
}