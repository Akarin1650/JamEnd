using Com.IsartDigital.Jam.Managers;
using Godot;
using System;

// Author : Baptiste VU LIEM
namespace Com.IsartDigital.Jam
{
	
	public class HomingSpawner : Node2D
	{
        static private HomingSpawner instance;

        static public HomingSpawner GetInstance()
        {
            if (instance == null) instance = new HomingSpawner();
            return instance;
        }

        private HomingSpawner() : base() { }

        [Export] private PackedScene homming;

        public override void _Ready()
        {
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(HomingSpawner) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;
        }

        public static void CreateHoming(Vector2 pStartPosition, Vector2 pTargetPosition)
        {
            Homing lHoming = (Homing)instance.homming.Instance();
            lHoming.GlobalPosition = pStartPosition;
            lHoming.targetPosition = pTargetPosition;
            instance.AddChild(lHoming);
        }

        public override void _Process(float delta)
        {
            Godot.Collections.Array lChildren = GetChildren();
            int length = lChildren.Count;
            for (int i = 0; i < length; i++)
                if (lChildren[i] is Particles2D)
                    if (!(lChildren[i] as Particles2D).Emitting) (lChildren[i] as Particles2D).QueueFree();

        }
    }
}