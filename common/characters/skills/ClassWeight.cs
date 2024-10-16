using System;
using Game.common.characters.classes;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.characters.skills {
    [RegisteredType(nameof(ClassWeight), "", nameof(Resource)), GlobalClass]
    public partial class ClassWeight : Resource, IComparable<ClassWeight> {
        [Export] public Class Class { private set; get; }
        [Export(PropertyHint.Range, "1,3")] private uint Weight { set; get; }

        public ClassWeight() {}

        private ClassWeight(Class @class, uint weight) {
            this.Class = @class;
            this.Weight = weight;
        }

        public int CompareTo(ClassWeight other) {
            return this.Weight.CompareTo(other.Weight);
        }

        public override string ToString() {
            return $"[{this.Class}: {this.Weight}]";
        }

        public static ClassWeight operator+(ClassWeight w1, ClassWeight w2) {
            if (w1.Class == w2.Class) {
                return new ClassWeight(w1.Class, w1.Weight + w2.Weight);
            }
            throw new ArgumentException($"{w1} + {w2} is undefined because their classes are different.");
        }

        public static bool operator==(ClassWeight w1, ClassWeight w2) {
            return w1.Weight == w2.Weight;
        }

        public static bool operator!=(ClassWeight w1, ClassWeight w2) {
            return w1.Weight != w2.Weight;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            if (ReferenceEquals(obj, null)) {
                return false;
            }
            return obj is ClassWeight w && this.Class == w.Class && this.Weight == w.Weight;
        }

        public override int GetHashCode() {
            return HashCode.Combine(Class, Weight);
        }
    }
}