using Godot;
using System;

//Author : Baptiste SIMON
namespace Com.IsartDigital.Jam.UI{
	
    public class PreLoad : Control
    {
        static private PreLoad instance;
		
		static public PreLoad GetInstance () {
			if (instance == null) instance = new PreLoad();
		    return instance;
		}

        private PreLoad ():base() {}

        public override void _Ready()
        {
            if (instance != null){  
                QueueFree();
                GD.Print(nameof(PreLoad) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;

            TranslationServer.SetLocale("en");
        }

        private void OnTimeout()
        {
            GetTree().ChangeScene(GlobalReferences.Scenes.MAIN);
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}