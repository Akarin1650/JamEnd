using Godot;
using System;

//Author : Baptiste SIMON
namespace Com.IsartDigital.ProjectName{
	
    public class WinScreen : Node
    {
        static private WinScreen instance;
		
		static public WinScreen GetInstance () {
			if (instance == null) instance = new WinScreen();
		    return instance;
		}

        private WinScreen ():base() {}

        public override void _Ready()
        {
            if (instance != null){  
                QueueFree();
                GD.Print(nameof(WinScreen) + " Instance already exist, destroying the last added.");
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