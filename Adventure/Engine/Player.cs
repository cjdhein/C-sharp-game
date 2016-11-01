using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Engine
{
    /*Holds the players information*/
    public class Player : LivingCreature
    {
        //required variables
        public int Gold { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
        public List<InventoryItem> Inventory { get; set; }
        public List<PlayerQuest> Quests { get; set; }

        //constructor
        public Player(int gold, int exp, int level, int curHitpoints, int maxHitpoints)
            : base(maxHitpoints,curHitpoints)
        {
            Gold = gold;
            ExperiencePoints = exp;
            Level = level;
            Inventory = new List<InventoryItem>();
            Quests = new List<PlayerQuest>();
        }

    }
}
