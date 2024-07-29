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
        [Export] public Array<Skill> Skills { set; get; } = [];
        [ExportGroup("Racial Avatars")]
        [Export] public Array<Texture2D> Humans { set; get; } = [];  
        [Export] public Array<Texture2D> Elves { set; get; } = [];  
        [Export] public Array<Texture2D> Dwarves { set; get; } = [];  
        [Export] public Array<Texture2D> Orcs { set; get; } = [];  
    }
}