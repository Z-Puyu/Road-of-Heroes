using Game.common.characters.skills;
using Godot;
using Godot.Collections;

namespace Game.common.characters.profession {
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
            Healer,
            Bard,
            Occultist
        }

        [Export] public Name ProfessionName { set; get; }
        [Export] public string Description { set; get; }
        [Export] public Texture2D Icon { set; get; }
        [Export] public Array<Skill> Skills { set; get; } = [];     
    }
}