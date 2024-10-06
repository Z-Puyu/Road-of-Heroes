using System;
using Game.common.autoload;
using Game.common.characters;
using Game.common.stats;
using Godot;

namespace Game.tests {
    public partial class CharacterStatsTest : Node {
        [Export] Character Character { get; set; }

        public override void _Ready() {
            this.Character = Hero.Random();
            this.Character.Log();
            Actor actor = Actor.Of(this.Character);
            this.AddChild(actor);
            actor.Update(StatType.Health, -5, 0, 0);
            this.Character.Log();
            Console.WriteLine(actor.Get(StatType.Health));
        }
    }
}