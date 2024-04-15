using Godot;
using System;

//Author : Baptiste SIMON
namespace Com.IsartDigital.Jam.Managers
{

    public class UIManager : Control
    {
        static private UIManager instance;
		
		static public UIManager GetInstance () {
			if (instance == null) instance = new UIManager();
		    return instance;
		}

        private UIManager ():base() {}

        public override void _EnterTree()
        {
            if (instance != null){  
                QueueFree();
                GD.Print(nameof(UIManager) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;


        }

        public override void _Process(float pDelta)
        {

        }


        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}