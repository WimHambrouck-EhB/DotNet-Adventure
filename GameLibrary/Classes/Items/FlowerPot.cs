using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary.Classes.Items
{
    public class FlowerPot : IItem, ILookable
    {
        public List<string> Names => new List<string> { "flower pot", "pot", "ornate pot", "ornate flower pot" };

        public string LookMessage()
        {
            return "A terracotta pot that hasn't been cleaned for some time. It's got a lovely flower in it, though.";
        }

        public override string ToString()
        {
            return "Flower pot";
        }
    }
}
