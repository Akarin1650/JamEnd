using Godot;
using System;

//Author : Julien Fournier
namespace Com.IsartDigital.ProjectName{
	
    public class TrashArea : Sprite
    {
        static private TrashArea instance;

        static public TrashArea GetInstance()
        {
            if (instance == null) instance = new TrashArea();
            return instance;
        }

        private TrashArea() : base() { }
        [Export] private NodePath areaPath;
        [Export] private NodePath animPath;
        public AnimationPlayer anim;


        public bool isCardIn;

        public Area2D area;
        public override void _Ready()
        {
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(TrashArea) + " Instance already exist, destroying the last added.");
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
            GD.Print("trash entered");
            isCardIn = true;
        }
        public void OnExited(Area2D pArea)
        {
            GD.Print("trash exited");
            isCardIn = false;
        }


        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}