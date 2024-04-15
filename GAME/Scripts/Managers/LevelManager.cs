using Godot;
using System;

namespace Com.IsartDigital.Jam.Managers
{

    public class LevelManager : Node2D
    {
        static private LevelManager instance;

        static public LevelManager GetInstance()
        {
            if (instance == null) instance = new LevelManager();
            return instance;
        }

        private LevelManager() : base() { }
        [Export] private PackedScene enemyCardScene;
        [Export] private PackedScene summonCardScene;

        private float cardYMargin;
        private float cardXMargin = 150f;
        private float cardXSpace;

        public override void _Ready()
        {
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(LevelManager) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;
            cardYMargin = GameManager.GetInstance().screenSize.y / 2 + 100f;
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