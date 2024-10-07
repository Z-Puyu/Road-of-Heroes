using System.Collections.Generic;
using Game.util.events;
using Game.util.events.system;
using Godot;

namespace Game.util {
    public static class GodotExtensions {
        public static Stack<Node> GetSubTree(this Node node) {
            Stack<Node> offsprings = [];
            offsprings.Push(node);
            foreach (Node child in node.GetChildren()) {
                Stack<Node> lowerOffsprings = child.GetSubTree();
                while (lowerOffsprings.Count > 0) {
                    offsprings.Push(lowerOffsprings.Pop());
                }
            }
            return offsprings;
        }

        public static void Recycle(this Node node) {
            Stack<Node> offsprings = node.GetSubTree();
            while (offsprings.Count > 0) {
                Node curr = offsprings.Pop();
                EventChannel<RecycleObjectEvent>.Publish(new RecycleObjectEvent(curr));
            }
        }

        public static void Disable(this Node node) {
            Stack<Node> offsprings = node.GetSubTree();
            while (offsprings.Count > 0) {
                Node curr = offsprings.Pop();
                if (curr is Node2D node2D) {
                    node2D.Hide();
                } else if (curr is Control control) {
                    control.Hide();
                } else if (curr is Node3D node3D) {
                    node3D.Hide();
                }
            }
        }

        public static void ConnectNeighbours(this AStar2D aStar2D, Vector2I src, 
                IEnumerable<Vector2I> possibleDirections) {
            foreach (Vector2I dir in possibleDirections) {
				long neighbourId = (long)Util.UniqueId(src + dir);
				if (aStar2D.HasPoint(neighbourId)) {
					aStar2D.ConnectPoints(neighbourId, (long)Util.UniqueId(src));
				}
			}
        }

        public static void DisconnectNeighbours(
            this AStar2D aStar2D, Vector2I src, 
            IEnumerable<Vector2I> possibleDirections,
            bool bidirectional = false
        ) {
            foreach (Vector2I dir in possibleDirections) {
				long neighbourId = (long)Util.UniqueId(src + dir);
				if (aStar2D.HasPoint(neighbourId)) {
					aStar2D.DisconnectPoints(neighbourId, (long)Util.UniqueId(src), bidirectional);
				}
			}
        }

        public static bool HasPoint(this AStar2D aStar2D, Vector2I pos) {
            return aStar2D.HasPoint((long)Util.UniqueId(pos));
        }

        public static Vector2[] GetPointPath(this AStar2D aStar2D, Vector2I from, Vector2I to) {
            long fromId = (long)Util.UniqueId(from);
            long toId = (long)Util.UniqueId(to);
            return aStar2D.GetPointPath(fromId, toId);
        }
    }
}