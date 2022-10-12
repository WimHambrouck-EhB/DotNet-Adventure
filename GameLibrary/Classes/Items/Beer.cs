using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary.Classes.Items
{
    public class Beer : IItem, IUsable, ILookable, ICombinableWith<Beer>
    {
        public List<string> Names => new List<string> { "beer", "bottle", "bottle of beer", "beer bottle" };
        public bool PlayerDrankTooMuch { get; set; }
        private int drinkCount;

        public string LookMessage()
        {
            return "Ik ben niet verslaafd aan den drank. Ik heb het wel nodig.";
        }

        public string UsedWith(Beer item)
        {
            // als de gebruiker "use beer" typt, wordt dit door game.Use() geïnterpreteerd als "use beer on beer".
            // De programmeur roept hier even dichterlijke vrijheid in om dit te interpreteren als "drink beer".
            // De UsedWith methode zal andere berichtjes teruggeven naargelang de speler meer drinkt. De speler kan blijkbaar ook niet zo goed tegen de drank, want na 8 slokken valt hij flauw.
            drinkCount++;            
            if (drinkCount >= 2 && drinkCount < 6)
            {
                return "You start feeling a little woozy.";
            }
            else if (drinkCount >= 6 && drinkCount <= 8)
            {
                return "the Beer iS rleARlly starTing to g e t to youur HEAD";
            }
            else if(drinkCount > 8)
            {
                PlayerDrankTooMuch = true;
                return "You really can't handle the alcohol very well. You stumble a few more paces and pass out on the side of a road...";
            }

            return "A little sip won't do any harm.";
        }
        public override string ToString()
        {
            return "Bottle of beer";
        }
    }
}
