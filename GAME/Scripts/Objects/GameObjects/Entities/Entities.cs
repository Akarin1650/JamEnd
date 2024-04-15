using Com.IsartDigital.Jam.Managers;
using Godot;
using System;

// Author : Baptiste Simon
namespace Com.IsartDigital.Jam.MyGameObjects.MyEntities
{

    public class Entities : Node2D
	{
        public int Health { get; set; }
        public int Strength { get; set; }
        public int Magic { get; set; }
        public float Efficiency { get; set; }
        public int Armor { get; set; }
        public int Charisma { get; set; }
        public string Element { get; set; }

        public Entities(int pHealth, int pStrength, int pMagic, float pEfficiency, int pArmor, int pCharisma, string pElement)
        {
            Health = pHealth;
            Strength = pStrength;
            Magic = pMagic;
            Efficiency = pEfficiency;
            Armor = pArmor;
            Charisma = pCharisma;
            Element = pElement;
        }
        public Entities() : this(0, 0, 0, 0, 0, 0, RandomElement()) { }

        private static string RandomElement()
        {
            Random lRandom = new Random();
            string[] lElements = { "Eau", "Feu", "Nature" };
            return lElements[lRandom.Next(lElements.Length)];
        }

        public virtual void SummonAttackEnemy(Entities pSummon, Entities pEnemy)
        {
            if (pEnemy.Health > 0)
            {
                GD.Print($"HEALTH ENEMY AVANT : {pEnemy.Health}");

                int lStrengthCheck = Math.Max(pSummon.Strength - pEnemy.Strength, 0);
                int lMagicCheck = Math.Max(pSummon.Magic - pEnemy.Magic, 0);
                int lCharismaCheck = Math.Max(pSummon.Charisma - pEnemy.Charisma, 0);

                int lStrengthDmg = Mathf.RoundToInt((lStrengthCheck) * pSummon.Efficiency);
                int lMagicDmg = Mathf.RoundToInt((lMagicCheck) * pSummon.Efficiency);
                int lCharismaDmg = Mathf.RoundToInt((lCharismaCheck) * pSummon.Efficiency);

                float lElementFactor = 1.0f;

                if ((pSummon.Element == "Eau" && pEnemy.Element == "Feu") ||
                    (pSummon.Element == "Feu" && pEnemy.Element == "Nature") ||
                    (pSummon.Element == "Nature" && pEnemy.Element == "Eau"))
                {
                    lElementFactor = 1.25f;
                }
                else if ((pSummon.Element == "Feu" && pEnemy.Element == "Eau") ||
                         (pSummon.Element == "Nature" && pEnemy.Element == "Feu") ||
                         (pSummon.Element == "Eau" && pEnemy.Element == "Nature"))
                {
                    lElementFactor = 0.75f;
                }

                lStrengthDmg = Mathf.Max(Mathf.RoundToInt(lStrengthDmg * lElementFactor), 0);
                lMagicDmg = Mathf.Max(Mathf.RoundToInt(lMagicDmg * lElementFactor), 0);
                lCharismaDmg = Mathf.Max(Mathf.RoundToInt(lCharismaDmg * lElementFactor), 0);

                pEnemy.Health -= lStrengthDmg;
                pEnemy.Health -= lMagicDmg;
                pEnemy.Health -= lCharismaDmg;

                GameManager.GetInstance().EnemyHpBarChange(GameManager.GetInstance().hpEnemyBar);

                GD.Print($"HEALTH ENEMY APRES : {pEnemy.Health} // Strength {lStrengthDmg} Magic {lMagicDmg} Charisma {lCharismaDmg}");

                if(pEnemy.Health <= 0)
                {
                    if (EntityManager.GetInstance().isEnemyCreeper)
                    {
                        GameManager.GetInstance().player.Health -= (10 - pSummon.Armor);
                    }

                    GD.Print("-------------------- Enemy is dead ------------------------");
                    UtilsSignals.GetInstance().EmitSignal(nameof(UtilsSignals.OnEnemyDead));
                    return;
                }

                UtilsSignals.GetInstance().EmitSignal(nameof(UtilsSignals.OnEnemyAttackPlayer));
            }
        }

        public virtual void EnemyAttackPlayer(Entities pPlayer, Entities pEnemy, Entities pSummon)
        {
            if (pPlayer.Health > 0 && pEnemy.Health > 0)
            {
                int lDamage = Math.Max(MSum(pEnemy) - pSummon.Armor, 0);
                pPlayer.Health -= lDamage;
                GD.Print("PLAYER HEALTH : " + pPlayer.Health);


                if (pPlayer.Health <= 0) GD.Print("You'Re Dead !");
                //emit signal here
            }
        }

        private int MSum(Entities pToSum)
        {
            int lSum = (pToSum.Strength + pToSum.Magic + pToSum.Charisma) / 3;
            return Mathf.RoundToInt(lSum);
        }

        protected override void Dispose(bool pDisposing)
		{

		}
	}
}
