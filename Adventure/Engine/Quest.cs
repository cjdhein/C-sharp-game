using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Quest
    {
        //required variables
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RewardExp { get; set; }
        public int RewardGold { get; set; }
        public List<QuestCompletionItem> QuestCompletionItems { get; set; }

        //optional variables
        public Item RewardItem { get; set; }

        //constructor
        public Quest(int id, string name, string description, int rewardExp, int rewardGold,
            Item rewardItem = null)
        {
            ID = id;
            Name = name;
            Description = description;
            RewardExp = rewardExp;
            RewardGold = rewardGold;
            RewardItem = rewardItem;
            QuestCompletionItems = new List<QuestCompletionItem>();
        }
        

    }
}
