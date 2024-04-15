using Com.IsartDigital.Jam.Managers;
using Com.IsartDigital.ProjectName;
using Com.IsartDigital.Shmup.Manager;
using Godot;
using System;
using System.Threading.Tasks;

namespace Com.IsartDigital.Jam.UI
{
	
    public class MainMenu : Node2D
    {
        [Export] NodePath startPath = default;
        [Export] Vector2 startScale = Vector2.One;
        [Export] NodePath optionPath = default;
        [Export] Vector2 optionScale = Vector2.One;
        [Export] NodePath creditPath = default;
        [Export] Vector2 creditScale = Vector2.One;
        [Export] NodePath exitPath = default;
        [Export] Vector2 exitScale = Vector2.One;

        [Export] private NodePath mainMenuCameraPath = null;
        public Camera2D mainMenuCamera;
        [Export] private NodePath creditsNodePath = null;
        public Control creditsNode;
        [Export] private NodePath tutoPath = null;
        public Node2D tuto;

        [Export] private NodePath mainCameraPath = null;
        [Export] private PackedScene creditsScene;
        public Camera2D mainCamera;

        private bool canOpening = true;

        public AudioStreamPlayer ambianceTavern;
        public AudioStreamPlayer ambianceTavernUI;

        [Export] private NodePath startAnimPath = null;
        private AnimationPlayer startAnim;

        #region Singleton
        static private MainMenu instance;
		
		static public MainMenu GetInstance () {
			if (instance == null) instance = new MainMenu();
		    return instance;

		}

        private MainMenu ():base() {}
        #endregion

        public override void _Ready()
        {
            #region Ready Singleton
            if (instance != null){  
                QueueFree();
                GD.Print(nameof(MainMenu) + " Instance already exist, destroying the last added.");
                return;
            }
            
            instance = this;
            #endregion

            ambianceTavern = SoundManager.GetInstance().PlayMusicSound(MusicName.AMBIANCE_TAVERN, SoundManager.GetInstance(), -45);
            ambianceTavernUI = SoundManager.GetInstance().PlayMusicSound(MusicName.AMBIANCE_TAVERN_UI, SoundManager.GetInstance());

            mainMenuCamera = GetNode<Camera2D>(mainMenuCameraPath);
            mainCamera = GetNode<Camera2D>(mainCameraPath);
            creditsNode = GetNode<Control>(creditsNodePath);
            tuto = GetNode<Node2D>(tutoPath);

            startAnim = GetNode<AnimationPlayer>(startAnimPath);
            startAnim.Connect("animation_finished", this, nameof(StartAnimEnded));
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }

        private void OnStartPressed()
        {
            ButtonEffect lButton = (ButtonEffect)GetNode(startPath);

            if (!lButton.isGrabbed)
            {
                SoundManager.GetInstance().PlaySfxSound(SfxName.CLICK_PLAY, 0, -20);
                SoundManager.GetInstance().FadeOutMusic(ambianceTavernUI, 40, 3);
                SoundManager.GetInstance().FadeInMusic(ambianceTavern, 40, 2);

                startAnim.Play("OpenningAnimation");
                LaunchTuto();
            }
        }
        private void LaunchStart()
        {
            Hide();
        }
        private void Nothing() 
        {

        }

        private void OnOptionsPressed()
        {
            ButtonEffect lButton = (ButtonEffect)GetNode(optionPath);
            if (!lButton.isGrabbed)
            {
                Settings.GetInstance().mainMenu = true;
                Visible = false;
                mainMenuCamera.Current = false;
                Settings.GetInstance().mainMenuCamera.Current = true;
                Settings.GetInstance().Visible = true;
		        SoundManager.GetInstance().PlaySfxSound(SfxName.CLICK, 0, -20);
            }

        }


        private void OnCreditPressed()
        {
            ButtonEffect lButton = (ButtonEffect)GetNode(creditPath);
            if (!lButton.isGrabbed)
            {
                SoundManager.GetInstance().PlaySfxSound(SfxName.CLICK, 0, -20);
                LaunchCredit();
            }
        }
        private void LaunchCredit()
        {
            creditsNode.AddChild(creditsScene.Instance());
        }


        private void OnExitPressed()
        {
            GetTree().Quit();
        }


        private void StartTween(TextureButton pButton,string pCallBack, Vector2 pScale)
        {
            //TouchButton lTouch = new TouchButton();
            //lTouch.StartTween(pButton, this, pCallBack, GetGlobalMousePosition(),pScale);
        }

        private void StartAnimEnded(string pAnimeName)
        {
            mainMenuCamera.Current = false;
            mainCamera.Current = true;
            Visible = false;
            GameManager.GetInstance().SpawnCards();
            GameManager.GetInstance().SpawnPlayer();
            GameManager.GetInstance().SpawnEnemy();

            GameManager.GetInstance().hpPlayerBar.Value = GameManager.GetInstance().hpPlayerBar.MaxValue;
            GameManager.GetInstance().animHpPlayer.Play("Spawn");
        }

        private async Task LaunchTuto()
        {
            await UtilsCoroutine.WaitForSeconds(3.2f);
            TutoManager.GetInstance().StartAnim();
        }
    }
}
