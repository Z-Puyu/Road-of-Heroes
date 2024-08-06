using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.common.autoload;
using Game.common.characters;
using Game.common.characters.skills;
using Game.common.effects.eot;
using Game.ui.characters;
using Game.ui.characters.player;
using Game.util;
using Godot; 

namespace Game.ui.battle {
	public partial class CharacterPanel : HBoxContainer {
		private readonly Dictionary<Stat.Category, StatLabel> stats = [];
		private readonly Dictionary<EoT.Effect, ResistanceLabel> resistances = [];
		[Export] private NodePath StatsInfo { set; get; }
		[Export] private NodePath Resistance { set; get; }
		[Export] private NodePath Skills { set; get; }

        public override void _Ready() {
            foreach (Node child in this.GetNode<VBoxContainer>(this.StatsInfo).GetChildren()) {
				if (child is StatLabel stat) {
					this.stats.Add(stat.Stat, stat);
				}
			}
			foreach (Node child in this.GetNode<HBoxContainer>(this.Resistance).GetChildren()) {
				if (child is ResistanceLabel resist) {
					this.resistances.Add(resist.Effect, resist);
				}
			}
			this.Modulate = new Color(1, 1, 1, 0);
			this.Hide();
        }

        public async Task Display(PlayerCharacter character, Skill currActiveSkill, int position = -1) {
			foreach (KeyValuePair<EoT.Effect, ResistanceLabel> pair in this.resistances) {
				pair.Value.Set(character.Get((Stat.Category)pair.Key));
			}
			foreach (KeyValuePair<Stat.Category, StatLabel> pair in this.stats) {
				pair.Value.Set(character.Get(pair.Key));
			}
			GridContainer skills = this.GetNode<GridContainer>(this.Skills);
			for (int i = 0; i < skills.GetChildCount(); i += 1) {
				if (i >= character.ActiveSkills.Count) {
					skills.GetChild<BattleSkillButton>(i).Disabled = true;
				} else {
					BattleSkillButton button = skills.GetChild<BattleSkillButton>(i);
					button.Load(character.ActiveSkills[i], character);
					button.Disabled = !position.In(character.ActiveSkills[i].UserPosition);
					button.ButtonPressed = character.ActiveSkills[i] == currActiveSkill;
				}
			}
			if (!this.Visible) {
				this.Show();
				await AnimationManager.Animate(this, "modulate:a", 1, 0.25, Tween.EaseType.Out);
			}
		}

		public async Task Erase() {
			if (this.Visible) {
				await AnimationManager.Animate(this, "modulate:a", 0, 0.25, Tween.EaseType.Out);
				this.Hide();
			}
		}
	}
}
