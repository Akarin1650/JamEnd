using Com.IsartDigital.Jam.MyGameObjects;
using Com.IsartDigital.Jam.MyGameObjects.MyEntities;
using Com.IsartDigital.Jam.UI;
using Com.IsartDigital.ProjectName;
using Com.IsartDigital.Shmup.Manager;
using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

//Author : Baptiste SIMON
namespace Com.IsartDigital.Jam.Managers{
	
    public class GameManager : Node2D
    {
        public Entities player = new Entities(pHealth: 40, 0, 0, 0, 0, 0, "null");

        static private GameManager instance;
		
		static public GameManager GetInstance () {
			if (instance == null) instance = new GameManager();
		    return instance;
		}

        [Export] public PackedScene looseScreen;

        private GameManager ():base() {}
        [Export] public List<PackedScene> cardList;
        [Export] public PackedScene playerScene;
        public List<Node2D> cardPosList;
        public Class currentSummon;

        [Export] private NodePath hpPlayerBarPath;
        public TextureProgress hpPlayerBar;
        
        [Export] private NodePath hpEnemyBarPath;
        public TextureProgress hpEnemyBar;

        [Export] private NodePath animHpPlayerPath;
        public AnimationPlayer animHpPlayer;
        
        [Export] private NodePath animHpEnemyPath;
        public AnimationPlayer animHpEnemy;


        [Export] private NodePath looseScreenPointPath;
        [Export] private NodePath spawnEnemyPath;
        [Export] private NodePath spawnPlayerPath;

        [Export] private NodePath cardOnePosPath;
        [Export] private NodePath cardTwoPosPath;
        [Export] private NodePath cardThreePosPath;
        [Export] private NodePath cardFourPosPath;
        [Export] private NodePath cardFivePosPath;

        [Export] public PackedScene zombieFacotry;
        [Export] public PackedScene squeletonFacotry;
        [Export] public PackedScene witcherFacotry;
        [Export] public PackedScene ogreFacotry;
        [Export] public PackedScene dragonFacotry;

        public Node2D looseScreenPoint;
        private Node2D cardOnePos;
        private Node2D cardTwoPos;
        private Node2D cardThreePos;
        private Node2D cardFourPos;
        private Node2D cardFivePos;

        public Node2D spawnEnemy;
        private Node2D spawnPlayer;

        public Vector2 screenSize;

        public bool isDiscarding;

        public RandomNumberGenerator rand = new RandomNumberGenerator();

        public int nMoves = 0;
        public bool halfTurn = false;

        public bool needToDiscard = true;
        public bool discardAllCard = false;

        private bool isDead = false;

        public int killCount = 0;

	    //Settings button
        [Export] private NodePath settingButtonPath;
        private Button settingsButton;

