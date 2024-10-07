using System;
using Game.common.autoload;
using Game.common.characters;
using Game.common.stats;
using Godot;

namespace Game.tests {
    public partial class CharacterModifiableValuesTest : Node {
        [Export] Character Character { get; set; }

        public override void _Ready() {
            this.Character = Hero.Random();
            Actor actor = Actor.Of(this.Character);
            this.AddChild(actor);
            actor.Update(ModifiableValueType.Health, -5, 0, 0);
            Console.WriteLine(actor.Get(ModifiableValueType.Health));
        }
    }
}