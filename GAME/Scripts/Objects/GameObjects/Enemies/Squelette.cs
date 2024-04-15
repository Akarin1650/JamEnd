using Com.IsartDigital.Jam.Managers;
using Com.IsartDigital.Jam.Cards;
using Com.IsartDigital.Jam.MyGameObjects.MyEntities;
using Com.IsartDigital.ProjectName;
using Godot;
using System;

// Author : Alexis MARTIN
namespace Com.IsartDigital.Jam.MyGameObjects
{	
	public class Squelette : Enemy
	{
        public Squelette(int pHealth, int pStrength, int pMagic, int pEfficiency, int pArmor, int pCharisma, string pElement) :
                           base(pHealth, pStrength, pMagic, pEfficiency, pArmor, pCharisma, pElement)
        {

        }
        public Squelette() : this(
                (int)GD.RandRange(6, 11),
                (int)GD.RandRange(5, 9),
                (int)GD.RandRange(0, 2),
                0,
                0,
                (int)GD.RandRange(0, 3),
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
