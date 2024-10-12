using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace Game.common.modules {
    [GlobalClass]
    public abstract partial class Module : Node {
        private Node root;

        protected Node Root { 
            get => root; 
            set {
                if (value == this.GetParent()) {
                    root = value;
                }
            }
        }

        public override async void _Ready() {
            await this.ToSignal(this.GetParent(), SignalName.Ready);
            this.Root = this.GetParent();
            this.ConnectEvents();
        }

        protected abstract void ConnectEvents();
    }
}