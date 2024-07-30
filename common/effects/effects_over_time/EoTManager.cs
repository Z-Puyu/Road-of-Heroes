using System.Collections.Generic;
using System.Threading.Tasks;
using Game.common.effects.eot;
using Godot;

namespace Game.common.tokens {
	public partial class EoTManager : VBoxContainer {
		private readonly Dictionary<EoT.Effect, Token> eots = [];
		[Export] private CharacterCard Root { set; get; }

        public override async void _Ready() {
			await this.Root.ToSignal(this.Root, Node.SignalName.Ready);
            foreach (Node child in this.GetChildren()) {
                if (child is Token token) {
					token.Root = this.Root;
                    this.eots.Add(token.Effect, token);
                }
            }
        }

        public void Add(EoT eot) {
            if (this.eots.TryGetValue(eot.EffectType, out Token token)) {
                token.Compose(eot);
                this.MoveChild(token, 0);
                token.Show();
            } 
        }

        public void Remove(EoT.Effect eot) {
            if (this.eots.TryGetValue(eot, out Token token)) {
                token.Cure();
            } 
        }

        public async Task ApplyTo() {
            foreach (Token token in this.eots.Values) {
                await token.Apply();
            }
        }
    }
}
