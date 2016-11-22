using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Engine;

namespace Adventure
{
    public partial class Adventure : Form
    {
        private Player _player; //the game's player
        private Monster _currentMonster; //current opponent
        public Adventure()
        {
            InitializeComponent();
            
            _player = new Player(20, 0, 1, 10, 10);


            lblHitPoints.Text = _player.CurHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();
        }

        private void MoveTo(Location newLocation)
        {
            if(newLocation.ItemRequiredToEnter != null) //if item is required
            {
                //Check if player has required item
                bool playerHasRequiredItem = false; //default false

                foreach(InventoryItem ii in _player.Inventory)
                {
                    if(ii.Details.ID == newLocation.ItemRequiredToEnter.ID)
                    {
                        //item is found in player's inventory
                        playerHasRequiredItem = true;
                        break; //leave for each now that item was found
                    }
                }

                if(!playerHasRequiredItem)
                {
                    //required item was not found
                    rtbMessages.Text += "You must have a " +
                        newLocation.ItemRequiredToEnter.Name +
                        " to enter this location." + Environment.NewLine;
                    return;

                }
            }
            //update player's location
            _player.CurrentLocation = newLocation;

            // Show / Hide available movement options
            btnNorth.Visible = (newLocation.LocToNorth != null);
            btnEast.Visible = (newLocation.LocToEast != null);
            btnSouth.Visible = (newLocation.LocToSouth != null);
            btnWest.Visible = (newLocation.LocToWest != null);

            //display current location name and description
            rtbLocation.Text = newLocation.Name + Environment.NewLine;
            rtbLocation.Text += newLocation.Description + Environment.NewLine;

            //fully heal the player
            _player.CurHitPoints = _player.MaxHitPoints;

            //update hitpoints in UI
            lblHitPoints.Text = _player.CurHitPoints.ToString();

            //does loication have a quest?
            if(newLocation.QuestAvailableHere != null)
            {
                // See if player has the quest and if they completed it
                bool playerAlreadyHasQuest = false;
                bool playerAlreadyCompletedQuest = false;

                foreach(PlayerQuest playerQuest in _player.Quests)
                {
                    if(playerQuest.Details.ID == newLocation.QuestAvailableHere.ID)
                    {
                        playerAlreadyHasQuest = true;

                        //check if player completed quest
                        if(playerQuest.IsCompleted)
                        {
                            playerAlreadyCompletedQuest = true;
                        }
                    }
                }

                //check if player has quest
                if(playerAlreadyHasQuest)
                {
                    //if player did not complete quest
                    if(!playerAlreadyCompletedQuest)
                    {
                        //check if player has required items
                        bool playerHasAllItemsToCompleteQuest = true;

                        foreach(QuestCompletionItem qci in
                            newLocation.QuestAvailableHere.QuestCompletionItems)
                        {
                            bool foundItemInPlayersInventory = false;

                            foreach(InventoryItem ii in _player.Inventory)
                            {
                                if(ii.Details.ID == qci.Details.ID)
                                {
                                    foundItemInPlayersInventory = true;

                                    //check if player has enough of the item
                                    if(ii.Quantity < qci.Quantity)
                                    {
                                        //not enough
                                        playerHasAllItemsToCompleteQuest = false;

                                        //quest can't be complete, so no reason to continue
                                    }

                                    break;
                                }
                            }

                            //item not found
                            if(!foundItemInPlayersInventory)
                            {
                                playerHasAllItemsToCompleteQuest = false;

                                break;
                            }

                        }

                        if(playerHasAllItemsToCompleteQuest)
                        {
                            //display message
                            rtbMessages.Text = Environment.NewLine;
                            rtbMessages.Text += "You complete the " +
                                newLocation.QuestAvailableHere.Name +
                                " quest." + Environment.NewLine;
                            
                            //remove quest item from inventory
                            foreach(QuestCompletionItem qci in
                                newLocation.QuestAvailableHere.QuestCompletionItems)
                            {
                                foreach(InventoryItem ii in _player.Inventory)
                                {
                                    if(ii.Details.ID == qci.Details.ID)
                                    {
                                        //subtract quantity used from player inventory
                                        ii.Quantity -= qci.Quantity;
                                        break;
                                    }
                                }
                            }

                            //give rewards to player
                            _player.ExperiencePoints += newLocation.QuestAvailableHere.RewardExp;
                            _player.Gold += newLocation.QuestAvailableHere.RewardGold;

                            bool addedItemToPlayerInventory = false;
                            //must check if player has reward item already
                            foreach(InventoryItem ii in _player.Inventory)
                            {
                                if(ii.Details.ID == newLocation.QuestAvailableHere.RewardItem.ID)
                                {
                                    //found item, increasing quantity
                                    ii.Quantity++;

                                    addedItemToPlayerInventory = true;

                                    break;
                                }
                            }
                            //Item not found in inventory
                            if(!addedItemToPlayerInventory)
                            {
                                _player.Inventory.Add(new InventoryItem(newLocation.QuestAvailableHere.RewardItem, 1));

                                addedItemToPlayerInventory = true;
                            }

                            //display rewards test
                            rtbMessages.Text += "You receive: " + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardExp.ToString() +
                            " experience" + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardGold.ToString() +
                            " gold" + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardItem.Name + Environment.NewLine;
                            rtbMessages.Text += Environment.NewLine;

                            //mark quest completed in quest log
                            foreach(PlayerQuest pq in _player.Quests)
                            {
                                if(pq.Details.ID == newLocation.QuestAvailableHere.ID)
                                {
                                    pq.IsCompleted = true;

                                    break;
                                }
                            }
                        }
                    }
                }
                else //player does NOT have quest
                {
                    //add quest
                    _player.Quests.Add(new PlayerQuest(newLocation.QuestAvailableHere));

                    rtbMessages.Text += "You have received the " +
                        newLocation.QuestAvailableHere.Name +
                        " quest." + Environment.NewLine;
                    rtbMessages.Text += newLocation.QuestAvailableHere.Description +
                        Environment.NewLine;
                    rtbMessages.Text += "To finish the quest, return with: " +
                        Environment.NewLine;
                    //loop through each completion item
                    foreach (QuestCompletionItem qci in newLocation.QuestAvailableHere.QuestCompletionItems)
                    {
                        if(qci.Quantity == 1)
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " +
                                qci.Details.Name + Environment.NewLine;
                        }
                        else
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " +
                                qci.Details.NamePlural + Environment.NewLine;
                        }
                    }
                    rtbMessages.Text += Environment.NewLine;

                    //Adds quest to player's quest list
                    _player.Quests.Add(new PlayerQuest(newLocation.QuestAvailableHere));
                }
            }

