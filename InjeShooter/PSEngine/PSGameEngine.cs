using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace InjeShooter.PSEngine
{
    class PSGameEngine
    {
        private Color _MainColor = Color.FromArgb(255,25,25,25);
        private int hfps = 60;
        private int fps = 0;
        private List<RunningState> list_RState;
        private RunningState state = RunningState.RUNNING;
        private Thread coreThread;
        private PSScene scene;
        private Form mForm;
        private Bitmap backBuffer;

        /**<summary>
         * 초기 값 : 60 fps
         * </summary>
         **/
        public int FPS
        {
            get { return this.fps; }
        }

        public void SetTargetFPS(int fps)
        {
            this.hfps = fps;
        }

        public PSGameEngine(Form form)
        {
            mForm = form;
            this.list_RState = new List<RunningState>();
            mForm.SizeChanged += mForm_SizeChanged;
            mForm.Shown += mForm_Shown;
            mForm.FormClosing += mForm_FormClosing;
        }

        public void SetSize(Point size)
        {
            mForm.Width = size.X;
            mForm.Height = size.Y;
        }

        public void SetSizeMode(FormWindowState state)
        {
            mForm.WindowState = state;
        }

        public void SetBorderStyle(FormBorderStyle style)
        {
            mForm.FormBorderStyle = style;
        }

        void mForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Finish();
        }

        void mForm_Shown(object sender, EventArgs e)
        {
            if (this.scene == null)
                throw new Exception("Form이 활성화 되기 전에 Scene을 지정해주세요.");
            this.Resume();
        }

        void mForm_SizeChanged(object sender, EventArgs e)
        {
            if (mForm.WindowState != FormWindowState.Minimized)
            {
                if (backBuffer != null)
                    backBuffer.Dispose();
                backBuffer = new Bitmap(mForm.Size.Width, mForm.Size.Height);
                Graphics g = Graphics.FromImage(backBuffer);
                g.Clear(_MainColor);
                if (coreThread == null)
                    this.Resume();
            }
            else this.Pause();
        }

        public void SetScene(PSScene scene)
        {
            if (this.scene != null)
                this.scene.OnFinished();
            this.scene = scene;
            this.scene.SetGameEngine(this);
            this.scene.OnResumed();
        }

        public void Resume()
        {
            lock (list_RState)
            {
                list_RState.Add(RunningState.RESUMED);
            }
            coreThread = new Thread(CoreThread);
            coreThread.Start();
        }

        public void Pause()
        {
            lock (list_RState)
            {
                list_RState.Add(RunningState.PAUSED);
            }
            coreThread.Join();
        }

        public void Finish()
        {
            lock (list_RState)
            {
                list_RState.Add(RunningState.FINISHED);
            }
            if(coreThread != null)
                coreThread.Join();
        }

        private void CoreThread()
        {
            var timer = new Stopwatch();
            timer.Start();
            long oldTime = timer.ElapsedMilliseconds;
            long elapsedTime = 0;
            int fps = 0;
            while (state != RunningState.FINISHED)
            {
                lock (list_RState)
                {
                    if (list_RState.Count > 0)
                    {
                        state = list_RState[0];
                        switch (state)
                        {
                            case RunningState.FINISHED:
                                this.scene.OnFinished();
                                list_RState.Clear();
                                timer.Stop();
                                return;
                            case RunningState.PAUSED:
                                this.scene.OnPaused();
                                list_RState.Clear();
                                timer.Stop();
                                coreThread = null;
                                return;
                            case RunningState.RESUMED:
                                this.scene.OnResumed();
                                list_RState.RemoveAt(0);
                                state = RunningState.RUNNING;
                                break;
                        }
                    }
                }
                if (state == RunningState.RUNNING)
                {
                    long nowTime = timer.ElapsedMilliseconds;
                    long tempElapsedTime = oldTime - nowTime;
                    long targetTime = (long)(1000.0f / fps);
                    if (tempElapsedTime >= targetTime)
                    {
                        elapsedTime += tempElapsedTime;
                        fps++;
                        if (elapsedTime > 1000)
                        {
                            this.fps = fps;
                            elapsedTime -= 1000;
                            fps = 0;
                        }

                        backBuffer = new Bitmap(mForm.Size.Width, mForm.Size.Height);

                        #region Core Part

                        scene.Update(tempElapsedTime);

                        #endregion
                    }
                    else Thread.Sleep((int)(targetTime - tempElapsedTime));
                }
            }
        }

        public void DrawBitmap(Bitmap bitmap, Point position)
        {

        }
    }
}
