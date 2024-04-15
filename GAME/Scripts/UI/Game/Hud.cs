using Godot;
using System;

//Author : Baptiste SIMON
namespace Com.IsartDigital.ProjectName{
	
    public class Hud : Node
    {
        static private Hud instance;
		
		static public Hud GetInstance () {
			if (instance == null) instance = new Hud();
		    return instance;
		}

        private Hud ():base() {}

        public override void _Ready()
        {
            if (instance != null){  
                QueueFree();
                GD.Print(nameof(Hud) + " Instance already exist, destroying the last added.");
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