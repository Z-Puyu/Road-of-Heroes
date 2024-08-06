using Game.common.autoload;
using Game.common.characters;
using Game.common.characters.enemies;
using Game.common.characters.skills;
using Game.ui.battle;
using Game.util;
using Game.util.events.battle;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.common.battle {
	public partial class CharacterManager : Control {
		[Export] private NodePath PlayerParty { set; get; }
		[Export] private NodePath EnemyParty { set; get; }
		[Export] private NodePath CharacterPanel { set; get; }
		private readonly List<PlayerCard> playerCards = [];
		private readonly List<EnemyCard> enemyCards = [];
		private PlayerCharacter active;
		private Skill skill;

        public override void _Ready() {
			this.Subscribe<DisplaceCharacterEvent>(this.OnDisplaceCharacter);
			this.Subscribe<SkillReadyEvent>(this.OnSkillReady);
			this.Subscribe<SkillUnequipedEvent>(this.OnUnequipSkill);
			this.Subscribe<SkillTargetSelectedEvent>(this.OnSelectSkillTarget);
        }

        private void OnSelectSkillTarget(object sender, EventArgs e) {
			Node node = this.GetParent();
			bool yes0 = node.IsInsideTree();
			bool yes = this.IsInsideTree();
			SceneTree tree = this.GetTree();
			bool b = this.GetTree().HasGroup("selected_targets");
			int a = 0;
			this.skill.Fire(
				this.playerCards.Find(x => x.Character == this.active), 
				[..this.GetTree().GetNodesInGroup("selected_targets").Cast<CharacterCard>()]
			);
        }


        private async void OnUnequipSkill(object sender, EventArgs e) {
			if (e is SkillUnequipedEvent @event && @event.Skill == this.skill) {
				List<Task> anims = [];
				foreach (PlayerCard card in this.playerCards) {
					anims.Add(card.Release());
				}
				foreach (EnemyCard card in this.enemyCards) {
					anims.Add(card.Release());
				}
				foreach (Task anim in anims) {
					await anim;
				}
				this.skill = null;
			}
        }


        private async void OnSkillReady(object sender, EventArgs e) {
            if (e is SkillReadyEvent @event) {
				PlayerCharacter src = @event.Character;
				Skill skill = @event.Skill;
				this.skill = skill;
				if (src != this.active) {
					return;
				}
				List<Task> anims = [];
				switch (skill.TargetRange) {
					case Skill.Range.AOEEnemy:
					case Skill.Range.SingleEnemy:
						for (int i = skill.TargetPosition.X; i < skill.TargetPosition.Y; i += 1) {
							anims.Add(this.enemyCards[i - 1].LockedOn());
						}
						break;
					case Skill.Range.AOEAlly:
					case Skill.Range.SingleAlly:
						for (int i = skill.TargetPosition.X; i < skill.TargetPosition.Y; i += 1) {
							anims.Add(this.playerCards[i - 1].LockedOn());
						}
						break;
					case Skill.Range.SelfOnly:
						anims.Add(this.playerCards.Find(x => x.Character == this.active).LockedOn());
						break;
					default:
						break;
				}
				foreach (Task anim in anims) {
					await anim;
				}
			}
        }

        private void OnDisplaceCharacter(object sender, EventArgs e) {
            if (e is DisplaceCharacterEvent @event) {
				this.Move(@event.Card, @event.StepSize);
			}
        }

        public void Init() {
			HBoxContainer playerParty = this.GetNode<HBoxContainer>(this.PlayerParty);
			foreach (PlayerCharacter character in PlayerManager.Combatants) {
				if (character == null) {
					continue;
				}
				PlayerCard card = PlayerCard.Of(character);
				this.playerCards.Add(card);
				playerParty.AddChild(card);
				card.MouseEntered += () => this.OnMouseEntered(card);
				card.MouseExited += () => this.OnMouseExited(card);
			}
			HBoxContainer enemyParty = this.GetNode<HBoxContainer>(this.EnemyParty);
			foreach (EnemyCharacter enemy in GameManager.RandomEnemies()) {
				EnemyCard card = EnemyCard.Of(enemy);
				this.enemyCards.Add(card);
				enemyParty.AddChild(card);
				card.MouseEntered += () => this.OnMouseEntered(card);
				card.MouseExited += () => this.OnMouseExited(card);
			}
		}

		public async Task SetActive(Character character) {
			if (this.active != null) {
				await this.playerCards.Find(x => x.Character == this.active).Deactivate();
			}
			if (character is PlayerCharacter c) {
				this.active = c;
				await this.playerCards.Find(x => x.Character == this.active).Activate();
				await this.GetNode<CharacterPanel>(this.CharacterPanel).Display(
					this.active, this.skill, this.playerCards.FindIndex(x => x.Character == this.active) + 1
				);
			} else {
				this.active = null;
				await this.GetNode<CharacterPanel>(this.CharacterPanel).Erase();
			}
		}

		private async void OnMouseEntered(PlayerCard card) {
			PlayerCharacter character = card.Character;
			if (character != this.active) {
				await this.GetNode<CharacterPanel>(this.CharacterPanel).Erase();
				await this.GetNode<CharacterPanel>(this.CharacterPanel).Display(character, this.skill);
			}
			card.Focus(this.skill != null && this.skill.TargetRange == Skill.Range.AOEAlly);
		}

		private async void OnMouseExited(PlayerCard card) {
			PlayerCharacter character = card.Character;
			if (this.active != character) {
				await this.GetNode<CharacterPanel>(this.CharacterPanel).Erase();
			}
			if (this.active != null) {
				await this.GetNode<CharacterPanel>(this.CharacterPanel).Display(
					this.active, this.skill, 
					this.playerCards.FindIndex(x => x.Character == this.active) + 1
				);
			}
			card.LoseFocus();
		}

		private void OnMouseEntered(EnemyCard card) {
			/* if (character != this.active) {
				await this.GetNode<CharacterPanel>(this.CharacterPanel).Erase();
				await this.GetNode<CharacterPanel>(this.CharacterPanel).Display(character);
			} */
			card.Focus(this.skill != null && this.skill.TargetRange == Skill.Range.AOEEnemy);
		}

		private void OnMouseExited(EnemyCard card) {
			/* if (this.active != character) {
				await this.GetNode<CharacterPanel>(this.CharacterPanel).Erase();
			}
			if (this.active != null) {
				await this.GetNode<CharacterPanel>(this.CharacterPanel).Display(this.active);
			} */
			card.LoseFocus();
		}

		public List<Character> GetAllCharacters() {
			IEnumerable<Character> heroes = this.playerCards.Select(card => card.Character);
			IEnumerable<Character> enemies = this.enemyCards.Select(card => card.Character);
			return [..heroes.Concat(enemies)];
		}

		private async void Move(CharacterCard card, int offset) {
			if (card is PlayerCard playerCard) {
				await this.Move(playerCard, this.playerCards.IndexOf(playerCard) + offset);
			}
		}

		public async Task Move(PlayerCard card, int position) {
			int currPos = this.playerCards.IndexOf(card);
			if (position == currPos) {
				return;
			}
			position = Math.Clamp(position, 0, this.playerCards.Count - 1);
			Dictionary<int, Vector2> positions = [];
			List<Task> anims = [];
			card.Reparent(this);
			if (position > currPos) {
				// Move back
				for (int i = currPos + 1; i <= position; i += 1) {
					positions.Add(i, this.playerCards[i - 1].GlobalPosition);
					this.playerCards[i].Reparent(this);
				}
				for (int i = currPos + 1; i <= position; i += 1) {
					anims.Add(AnimationManager.Animate(
						this.playerCards[i], "global_position", positions[i], 
						0.25, Tween.EaseType.InOut
					));
				}
			} else {
				// Move forward
				for (int i = currPos - 1; i >= 0; i -= 1) {
					positions.Add(i, this.playerCards[i + 1].GlobalPosition);
					this.playerCards[i].Reparent(this);
				}
				for (int i = currPos - 1; i >= 0; i -= 1) {
					anims.Add(AnimationManager.Animate(
						this.playerCards[i], "global_position", positions[i], 
						0.25, Tween.EaseType.InOut
					));
				}
			}
			anims.Add(AnimationManager.Animate(
				card, "global_position", position, 0.25, Tween.EaseType.InOut
			));
			foreach (Task anim in anims) {
				await anim;
			}
		}
	}
}
