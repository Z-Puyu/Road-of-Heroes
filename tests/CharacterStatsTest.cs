using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.common.characters;
using Game.common.stats;
using Godot;

namespace Game.tests {
    public partial class CharacterStatsTest : Node {
        [Export] Character Character { get; set; }

        public override void _Ready() {
            this.Character.Log();
            Actor actor = new Actor(this.Character);
            this.AddChild(actor);
            actor.Update(StatType.Health, -5, 0, 0);
            this.Character.Log();
        }
    }
}