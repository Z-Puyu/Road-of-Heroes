using System;
using Game.common.characters;
using Game.util.events;
using Game.util.events.test;
using Game.util.math;
using Godot;

namespace Game.tests {
    public partial class CharacterStatsTest : Node {
        private int id = 0;
        private Guid eventId;
        private int success = 0;

        public CharacterStatsTest() {}

        public CharacterStatsTest(int id) {
            this.id = id;
        }
        
        public override void _Ready() {
            CharacterStatsTest another = new CharacterStatsTest(1);
            another.Subscribe<EventArgs>(another.OnTestEvent);
            this.Subscribe<EventArgs>(this.OnTestEvent);
            Console.WriteLine("Generating a random character...");
            Hero character = Hero.Random((uint)MathUtil.Randi(0, 6));
            character.Log();
            Console.WriteLine("Testing the event bus...");
            for (int i = 0; i < 100; i += 1) {
                TestEvent e = new TestEvent();
                Guid id = e.Id;
                this.eventId = id;
                another.eventId = id;
                Console.WriteLine($"Publishing test event with id {id}...");
                character.Publish(e, [another], (sender, target) => target is CharacterStatsTest test && test.id == 0);
            }      
            Console.WriteLine($"Test complete. Success: {this.success}/{100},\n{another.success} events are received by the wrong target.");  
        }

        private void OnTestEvent(EventArgs @event) {
            if (@event is TestEvent e) {
                Console.WriteLine($"Received test event with id: {e.Id}");
                if (e.Id != this.eventId) {
                    Console.WriteLine("Test failed! Events do not match.");
                } else {
                    this.success += 1;
                }
            } else {
                Console.WriteLine("Test failed! Received event is not a TestEvent.");
            }
        }
    }
}