        //Does new location have monster?
            if(newLocation.MonsterHere != null)
            {
                rtbMessages.Text += "You see a " + newLocation.MonsterHere.Name + Environment.NewLine;

                //create the monster using default values
                Monster standardMonster = World.MonsterByID(newLocation.MonsterHere.ID);

                _currentMonster = new Monster(standardMonster.ID, standardMonster.Name, standardMonster.MaximumDamage, standardMonster.RewardExp, standardMonster.RewardGold,
                                                standardMonster.CurHitPoints, standardMonster.MaxHitPoints);
                //populate monster's loot table
                foreach (LootItem lootItem in standardMonster.LootTable)
                    _currentMonster.LootTable.Add(lootItem);

                //monster present, display combat controls
                cboPotions.Visible = true;
                cboWeapons.Visible = true;
                btnUsePotion.Visible = true;
                btnUseWeapon.Visible = true;
            }
            else //no monster
            {
                _currentMonster = null;
                cboPotions.Visible = false;
                cboWeapons.Visible = false;
                btnUsePotion.Visible = false;
                btnUseWeapon.Visible = false;
            }

            //Refresh player's inventory
            refreshInventory();
            //refresh quests
            refreshQuests();

            //refresh player's weapons combobox
            refreshWeaponCboBox();

            //refresh potion's combobox
            refreshPotionsCboBox();
        }
        
        private void refreshWeaponCboBox()
        {
            List<Weapon> weapons = new List<Weapon>();

            foreach(InventoryItem inventoryItem in _player.Inventory)
            {
                if(inventoryItem.Details is Weapon)
                {
                    if(inventoryItem.Quantity > 0)
                    {
                        weapons.Add((Weapon)inventoryItem.Details);
                    }
                }
            }

            if(weapons.Count == 0)
            {
                //player is unarmed, so weapon combobox and use button is hidden
                cboWeapons.Visible = false;
                btnUseWeapon.Visible = false;
            }
            else
            {
                cboWeapons.DataSource = weapons;
                cboWeapons.DisplayMember = "Name";
                cboWeapons.ValueMember = "ID";

                cboWeapons.SelectedIndex = 0;
            }
        }

        private void refreshPotionsCboBox()
        {
            List<HealingPotion> healingPotions = new List<HealingPotion>();

            foreach(InventoryItem inventoryItem in _player.Inventory)
            {
                if(inventoryItem.Details is HealingPotion)
                {
                    if (inventoryItem.Quantity > 0)
                    {
                        healingPotions.Add((HealingPotion)inventoryItem.Details);
                    }
                }
            }

            if(healingPotions.Count == 0)
            {
                //no potions, hide use btn and cbobox
                cboPotions.Visible = false;
                btnUsePotion.Visible = false;
            }
            else
            {
                cboPotions.DataSource = healingPotions;
                cboPotions.DisplayMember = "Name";
                cboPotions.ValueMember = "ID";

                cboPotions.SelectedIndex = 0;
            }
        }

        private void refreshQuests()
        {
            dgvQuests.RowHeadersVisible = false;

            dgvQuests.ColumnCount = 2;
            dgvQuests.Columns[0].Name = "Name";
            dgvQuests.Columns[0].Width = 197;
            dgvQuests.Columns[1].Name = "Done?";

            dgvQuests.Rows.Clear();

            foreach (PlayerQuest playerQuest in _player.Quests)
            {
                dgvQuests.Rows.Add(new[] { playerQuest.Details.Name, playerQuest.IsCompleted.ToString() });
            }
        }
        
        //Refresh player's inventory
        private void refreshInventory()
        {
            dgvInventory.RowHeadersVisible = false;
            dgvInventory.ColumnCount = 2;
            dgvInventory.Columns[0].Name = "Name";
            dgvInventory.Columns[0].Width = 197;
            dgvInventory.Columns[1].Name = "Quantity";

            dgvInventory.Rows.Clear();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if (inventoryItem.Quantity > 0)
                {
                    dgvInventory.Rows.Add(new[] { inventoryItem.Details.Name, inventoryItem.Quantity.ToString() });
                }
            }
        }

        private void btnUseWeapon_Click(object sender, EventArgs e)
        {

        }

        private void btnUsePotion_click(object sender, EventArgs e)
        {

        }
        private void btnNorth_Click(object sender, EventArgs e)
        {

        }

        private void btnEast_Click(object sender, EventArgs e)
        {

        }

        private void btnSouth_Click(object sender, EventArgs e)
        {

        }

        private void btnWest_Click(object sender, EventArgs e)
        {

        }
    }
}
