using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary.Classes.Items
{
    public class Key : Item, ITakeable, ILookable, IUsable
    {
        public override List<string> Names => new List<string> { "key", "unwieldy key", "large key", "old key", "old looking key" };

        public string LookMessage(Room currentRoom)
        {
            return "A rather unwieldy, old looking key. It must have been lying here for ages!";
        }

        public string TakeMessage()
        {
            return "You decide to keep the key in your pocket, should you need it.";
        }

        public override string ToString()
        {
            return "Old key";
        }
    }
}