        public override void _EnterTree()
        {
            if (instance != null){  
                QueueFree();
                GD.Print(nameof(GameManager) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;
            rand.Randomize();

            screenSize = GetViewportRect().Size;

            spawnEnemy = GetNode<Node2D>(spawnEnemyPath);
            spawnPlayer = GetNode<Node2D>(spawnPlayerPath);

            looseScreenPoint = GetNode<Node2D>(looseScreenPointPath);
            cardOnePos = GetNode<Node2D>(cardOnePosPath);
            cardTwoPos = GetNode<Node2D>(cardTwoPosPath);
            cardThreePos = GetNode<Node2D>(cardThreePosPath);
            cardFourPos = GetNode<Node2D>(cardFourPosPath);
            cardFivePos = GetNode<Node2D>(cardFivePosPath);
            settingsButton = GetNode<Button>(settingButtonPath);

            hpEnemyBar = GetNode<TextureProgress>(hpEnemyBarPath);
            hpPlayerBar = GetNode<TextureProgress>(hpPlayerBarPath);

            animHpEnemy = GetNode<AnimationPlayer>(animHpEnemyPath);
            animHpPlayer = GetNode<AnimationPlayer>(animHpPlayerPath);

            cardPosList = new List<Node2D>() { cardOnePos, cardTwoPos, cardThreePos, cardFourPos, cardFivePos };

            settingsButton.Connect("pressed", this, nameof(OnSettingsButtonPressed));
            UtilsSignals.GetInstance().Connect(nameof(UtilsSignals.OnCardSelected), this, nameof(SetCurrentSummon), new Godot.Collections.Array { });
            UtilsSignals.GetInstance().Connect(nameof(UtilsSignals.OnStartFight), this, nameof(LaunchFight));
            UtilsSignals.GetInstance().Connect(nameof(UtilsSignals.OnEnemyDead), this, nameof(EnemyIsDead));
            UtilsSignals.GetInstance().Connect(nameof(UtilsSignals.OnEnemyAttackPlayer), this, nameof(SetDamageToPlayer));
        }

        public override void _Process(float pDelta)
        {
            
        }
        public string RandomElement()
        {
            Random lRandom = new Random();
            string[] lElements = { "Eau", "Feu", "Nature" };
            return lElements[lRandom.Next(lElements.Length)];
        }

        public async void SpawnCards()
        {
            await UtilsCoroutine.WaitForSeconds(0.01f);
            SoundManager.GetInstance().PlaySfxSound(SfxName.CARD_DRAW_AMBIANCE, 0, -3);

            for (int i = 0; i < cardPosList.Count; i++)
            {
                if (cardPosList[i].GetChildCount() == 0)
                {
                    await UtilsCoroutine.WaitForSeconds(0.1f);
                    SoundManager.GetInstance().PlaySfxSound(SfxName.CARD_DRAW, rand.RandiRange(0, SoundManager.GetInstance().cardDrawList.Count - 1), 10);
                    cardPosList[i].AddChild(cardList[rand.RandiRange(0, cardList.Count-1)].Instance());
                }
            }

            if (isDead)
            {
                isDead = false;
                SpawnEnemy();
            }
        }

        public void SpawnEnemy()
        {
            EntityManager.GetInstance().WaveHandler();

            killCount++;
        }

        public void SpawnPlayer()
        {
            spawnPlayer.AddChild(playerScene.Instance());
        }

        public void SetDamageToPlayer()
        {
            EntityManager.GetInstance().SetEnemyAttribute(EntityManager.GetInstance().setEnemy, EntityManager.GetInstance().currentEnemy);
            player.EnemyAttackPlayer(player, EntityManager.GetInstance().currentEnemy, currentSummon);

            PlayerHpBarChange(hpPlayerBar);
        }

        private void SetCurrentSummon(Class pClass)
        {
            currentSummon = pClass;
        }

        private async void SetModeDiscard()
        {
            await UtilsCoroutine.WaitForSeconds(2f);
            TrashArea.GetInstance().Visible = true;
            TrashArea.GetInstance().anim.Play("Spawn");
            isDiscarding = true;
        }

        private void LaunchFight()
        {
            PlayEnemySoundAsync(EntityManager.GetInstance().currentEnemy);

            player.SummonAttackEnemy(currentSummon, EntityManager.GetInstance().currentEnemy);

            AttackAnimation();


            SoundManager.GetInstance().PlaySfxSound(SfxName.ATTACK_WATER, 0, 0);

            if (needToDiscard && !discardAllCard)
            {
                SetModeDiscard();
            }
            else
            {
                //SpawnCards();
            }

            nMoves++;

            if(nMoves % 2 == 0)
            {
                halfTurn = true;
            }
        }

        private void AttackAnimation()
        {
            int lAttackNmb = 10;
            for (int i = 0; i < lAttackNmb; i++)
                HomingSpawner.CreateHoming(spawnPlayer.GlobalPosition, spawnEnemy.GlobalPosition);
        }

        private void EnemyIsDead()
        {
            needToDiscard = true;
            discardAllCard = false;
            spawnEnemy.GetChild(spawnEnemy.GetChildren().Count - 1).QueueFree();
            animHpEnemy.PlayBackwards("Spawn");
            SpawnEnemy();
        }

        public void DiscardAll()
        {
            if (discardAllCard)
            {
                for (int i = 0; i < cardPosList.Count; i++)
                {
                    if (cardPosList[i].GetChildCount() == 1)
                    {
                        cardPosList[i].GetChild(0).QueueFree();
                    }
                }
                TrashArea.GetInstance().isCardIn = false;
                isDiscarding = false;
                SpawnCards();
                TrashArea.GetInstance().Visible = false;
            }

            nMoves++;

            if(nMoves % 2 == 0)
            {
                halfTurn = true;
            }
        }
        public void EnemyHpBarChange(TextureProgress pBar)
        {
            Tween lTween = new Tween();
            pBar.AddChild(lTween);
            lTween.InterpolateProperty(pBar, "value", pBar.Value, EntityManager.GetInstance().currentEnemy.Health, 0.5f, Tween.TransitionType.Linear, Tween.EaseType.InOut);
            lTween.Start();
        }
        public void PlayerHpBarChange(TextureProgress pBar)
        {
            Tween lTween = new Tween();
            pBar.AddChild(lTween);
            lTween.InterpolateProperty(pBar, "value", pBar.Value, player.Health, 0.5f, Tween.TransitionType.Linear, Tween.EaseType.InOut);
            lTween.Start();
        }

        private void OnSettingsButtonPressed()
        {
            
        }

        private async void PlayEnemySoundAsync(Enemy pEnemyType) 
        {
            await UtilsCoroutine.WaitForSeconds(0.8f);

            if (pEnemyType is Dragon)
            {
                //SoundManager.GetInstance().PlaySfxSound(SfxName.ATTACK_DRAGON, 0, 0);
                SoundManager.GetInstance().PlaySfxSound(SfxName.HURT_DRAGON, 0, 0);
            }
            if (pEnemyType is Demon)
            {
                //SoundManager.GetInstance().PlaySfxSound(SfxName.ATTACK_OGRE, 0, 0);
                SoundManager.GetInstance().PlaySfxSound(SfxName.HURT_OGRE, 0, 0);
            }
            if (pEnemyType is Squelette)
            {
                //SoundManager.GetInstance().PlaySfxSound(SfxName.ATTACK_SKELETON, 0, 0);
                SoundManager.GetInstance().PlaySfxSound(SfxName.SKELETON_HURT, 0, 0);
            }
            if (pEnemyType is Sorcier)
            {
                //SoundManager.GetInstance().PlaySfxSound(SfxName.ATTACK_WITCHER, 0, 0);
                SoundManager.GetInstance().PlaySfxSound(SfxName.HURT_WITCHER, 0, 0);
            }
            if (pEnemyType is Zombie)
            {
                //SoundManager.GetInstance().PlaySfxSound(SfxName.ATTACK_ZOMBIE, 0, 0);
                SoundManager.GetInstance().PlaySfxSound(SfxName.ZOMBIE_HURT, 0, 0);
            }
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}
