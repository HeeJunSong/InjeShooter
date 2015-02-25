using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InjeShooter.PSEngine;
using System.Drawing;

namespace InjeShooter
{
    class MainScene : PSScene
    {
        Bitmap btm_Logo;
        public override void OnEngineSetted()
        {
            System.Resources.ResourceManager rm =
                InjeShooter.Properties.Resources.ResourceManager;
            btm_Logo = rm.GetObject("Logo") as Bitmap;
            engine.SetSizeMode(System.Windows.Forms.FormWindowState.Maximized);
            engine.SetBorderStyle(System.Windows.Forms.FormBorderStyle.None);
        }
        public override void Update(long deltaTime)
        {
            
        }

        public override void OnResumed()
        {
            
        }

        public override void OnFinished()
        {
            
        }

        public override void OnPaused()
        {
            
        }
    }
}
