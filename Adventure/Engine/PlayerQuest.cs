using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class PlayerQuest
    {
        //required variables
        public Quest Details { get; set; }
        public bool IsCompleted { get; set; }

        //constructor
        public PlayerQuest(Quest details)
        {
            Details = details;
            IsCompleted = false; //always starts false
        }
    }
}
