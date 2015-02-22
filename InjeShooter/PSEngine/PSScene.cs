using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjeShooter.PSEngine
{
    abstract class PSScene
    {
        protected PSGameEngine engine;
        public void SetGameEngine(PSGameEngine engine)
        {
            this.engine = engine;
            this.OnEngineSetted();
        }
        public abstract void OnEngineSetted();
        public abstract void Update(long deltaTime);
        public abstract void OnResumed();
        public abstract void OnPaused();
        public abstract void OnFinished();
    }
}
