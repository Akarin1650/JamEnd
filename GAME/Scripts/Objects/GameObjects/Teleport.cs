using Godot;
using System;

namespace Com.IsartDigital.Jam.MyGameObjects
{

    public class Teleport : Node2D
    {
        static private Teleport instance;
        [Export] private NodePath animPath;

        public AnimationPlayer anim;

        static public Teleport GetInstance()
        {
            if (instance == null) instance = new Teleport();
            return instance;
        }

        private Teleport() : base() { }
        [Export] private NodePath areaPath;

        public bool isCardIn;

        public Area2D area;
        public override void _Ready()
        {
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(Teleport) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;
            area = GetNode<Area2D>(areaPath);
            anim = GetNode<AnimationPlayer>(animPath);
            area.Connect(GlobalReferences.Signals.AREA_ENTERED, this, nameof(OnEntered));
            area.Connect(GlobalReferences.Signals.AREA_EXITED, this, nameof(OnExited));
        }

        public void OnEntered(Area2D pArea)
        {
            isCardIn = true;
        }
        public void OnExited(Area2D pArea)
        {
            isCardIn = false;
        }


        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}