using Godot;
using System;

//Author : Baptiste SIMON
namespace GAME.Scripts.Objects.GameObjects.Entities.Summon
{

    public class summonDefault : Node2D
    {
        [Export] private NodePath rectPath;

        private ColorRect rect;

        public override void _Ready()
        {
            rect = GetNode<ColorRect>(rectPath);
        }

        public override void _Process(float pDelta)
        {

        }

    }
}