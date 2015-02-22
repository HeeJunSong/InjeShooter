using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using InjeShooter.PSEngine;

namespace InjeShooter
{
    public partial class MainFrame : Form
    {
        PSGameEngine game;
        public MainFrame()
        {
            InitializeComponent();
            game = new PSGameEngine(this);
            game.SetScene(new MainScene());
        }
    }
}
