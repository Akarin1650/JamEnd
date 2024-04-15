using Com.IsartDigital.Jam.Cards;
using Com.IsartDigital.Jam.Managers;
using Com.IsartDigital.Jam.MyGameObjects.MyEntities;
using Com.IsartDigital.ProjectName;
using Godot;
using System;
using static GlobalReferences;

// Author : Alexis MARTIN
namespace Com.IsartDigital.Jam.MyGameObjects
{	
	public class Demon : Enemy
	{
        private string name = "OGRE";

        public Demon(int pHealth, int pStrength, int pMagic, int pEfficiency, int pArmor, int pCharisma, string pElement) :
                           base(pHealth, pStrength, pMagic, pEfficiency, pArmor, pCharisma, pElement)
        {

        }

        public Demon() : this(
                (int)GD.RandRange(25, 35),
                (int)GD.RandRange(2, 5),
                (int)GD.RandRange(6, 9),
                0,
                0,
                (int)GD.RandRange(5, 8),
                GameManager.GetInstance().RandomElement()
            )
        { }

        public override void _Process(float delta)
        {
            base._Process(delta);

            if (!isOpen && isEntered)
            {
                if (Input.IsActionJustReleased("LMB"))
                {
                    isOpen = true;
                    isEntered = false;

                    if (EntityManager.GetInstance().currentEnemy.attributes.Count == 1)
                        Card.GetInstance().SetPassportInfos(this, TranslationServer.Translate("ZOMBIE_DESCRIPTION"), TranslationServer.Translate("ZOMBIE_NAME"), Element, EntityManager.GetInstance().currentEnemy.attributes[0]);
                    else if (EntityManager.GetInstance().currentEnemy.attributes.Count == 2)
                        Card.GetInstance().SetPassportInfos(this, TranslationServer.Translate("ZOMBIE_DESCRIPTION"), TranslationServer.Translate("ZOMBIE_NAME"), Element, EntityManager.GetInstance().currentEnemy.attributes[0], EntityManager.GetInstance().currentEnemy.attributes[1]);

                    Card.GetInstance().ShowPassport();
                }
            }
        }

        protected override void OnEntered()
        {
            isEntered = true;
        }

    }
}