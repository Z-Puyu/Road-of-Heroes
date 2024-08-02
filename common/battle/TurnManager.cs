using Game.common.battle;
using Game.common.characters;
using Game.common.characters.enemies;
using Game.util;
using Game.util.events.battle;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class TurnManager : Node {
	[Export] private NodePath CharacterManager { set; get; }
	[Export] private NodePath TurnLabel { set; get; }
	private List<Character> participants;
	private readonly Queue<Character> queue = [];
	private int nTurns = 0;

	public override async void _Ready() {
		CharacterManager characters = this.GetNode<CharacterManager>(this.CharacterManager);
		characters.Init();
		this.participants = characters.GetAllCharacters();
		await this.NewTurn();
	}

	private bool IsPlayerVictory() {
		return this.participants.All(x => x is PlayerCharacter);
	}

	private bool IsEnemyVictory() {
		return this.participants.All(x => x is EnemyCharacter);
	}

    private async Task NewTurn() {
		if (this.IsPlayerVictory()) {
			Console.WriteLine("Battle ended with player victory");
			this.Recycle();
			return;
		}
		if (this.IsEnemyVictory()) {
			Console.WriteLine("Battle ended with enemy victory");
			this.Recycle();
			return;
		}
		participants.Sort((x, y) => y.Get(Stat.Category.Speed) - x.Get(Stat.Category.Speed));
		foreach (Character participant in participants) {
			this.queue.Enqueue(participant);
		}
		this.nTurns += 1;
		this.GetNode<Label>(this.TurnLabel).Text = $"{this.nTurns}";
		await this.NextCharacter();
	}

	private async Task NextCharacter() {
		if (this.queue.Count == 0) {
			await this.NewTurn();
		} else {
			await this.GetNode<CharacterManager>(this.CharacterManager)
			          .SetActive(this.queue.Dequeue());
		}
	}
}
