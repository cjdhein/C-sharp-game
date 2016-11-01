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
        public Adventure()
        {
            InitializeComponent();

            Location location = new Location(1, "Home", "This is your house.");

            _player = new Player(20, 0, 1, 10, 10);

            lblHitPoints.Text = _player.CurHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();
        }

    }
}
