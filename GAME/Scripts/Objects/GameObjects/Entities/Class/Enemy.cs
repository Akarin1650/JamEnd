using Com.IsartDigital.Jam.Cards;
using Com.IsartDigital.Jam.Managers;
using Godot;
using System;
using System.Collections.Generic;
using static GlobalReferences;

// Author : Baptiste Simon
namespace Com.IsartDigital.Jam.MyGameObjects.MyEntities {
	
	public class Enemy : Class
	{
        [Export] protected NodePath areaPath;
        protected Area2D area2D;

        public List<string> attributes = new List<string> ();

        public bool isOpen;
        public bool isEntered;

        public Enemy(int pHealth, int pStrength, int pMagic, int pEfficiency, int pArmor, int pCharisma, string pElement) :
                        base(pHealth, pStrength, pMagic, pEfficiency, pArmor, pCharisma, pElement)
        {

        }
        public Enemy() : this(8, 5, 5, 0, 0, 5, GameManager.GetInstance().RandomElement()) { }

        public override void _Ready()
        {
            base._Ready();
            GD.Print("ok");
            area2D = GetNode<Area2D>(areaPath);
            area2D.Connect("mouse_entered", this, nameof(OnEntered));
        }

        public override void _Process(float delta)
        {
            if(Card.GetInstance().Visible && isOpen) isOpen = false;
        }

        protected virtual void OnEntered()
        {
            
        }

        public override Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>
        {
            {"+2 strength, +4 efficienty", "ATTRIBUTES_ENEMY_0" },
            {"+2 magic", "ATTRIBUTES_ENEMY_1" },
            {"1/2 *1.5 damage", "ATTRIBUTES_ENEMY_2" },
            {"+2 strength, +2 charism", "ATTRIBUTES_ENEMY_3" },
            {"-2 charism", "ATTRIBUTES_ENEMY_4" },
            {"-10 magic", "ATTRIBUTES_ENEMY_5" },
            {"when die make 10damage", "ATTRIBUTES_ENEMY_6" },
            {"-10 charism", "ATTRIBUTES_ENEMY_7" },
            {"-10 strength", "ATTRIBUTES_ENEMY_8" },
            {"+1 Damage per turn", "ATTRIBUTES_ENEMY_9" },
            {"+2 armor", "ATTRIBUTES_ENEMY_10" },
            {"-1damage Taken", "ATTRIBUTES_ENEMY_11" },
            {"+2 efficienty", "ATTRIBUTES_ENEMY_12" },
            {"don't discard when alive", "ATTRIBUTES_ENEMY_13" },
            {"all discard when alive", "ATTRIBUTES_ENEMY_14" },
        };
    }
}