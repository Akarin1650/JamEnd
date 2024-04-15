using Com.IsartDigital.Jam.Cards;
using Com.IsartDigital.Jam.MyGameObjects;
using Com.IsartDigital.Jam.MyGameObjects.MyEntities;
using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Xml.Linq;

//Author : Baptiste SIMON
namespace Com.IsartDigital.Jam.Managers
{
    public class EntityManager : Node2D
    {

        public class InitialEnemyAttributes
        {
            public int iStrength { get; set; }
            public int iMagic { get; set; }
            public int iCharisma { get; set; }
        }

        private InitialEnemyAttributes initialEnemyAttributes;

        private string description;
        private string name;
        private PackedScene currentSceneEnemy;

        private int enemyCount = 0;
        private int bossCount = 0;

        public Enemy currentEnemy;
        public SetClass setEnemy;

        public bool isEnemyCreeper = false;
        public bool damageWhenDiscard = false;

        public List<string> enemyText = new List<string>();
        public List<string> summonText = new List<string>();

        #region SINGLETON
        static private EntityManager instance;
		
		static public EntityManager GetInstance () {
			if (instance == null) instance = new EntityManager();
		    return instance;
		}
        #endregion

        private EntityManager ():base() {}

        public override void _EnterTree()
        {
            GD.Randomize();

            #region SINGLETON
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(EntityManager) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;
            #endregion
        }

