using Game.common.autoload;
using Game.common.fsm;
using Game.common.player;
using Game.util;
using Game.util.events;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.common.map {
	[GlobalClass]
	public partial class GameBoard : TileMap {
		public enum Layer { Base, Locations, UI }
		private readonly static List<Vector2I> DIRECTIONS = [
			Vector2I.Up, Vector2I.Down, Vector2I.Left, Vector2I.Right
		];

		[Export] private Vector2I NavigableCell { set; get; } = Vector2I.Zero;
		[Export] private Vector2I SnapThreshold { set; get; } = new Vector2I(5, 3);
		[Export] private Texture2D CellTexture { set; get; }
		[Export] private StateMachine FSM { set; get; }

        private int size = 100;
		private readonly HashSet<Vector2I> cells = [];
		private readonly AStar2D pathFinder = new AStar2D();
		private Player player;
		private Vector2I playerDestination;
		private Vector2 centre;

		public Player Player => player;

		public override void _Ready() {
			this.ClearLayer((int)Layer.Base);
			this.Generate(this.size, Vector2I.Zero);
			this.centre = this.Position;
			Vector2 globalPos = this.ToGlobal(this.MapToLocal(this.cells.ElementAt(Utilities.Randi(0, this.size - 1))));
			this.player = GameManager.Instantiate<Player>(Player.Scene, globalPos, this);
			this.Translate(this.centre - this.player.Position);
			this.centre = this.player.Position;

			this.FSM.Listen<PlayerReachedDestinationEvent>();
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

		public async Task Reveal(Vector2I centre, int radius) {
			HashSet<Vector2I> visited = new HashSet<Vector2I>();
			Queue<(Vector2I, int)> queue = new Queue<(Vector2I, int)>();
			queue.Enqueue((centre, 0));

			while (queue.Count > 0) {
				(Vector2I, int) next = queue.Dequeue();
				Vector2I cell = next.Item1;
				int d = next.Item2;
                Sprite2D cellSprite = new Sprite2D {
                    Position = this.MapToLocal(cell),
					Texture = this.CellTexture,
					Modulate = new Color(1, 1, 1, 0)
                };
				this.AddChild(cellSprite);
				await AnimationManager.Animate(cellSprite, "modulate:a", 1, 0.2, Tween.EaseType.In, true);
				this.SetCell((int)Layer.Base, cell, 0, this.NavigableCell);
				visited.Add(cell);
				if (d < radius) {
					foreach (Vector2I dir in GameBoard.DIRECTIONS) {
						Vector2I pos = cell + dir;
						if ((!visited.Contains(pos)) && this.cells.Contains(pos) && 
								(!this.pathFinder.HasPoint(pos))) {
							queue.Enqueue((pos, d + 1));
						}
					}
				}	
			}

			foreach (Vector2I cell in visited) {
				this.pathFinder.AddPoint((long)cell.UniqueId(), this.MapToLocal(cell));
			}
			foreach (Vector2I cell in visited) {
				this.pathFinder.ConnectNeighbours(cell, GameBoard.DIRECTIONS);
			}
		}

		public async Task SnapMap() {
			Vector2I playerPos = this.LocalToMap(this.player.Position);
			Vector2I centralCell = this.LocalToMap(this.centre);
			Vector2I offset = new Vector2I(Math.Abs(playerPos.X - centralCell.X), Math.Abs(playerPos.Y - centralCell.Y));
			if (offset.X > this.SnapThreshold.X || offset.Y > this.SnapThreshold.Y) {
				Task snap = AnimationManager.Animate(this, "position", this.Position + this.centre - this.player.Position, 1, Tween.EaseType.InOut);
				this.centre = this.player.Position;
				await snap;
			}
			await this.Reveal(playerPos, 3);
		}

		public void FindPath() {
			this.ClearLayer((int)Layer.UI);
			Vector2I pos = this.LocalToMap(this.GetLocalMousePosition());
			if (this.pathFinder.HasPoint(pos) && this.LocalToMap(this.player.Position) != pos) {
				this.playerDestination = pos;
                this.player.Path = this.pathFinder.GetPointPath(
                    this.LocalToMap(this.player.Position), pos
				);
				foreach (Vector2 cell in this.player.Path) {
					this.SetCellsTerrainConnect((int)Layer.UI, [..this.player.Path.Select(x => this.LocalToMap(x))], 0, 0);
				}
			} else {
				this.player.Path = [];
				this.playerDestination = this.LocalToMap(this.player.Position);
			}
		}

		public bool MovePlayer() {
			this.ClearLayer((int)Layer.UI);
			if (!this.pathFinder.HasPoint(this.LocalToMap(this.GetLocalMousePosition()))) {
				this.player.Path = [];
				return false;
			} 
			if (this.player.Path.Length > 0) {
				this.SetCellsTerrainConnect((int)Layer.UI, [this.playerDestination], 0, 0);
				this.player.Move();
				this.player.Path = [];
				return true;
			}
			return false;
		}

        public override void _UnhandledInput(InputEvent @event) {
			this.FSM.OnInput(@event);
        }
    }
}
