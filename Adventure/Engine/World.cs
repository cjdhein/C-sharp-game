using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Engine
{
    public static class World
    {
        public static readonly XDocument xmlData = XDocument.Load(@"Data.xml");
        //lists
        public static readonly List<Item> Items = new List<Item>();
		public static readonly List<Monster> Monsters = new List<Monster>();
		public static readonly List<Location> Locations = new List<Location>();
		public static readonly List<Quest> Quests = new List<Quest>();
		
		//items
		public const int ITEM_ID_RUSTY_SWORD = 1;
		public const int ITEM_ID_RAT_TAIL = 2;
		public const int ITEM_ID_PIECE_OF_FUR = 3;
		public const int ITEM_ID_SNAKE_FANG = 4;
		public const int ITEM_ID_SNAKESKIN = 5;
		public const int ITEM_ID_CLUB = 6;
		public const int ITEM_ID_HEALING_POTION = 7;
		public const int ITEM_ID_SPIDER_FANG = 8;
		public const int ITEM_ID_SPIDER_SILK = 9;
		public const int ITEM_ID_ADVENTURER_PASS = 10;
		
		//monsters
		public const int MONSTER_ID_RAT = 1;
		public const int MONSTER_ID_SNAKE = 2;
		public const int MONSTER_ID_GIANT_SPIDER = 3;
		
		//quests
		public const int QUEST_ID_CLEAR_ALCHEMIST_GARDEN = 1;
		public const int QUEST_ID_CLEAR_FARMERS_FIELD = 2;
		
		//locations
		public const int LOCATION_ID_HOME = 1;
		public const int LOCATION_ID_TOWN_SQUARE = 2;
		public const int LOCATION_ID_GUARD_POST = 3;
		public const int LOCATION_ID_ALCHEMIST_HUT = 4;
		public const int LOCATION_ID_ALCHEMISTS_GARDEN = 5;
		public const int LOCATION_ID_FARMHOUSE = 6;
		public const int LOCATION_ID_FARM_FIELD = 7;
		public const int LOCATION_ID_BRIDGE = 8;
		public const int LOCATION_ID_SPIDER_FIELD = 9;
		
		//constructor
		static World()
		{
			PopulateItems();
			PopulateMonsters();
			PopulateQuests();
			PopulateLocations();
		}
		
		//initializes items
		private static void PopulateItems()
		{
            Items.Add(new Weapon(ITEM_ID_RUSTY_SWORD, "Rusty sword", "Rusty swords", 0, 5));
            Items.Add(new Item(ITEM_ID_RAT_TAIL, "Rat tail", "Rat tails"));
            Items.Add(new Item(ITEM_ID_PIECE_OF_FUR, "Piece of fur", "Pieces of fur"));
            Items.Add(new Item(ITEM_ID_SNAKE_FANG, "Snake fang", "Snake fangs"));
            Items.Add(new Item(ITEM_ID_SNAKESKIN, "Snakeskin", "Snakeskins"));
            Items.Add(new Weapon(ITEM_ID_CLUB, "Club", "Clubs", 3, 10));
            Items.Add(new HealingPotion(ITEM_ID_HEALING_POTION, "Healing potion", "Healing potions", 5));
            Items.Add(new Item(ITEM_ID_SPIDER_FANG, "Spider fang", "Spider fangs"));
            Items.Add(new Item(ITEM_ID_SPIDER_SILK, "Spider silk", "Spider silks"));
            Items.Add(new Item(ITEM_ID_ADVENTURER_PASS, "Adventurer pass", "Adventurer passes"));
		}
		
		//initializes monsters
        private static void PopulateMonsters()
        {
            Monster rat = new Monster(MONSTER_ID_RAT, "Rat", 5, 3, 10, 3, 3);
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_RAT_TAIL), 75, false));
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_PIECE_OF_FUR), 75, true));

            Monster snake = new Monster(MONSTER_ID_SNAKE, "Snake", 5, 3, 10, 3, 3);
            snake.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKE_FANG), 75, false));
            snake.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKESKIN), 75, true));

            Monster giantSpider = new Monster(MONSTER_ID_GIANT_SPIDER, "Giant spider", 20, 5, 40, 10, 10);
            giantSpider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_FANG), 75, true));
            giantSpider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_SILK), 25, false));
			
			/* populate list */
            Monsters.Add(rat);
            Monsters.Add(snake);
            Monsters.Add(giantSpider);
        }
		
		//initializes quests
        private static void PopulateQuests()
        {

			
			
			/* Alchemists Garden */
            Quest clearAlchemistGarden =
                new Quest(
                    QUEST_ID_CLEAR_ALCHEMIST_GARDEN, 
					"Clear the alchemist's garden", 
					"Kill rats in the alchemist's garden and bring back 3 rat tails. You will receive a healing potion and 10 gold pieces.", 20, 10);

            clearAlchemistGarden.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_RAT_TAIL), 3));

            clearAlchemistGarden.RewardItem = ItemByID(ITEM_ID_HEALING_POTION);
			
			/* Farmer's Field */
            Quest clearFarmersField =
                new Quest(
                    QUEST_ID_CLEAR_FARMERS_FIELD,
                    "Clear the farmer's field",
                    "Kill snakes in the farmer's field and bring back 3 snake fangs. You will receive an adventurer's pass and 20 gold pieces.", 20, 20);

            clearFarmersField.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_SNAKE_FANG), 3));

            clearFarmersField.RewardItem = ItemByID(ITEM_ID_ADVENTURER_PASS);
			
			/* populate list */
            Quests.Add(clearAlchemistGarden);
            Quests.Add(clearFarmersField);
        }		
		
        private static void PopulateLocations()
        {
			var xmlLocations = xmlData.Descendants("Locations"); //.Root.Descendants("Locations");
				
           
            // Create each location
            Location home = new Location(LOCATION_ID_HOME, xmlLocations.ElementAt(LOCATION_ID_HOME).Descendants("name").ToString()
                , xmlLocations.ElementAt(LOCATION_ID_HOME).Descendants("description").ToString());

            Location townSquare = new Location(LOCATION_ID_TOWN_SQUARE, xmlLocations.ElementAt(LOCATION_ID_TOWN_SQUARE).Descendants("name").ToString()
                , xmlLocations.ElementAt(LOCATION_ID_TOWN_SQUARE).Descendants("description").ToString());

            Location alchemistHut = new Location(LOCATION_ID_ALCHEMIST_HUT, "Alchemist's hut", "There are many strange plants on the shelves.");
            alchemistHut.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_ALCHEMIST_GARDEN);

            Location alchemistsGarden = new Location(LOCATION_ID_ALCHEMISTS_GARDEN, "Alchemist's garden", "Many plants are growing here.");
            alchemistsGarden.MonsterHere = MonsterByID(MONSTER_ID_RAT);

            Location farmhouse = new Location(LOCATION_ID_FARMHOUSE, "Farmhouse", "There is a small farmhouse, with a farmer in front.");
            farmhouse.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_FARMERS_FIELD);

            Location farmersField = new Location(LOCATION_ID_FARM_FIELD, "Farmer's field", "You see rows of vegetables growing here.");
            farmersField.MonsterHere = MonsterByID(MONSTER_ID_SNAKE);

            Location guardPost = new Location(LOCATION_ID_GUARD_POST, "Guard post", "There is a large, tough-looking guard here.", ItemByID(ITEM_ID_ADVENTURER_PASS));

            Location bridge = new Location(LOCATION_ID_BRIDGE, "Bridge", "A stone bridge crosses a wide river.");

            Location spiderField = new Location(LOCATION_ID_SPIDER_FIELD, "Forest", "You see spider webs covering covering the trees in this forest.");
            spiderField.MonsterHere = MonsterByID(MONSTER_ID_GIANT_SPIDER);

            // Link the locations together
            home.LocToNorth = townSquare;

            townSquare.LocToNorth = alchemistHut;
            townSquare.LocToSouth = home;
            townSquare.LocToEast = guardPost;
            townSquare.LocToWest = farmhouse;

            farmhouse.LocToEast = townSquare;
            farmhouse.LocToWest = farmersField;

            farmersField.LocToEast = farmhouse;

            alchemistHut.LocToSouth = townSquare;
            alchemistHut.LocToNorth = alchemistsGarden;

            alchemistsGarden.LocToSouth = alchemistHut;

            guardPost.LocToEast = bridge;
            guardPost.LocToWest = townSquare;

            bridge.LocToWest = guardPost;
            bridge.LocToEast = spiderField;

            spiderField.LocToWest = bridge;

            // Add the locations to the static list
            Locations.Add(home);
            Locations.Add(townSquare);
            Locations.Add(guardPost);
            Locations.Add(alchemistHut);
            Locations.Add(alchemistsGarden);
            Locations.Add(farmhouse);
            Locations.Add(farmersField);
            Locations.Add(bridge);
            Locations.Add(spiderField);
        }		
		
		//functions to get objects by their ID
		public static Item ItemByID(int id)
		{
			foreach(Item item in Items)
			{
				if(item.ID == id)
					return item;
			}
			
			return null; //no item found
		}
		
		public static Monster MonsterByID(int id)
		{
			foreach(Monster monster in Monsters)
			{
				if(monster.ID == id)
					return monster;
			}
			return null; //no item found
		}
		
		public static Location LocationByID(int id)
		{
			foreach(Location location in Locations)
			{
				if(location.ID == id)
					return location;
			}
			return null; //no item found
		}		

		public static Quest QuestByID(int id)
		{
			foreach(Quest quest in Quests)
			{
				if(quest.ID == id)
					return quest;
			}
			return null; //no item found
		}		
    }
}
