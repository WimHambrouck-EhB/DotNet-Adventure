using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary.Classes.Items
{
    public class Can : IItem, IUsable, ITakeable, ILookable
    {
        public List<string> Names => new List<string> { "can", "can of wd40", "wd40", "lubricant", "can of lubricant" };
        public bool IsEmpty { get; set; }

        public string LookMessage()
        {
            // na gebruik op Door, is de bus leeg (zie ook Door klasse)
            if(IsEmpty)
            {
                return "A can of WD40. It's empty.";
            }
            else
            {
                return "A can of WD40. \"Lubricates almost anything.\"";
            }
        }

        public string TakeMessage()
        {
            return "Yuk, it's all greasy! Trying not to get your hands filthy, you quickly put it in your bag.";
        }
        public override string ToString()
        {
            return "Can of WD40";
        }

    }
}
