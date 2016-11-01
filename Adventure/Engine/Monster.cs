using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Monster : LivingCreature
    {
        //public variables
        public int ID { get; set; }
        public string Name { get; set; }
        public int MaximumDamage { get; set; }
        public int RewardExp { get; set; }
        public int RewardGold { get; set; }
        public List<LootItem> LootTable { get; set; }
        
        //constructor
        public Monster(int id, string name, int maxDamage, int rewardExp, int rewardGold,
            int maxHitPoints, int curHitPoints) : base(maxDamage,curHitPoints)
        {
            ID = id;
            Name = name;
            MaximumDamage = maxDamage;
            RewardExp = rewardExp;
            RewardGold = rewardGold;
            LootTable = new List<LootItem>();
        }

    }
}