        #region ATTRIBUTES
        public void SetSummonAttribute(SetClass pSetClass, Class pClass)
        {
            foreach (KeyValuePair<string, string> lAttribute in pSetClass.SelectedAttributes)
            {
                string lKey = lAttribute.Key;
                string lValue = lAttribute.Value;

                switch (lKey)
                {
                    case "-2:strength":
                        pClass.Strength -= 2;
                        summonText.Add(lValue);

                        break;
                    case "-2:magic":
                        pClass.Magic -= 2;
                        summonText.Add(lValue);

                        break;
                    case "+2:armor":
                        pClass.Armor += 2;
                        summonText.Add(lValue);

                        break;
                    case "-2:efficienty":
                        pClass.Efficiency -= 2;
                        summonText.Add(lValue);

                        break;
                    case "-2:charism":
                        pClass.Charisma -= 2;
                        summonText.Add(lValue);

                        break;
                    case "-1:strength, -1:efficienty":
                        pClass.Strength -= 1;
                        pClass.Efficiency -= 1;
                        summonText.Add(lValue);

                        break;
                    case "+2:strength, -2:magic":
                        pClass.Strength += 2;
                        pClass.Magic -= 2;
                        summonText.Add(lValue);

                        break;
                    case "-2:charism, -2:efficienty":
                        pClass.Charisma -= 2;
                        pClass.Efficiency -= 2;
                        summonText.Add(lValue);

                        break;
                    case "-2:armor, +2:charism":
                        pClass.Armor -= 2;
                        pClass.Charisma += 2;
                        summonText.Add(lValue);

                        break;
                    case "-2:armor, +2:magic":
                        pClass.Armor -= 2;
                        pClass.Magic += 2;
                        summonText.Add(lValue);

                        break;
                    case "roll magic, strength, charism":
                        int lRandA = (int)GD.RandRange(1, 10);
                        int lRandB = (int)GD.RandRange(1, 10);
                        int lRandC = (int)GD.RandRange(1, 10);

                        pClass.Strength = lRandA;
                        pClass.Charisma = lRandB;
                        pClass.Magic = lRandC;

                        summonText.Add(lValue);

                        break;

                    case "folie":
                        //quand on discard on a le choix entre deux carte, on en pick une
                        pClass.Armor = 0;

                        int lMainAttriubte = Math.Max(Math.Max(pClass.Strength, pClass.Magic), pClass.Charisma);

                        if (lMainAttriubte == pClass.Magic)
                        {
                            pClass.Magic = Mathf.RoundToInt(pClass.Magic * 1.4f);
                        }
                        else if (lMainAttriubte == pClass.Charisma)
                        {
                            pClass.Charisma = Mathf.RoundToInt(pClass.Charisma * 1.4f);
                        }
                        else if (lMainAttriubte == pClass.Strength)
                        {
                            pClass.Strength = Mathf.RoundToInt(pClass.Strength * 1.4f);
                        }


                        summonText.Add(lValue);
                        break;

                    case "win or loose":
                        int lIndex = GameManager.GetInstance().rand.RandiRange(0, 1);
                        if (lIndex == 0)
                        {
                            pClass.Strength += currentEnemy.Strength;
                            pClass.Magic += currentEnemy.Magic;
                            pClass.Charisma += currentEnemy.Charisma;
                        }
                        else if(lIndex == 1)
                        {
                            pClass.Strength -= currentEnemy.Strength;
                            pClass.Magic -= currentEnemy.Magic;
                            pClass.Charisma -= currentEnemy.Charisma;
                        }

                        summonText.Add(lValue);
                        break;

                    default:
                        break;
                }
            }
        }
        public void SetEnemyAttribute(SetClass pSetClass, Class pClass)
        {
            if (initialEnemyAttributes is null)
            {
                initialEnemyAttributes = new InitialEnemyAttributes
                {
                    iStrength = pClass.Strength,
                    iMagic = pClass.Magic,
                    iCharisma = pClass.Charisma
                };
            }

            foreach (KeyValuePair<string, string> lAttribute in pSetClass.SelectedAttributes)
            {
                string lKey = lAttribute.Key;
                string lValue = lAttribute.Value;

                switch (lKey)
                {
                    case "+2 strength, +4 efficienty":
                        pClass.Strength = initialEnemyAttributes.iStrength - 2;
                        pClass.Strength = (int)Math.Round(initialEnemyAttributes.iStrength * 0.6f);
                        pClass.Charisma = (int)Math.Round(initialEnemyAttributes.iCharisma * 0.6f);
                        pClass.Magic = (int)Math.Round(initialEnemyAttributes.iCharisma * 0.6f);

                        enemyText.Add(lValue);

                        break;
                    case "+2 magic":
                        pClass.Magic = initialEnemyAttributes.iMagic - 2;
                        enemyText.Add(lValue);

                        break;
                    case "1/2 *1.5 damage":

                        if (GameManager.GetInstance().halfTurn)
                        {
                            pClass.Strength = (int)Math.Round(initialEnemyAttributes.iStrength * 1.5f);
                            pClass.Magic = (int)Math.Round(initialEnemyAttributes.iMagic * 1.5f);
                            pClass.Charisma = (int)Math.Round(initialEnemyAttributes.iCharisma * 1.5f);
                        }
                        else
                        {
                            pClass.Strength = initialEnemyAttributes.iStrength;
                            pClass.Magic = initialEnemyAttributes.iMagic;
                            pClass.Charisma = initialEnemyAttributes.iCharisma;
                        }

                        enemyText.Add(lValue);

                        break;
                    case "+2 strength, +2 charism":
                        pClass.Strength = initialEnemyAttributes.iStrength - 2;
                        pClass.Charisma = initialEnemyAttributes.iCharisma - 2;
                        enemyText.Add(lValue);

                        break;
                    case "-2 charism":
                        pClass.Charisma = initialEnemyAttributes.iCharisma - 2;
                        enemyText.Add(lValue);

                        break;
                    case "-10 magic":
                        pClass.Magic = initialEnemyAttributes.iMagic + 7;
                        enemyText.Add(lValue);

                        break;
                    case "-10 charism":
                        pClass.Charisma = initialEnemyAttributes.iCharisma + 7;
                        enemyText.Add(lValue);

                        break;
                    case "-10 strength":
                        pClass.Strength = initialEnemyAttributes.iStrength + 7;
                        enemyText.Add(lValue);

                        break;
                    case "+1 Damage per turn":
                        pClass.Strength += 1;
                        pClass.Charisma += 1;
                        pClass.Magic += 1;

                        enemyText.Add(lValue);

                        break;
                    case "+2 armor":
                        pClass.Charisma = initialEnemyAttributes.iStrength + 2;
                        pClass.Strength = initialEnemyAttributes.iStrength + 2;
                        pClass.Magic = initialEnemyAttributes.iStrength + 2;
                        enemyText.Add(lValue);

                        break;
                    case "-1damage Taken":
                        pClass.Strength = initialEnemyAttributes.iStrength + 1;
                        pClass.Magic = initialEnemyAttributes.iMagic + 1;
                        pClass.Charisma = initialEnemyAttributes.iCharisma + 1;

                        enemyText.Add(lValue);

                        break;
                    case "+2 efficienty":
                        pClass.Strength = (int)Math.Round(initialEnemyAttributes.iStrength * 0.85f);
                        pClass.Charisma = (int)Math.Round(initialEnemyAttributes.iCharisma * 0.85f);
                        pClass.Magic = (int)Math.Round(initialEnemyAttributes.iCharisma * 0.85f);

                        enemyText.Add(lValue);

                        break;
                    case "don't discard when alive":
                        GameManager.GetInstance().needToDiscard = false;
                        enemyText.Add(lValue);

                        break;
                    case "all discard when alive":
                        GameManager.GetInstance().discardAllCard = true;
                        enemyText.Add(lValue);

                        break;

                    default:
                        break;
                }
            }
        }

        #endregion


        public void WaveHandler()
        {

            if (enemyCount == 0 && GameManager.GetInstance().hpPlayerBar.Value != 55)
            {
                GameManager.GetInstance().player.Health = 55;
                GameManager.GetInstance().hpPlayerBar.Value = 55f;
                GameManager.GetInstance().PlayerHpBarChange(GameManager.GetInstance().hpPlayerBar);
            }

            enemyCount++;
            enemyText.Clear();

            if(enemyCount < GlobalReferences.IndexReferences.ENEMIES_PER_WAVE)
            {
                CreateEnemy();
                return;
            }

            CreateBoss();
            enemyCount = 0;
        }

