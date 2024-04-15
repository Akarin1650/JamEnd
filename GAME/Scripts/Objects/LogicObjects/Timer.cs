using Godot;
using System;

//Author : Baptiste SIMON
namespace Com.IsartDigital.ProjectName{
	
    public class Timer : Node
    {
        static private Timer instance;
		
		static public Timer GetInstance () {
			if (instance == null) instance = new Timer();
		    return instance;
		}

        private Timer ():base() {}

        public override void _Ready()
        {
            if (instance != null){  
                QueueFree();
                GD.Print(nameof(Timer) + " Instance already exist, destroying the last added.");
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