using Game.common.autoload;
using Game.common.characters;
using Game.common.characters.enemies;
using Game.util;
using Game.util.events.battle;
using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.common.battle {
	public partial class CharacterManager : Control {
		[Export] private NodePath PlayerParty { set; get; }
		[Export] private NodePath EnemyParty { set; get; }
		private readonly List<PlayerCard> playerCards = [];
		private readonly List<EnemyCard> enemyCards = [];

        public override void _Ready() {
			this.Subscribe<BattleStartEvent>(this.OnBattleStart);
			this.Subscribe<DisplaceCharacterEvent>(this.OnDisplaceCharacter);
        }

        private void OnDisplaceCharacter(object sender, EventArgs e) {
            if (e is DisplaceCharacterEvent @event) {
				this.Move(@event.Card, @event.StepSize);
			}
        }


        private void OnBattleStart(object sender, EventArgs e) {
			this.Init();
        }


        public void Init() {
			HBoxContainer playerParty = this.GetNode<HBoxContainer>(this.PlayerParty);
			foreach (PlayerCharacter character in PlayerManager.Combatants) {
				PlayerCard card = PlayerCard.Of(character);
				this.playerCards.Add(card);
				playerParty.AddChild(card);
			}
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
