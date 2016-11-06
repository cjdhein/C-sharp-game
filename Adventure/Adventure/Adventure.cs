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
        private Monster _monster; //current opponent
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
                    foreach(QuestCompletionItem qci in newLocation.QuestAvailableHere.QuestCompletionItems)
                }
            }

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
