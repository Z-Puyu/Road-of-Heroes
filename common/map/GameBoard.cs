using Game.common.autoload;
using Game.common.player;
using Game.util;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Game.common.map {
	public partial class GameBoard : TileMap {
		private enum Layer { Base, Locations }
		private readonly static List<Vector2I> DIRECTIONS = [
			Vector2I.Up, Vector2I.Down, Vector2I.Left, Vector2I.Right
		];

		[Export] Vector2I NavigableCell { set; get; } = Vector2I.Zero;
		[Export] Vector2I SnapThreshold { set; get; } = new Vector2I(5, 3);
		private int size = 100;
		private readonly HashSet<Vector2I> cells = [];

		public override void _Ready() {
			this.ClearLayer((int)Layer.Base);
			this.Generate(this.size, Vector2I.Zero);
			Vector2I start = this.cells.ElementAt(Utilities.Randi(0, this.size - 1));
			Vector2 globalPos = this.ToGlobal(this.MapToLocal(start));
			Player player = GameManager.Instantiate<Player>(Player.Scene, globalPos, this);
			this.Translate(this.Position - this.MapToLocal(start));
			this.Reveal(start, 3);
			Console.WriteLine($"{this.ToGlobal(this.MapToLocal(start))}, {player.GlobalPosition})");
		}

		private bool IsValidCell(Vector2I pos) {
			Vector2I left = pos + Vector2I.Left;
			Vector2I topLeft = pos + Vector2I.Left + Vector2I.Up;
			Vector2I right = pos + Vector2I.Right;
			Vector2I topRight = pos + Vector2I.Up + Vector2I.Right;
			Vector2I top = pos + Vector2I.Up;
			Vector2I bottomRight = pos + Vector2I.Right + Vector2I.Down;
			Vector2I bottom = pos + Vector2I.Down;
			Vector2I bottomLeft = pos + Vector2I.Down + Vector2I.Left;
			if (this.cells.Contains(topLeft) && this.cells.Contains(top) 
				&& this.cells.Contains(left)) {
				return false;
			}
			if (this.cells.Contains(topRight) && this.cells.Contains(top) 
				&& this.cells.Contains(right)) {
				return false;		
			}
			if (this.cells.Contains(bottomRight) && this.cells.Contains(bottom) 
				&& this.cells.Contains(right)) {
				return false;
			}
			if (this.cells.Contains(bottomLeft) && this.cells.Contains(bottom) 
				&& this.cells.Contains(left)) {
				return false;
			}
			return true;
		}

		public bool Generate(int size, Vector2I src, int maxIter = 10000) {
			if (size == 0 || maxIter == 0) {
				return size == 0;
			}
			this.cells.Add(src);
			List<Vector2I> dirs = [];
			foreach (Vector2I d in GameBoard.DIRECTIONS)	{
				if (this.IsValidCell(src + d)) {
					dirs.Add(d);
				}
			}
			if (dirs.Count == 0) {
				return false;
			}
			while (dirs.Count > 0) {
				int idx = Utilities.Randi(0, dirs.Count - 1);
				Vector2I dir = dirs[idx];
				Vector2I next = src + dir;
				if (this.cells.Contains(next)) {
					if (this.Generate(size, next, maxIter - 1)) {
						return true;
					}
				} else if (this.Generate(size - 1, next, maxIter - 1)) {
					return true;
				}
				dirs.RemoveAt(idx);
			}
			return false;
		}

		public void Reveal(Vector2I centre, int radius) {
			HashSet<Vector2I> visited = new HashSet<Vector2I>();
			Queue<(Vector2I, int)> queue = new Queue<(Vector2I, int)>();
			queue.Enqueue((centre, 0));
			while (queue.Count > 0) {
				(Vector2I, int) next = queue.Dequeue();
				Vector2I cell = next.Item1;
				int d = next.Item2;
				this.SetCell((int)Layer.Base, cell, 0, this.NavigableCell);
				visited.Add(cell);
				if (d < radius) {
					foreach (Vector2I dir in GameBoard.DIRECTIONS) {
						Vector2I pos = cell + dir;
						if ((!visited.Contains(pos)) && this.cells.Contains(pos)) {
							queue.Enqueue((pos, d + 1));
						}
					}
				}	
			}
		}
	}
}
