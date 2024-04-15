using Com.IsartDigital.Jam.Managers;
using Godot;
using System;

//Author : Baptiste SIMON
namespace Com.IsartDigital.Jam.UI {
	
    public class TitleCard : Control
    {

        [Export] private NodePath btnPlayPath;

        private Button btnPlay;


        static private TitleCard instance;
		
		static public TitleCard GetInstance () {
			if (instance == null) instance = new TitleCard();
		    return instance;
		}

        private TitleCard ():base() {}

        public override void _Ready()
        {
            if (instance != null){  
                QueueFree();
                GD.Print(nameof(TitleCard) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;

            btnPlay = (Button)GetNode(btnPlayPath);
            btnPlay.Connect(GlobalReferences.Signals.PRESSED, this, nameof(OnPlayPressed));
        }

        private void OnPlayPressed()
        {
            Visible = false;
            GameManager.GetInstance().SpawnCards();
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}