        public async void CreateEnemy()
        {
            int lNAttributes;

            int lEnemyType = GameManager.GetInstance().rand.RandiRange(0, 2);
            if (lEnemyType == 0)
            {
                currentEnemy = new Squelette();
                description = "SQUELETON_DESCRIPTION";
                name = "SQUELETON_NAME";
                currentSceneEnemy = GameManager.GetInstance().squeletonFacotry;
            }
            else if (lEnemyType == 1)
            {
                currentEnemy = new Zombie();
                description = "ZOMBIE_DESCRIPTION";
                name = "ZOMBIE_NAME";
                currentSceneEnemy = GameManager.GetInstance().zombieFacotry;
            }
            else
            {
                currentEnemy = new Sorcier();
                description = "WITCHER_DESCRIPTION";
                name = "WITCHER_NAME";
                currentSceneEnemy = GameManager.GetInstance().witcherFacotry;
            }


            int lNumber = GameManager.GetInstance().rand.RandiRange(0, 2);
            if(lNumber == 0)
            {
                lNAttributes = 1;
            }
            else if(lNumber == 1)
            {
                lNAttributes = 1;
            }
            else
            {
                lNAttributes = 2;
            }

            setEnemy = new SetClass(currentEnemy, lNAttributes);

            SetEnemyAttribute(setEnemy, currentEnemy);

            for (int i = 0; i < enemyText.Count; i++)
            {
                currentEnemy.attributes.Add(enemyText[i]);
            }


            GameManager.GetInstance().hpEnemyBar.MaxValue = currentEnemy.Health;
            GameManager.GetInstance().hpEnemyBar.Value = currentEnemy.Health;
            GameManager.GetInstance().EnemyHpBarChange(GameManager.GetInstance().hpEnemyBar);
            await UtilsCoroutine.WaitForSeconds(0.5f);
            GameManager.GetInstance().spawnEnemy.AddChild(currentSceneEnemy.Instance());
            GameManager.GetInstance().animHpEnemy.Play("Spawn");
            await UtilsCoroutine.WaitForSeconds(0.5f);

            //if (lNAttributes == 1)
            //    Card.GetInstance().SetPassportInfos(currentEnemy, TranslationServer.Translate(description), name, currentEnemy.Element, enemyText[0]) ;
            //else if (lNAttributes == 2)
            //    Card.GetInstance().SetPassportInfos(currentEnemy, TranslationServer.Translate(description), name, currentEnemy.Element, enemyText[0], enemyText[1]);

            //Card.GetInstance().ShowPassport();
        }

        public async void CreateBoss()
        {
            int lNAttribute;

            if(bossCount == 0)
            {
                int lEnemyType = GameManager.GetInstance().rand.RandiRange(0, 1);
                if (lEnemyType == 0)
                {
                    currentEnemy = new Dragon();
                    name = "DRAGON_NAME";
                    description = "DRAGON_DESCRIPTION";
                    currentSceneEnemy = GameManager.GetInstance().dragonFacotry;
                }
                else if (lEnemyType == 1)
                {
                    currentEnemy = new Demon();
                    name = "OGRE_NAME";
                    description = "OGRE_DESCRIPTION";
                    currentSceneEnemy = GameManager.GetInstance().ogreFacotry;
                }

                lNAttribute = 2;
                setEnemy = new SetClass(currentEnemy, lNAttribute);
                bossCount++;
            }
            else
            {
                int lEnemyType = (int)GD.RandRange(0, 1);
                if (lEnemyType == 0)
                {
                    currentEnemy = new Dragon();
                    name = "DRAGON_NAME";
                    description = "DRAGON_DESCRIPTION";
                    currentSceneEnemy = GameManager.GetInstance().dragonFacotry;
                }
                else if (lEnemyType == 1)
                {
                    currentEnemy = new Demon();
                    name = "OGRE_NAME";
                    description = "OGRE_DESCRIPTION";
                    currentSceneEnemy = GameManager.GetInstance().ogreFacotry;
                }

                lNAttribute = 3;
                setEnemy = new SetClass(currentEnemy, lNAttribute);
            }
            
            SetEnemyAttribute(setEnemy, currentEnemy);

            for (int i = 0; i < enemyText.Count; i++)
            {
                currentEnemy.attributes.Add(enemyText[i]);
            }

            GameManager.GetInstance().hpEnemyBar.MaxValue = currentEnemy.Health;
            GameManager.GetInstance().hpEnemyBar.Value = currentEnemy.Health;
            GameManager.GetInstance().EnemyHpBarChange(GameManager.GetInstance().hpEnemyBar);
            await UtilsCoroutine.WaitForSeconds(0.5f);
            GameManager.GetInstance().spawnEnemy.AddChild(currentSceneEnemy.Instance());
            GameManager.GetInstance().animHpEnemy.Play("Spawn");
            await UtilsCoroutine.WaitForSeconds(0.5f);
            //if (lNAttribute == 2)
            //    Card.GetInstance().SetPassportInfos(currentEnemy, TranslationServer.Translate(description), name, currentEnemy.Element, enemyText[0], enemyText[1]);
            //else if (lNAttribute == 3)
            //    Card.GetInstance().SetPassportInfos(currentEnemy, TranslationServer.Translate(description), name, currentEnemy.Element, enemyText[0], enemyText[1], enemyText[3]);

            //Card.GetInstance().ShowPassport();
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}
