using Game.common.characters.skills;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.characters.profession {
    [RegisteredType(nameof(Profession), "", nameof(Resource)), GlobalClass]
    public partial class Profession : Resource {
        public enum Name {
            Warrior,
            Spellsword,
            Mercenary,
            Ranger,
            Archer,
            Assassin,
            Mage,
            Alchemist,
            Necromancer,
            Enchanter,
            Bard,
            Occultist
        }

        [Export] public Name ProfessionName { set; get; }
        [Export] public string Description { set; get; }
        [Export] public Texture2D Icon { set; get; }

        public override string ToString() {
            return this.ProfessionName.ToString();
        }
    }
}