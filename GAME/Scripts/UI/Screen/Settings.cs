using Godot;
using System;
using Com.IsartDigital.Jam.Managers;
using Com.IsartDigital.Shmup.Manager;

//Author : Louis Peyrat
namespace Com.IsartDigital.Jam.UI
{
	
    public class Settings : Node2D
    {
        [Export] NodePath startPath = default;
        [Export] Vector2 startScale = Vector2.One;
        [Export] NodePath optionPath = default;
        [Export] Vector2 optionScale = Vector2.One;
        [Export] NodePath creditPath = default;
        [Export] Vector2 creditScale = Vector2.One;
        [Export] NodePath backPath = default;
        [Export] Vector2 backScale = Vector2.One;
        [Export] private NodePath pathSliderMain;
        [Export] private NodePath mainMenuCameraPath = null;

        public bool mainMenu;
        public Camera2D mainMenuCamera;

        //bus
        private int masterBusIndex = AudioServer.GetBusIndex("Master");

        private bool canOpening = true;
        private HSlider mainSlider;
        private const string VALUE_CHANGED = "value_changed";

        #region Singleton
        static private Settings instance;

        static public Settings GetInstance()
        {
            if (instance == null) instance = new Settings();
            return instance;

        }

        private Settings() : base() { }
        #endregion

        public override void _Ready()
        {
            #region Ready Singleton
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(Settings) + " Instance already exist, destroying the last added.");
                return;
            }

            instance = this;
            #endregion
            AudioServer.SetBusVolumeDb(masterBusIndex, 6);           
            mainSlider = GetNode<HSlider>(pathSliderMain);
            mainMenuCamera = GetNode<Camera2D>(mainMenuCameraPath);
            mainSlider.Connect(VALUE_CHANGED, this, nameof(OnVFXValueChanged));           
            mainMenu = false;
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
        private void OnValueChanged(float pValue, int busIndex)
        {
            AudioServer.SetBusVolumeDb(busIndex, pValue);
            AudioServer.SetBusMute(busIndex, pValue == -30);
        }

        private void OnVFXValueChanged(float pValue)
        {
            OnValueChanged(pValue, masterBusIndex);
        }

        private void OnStartPressed()
        {
            ButtonEffect lButton = (ButtonEffect)GetNode(startPath);
            if (!lButton.isGrabbed)
            {
                OS.WindowFullscreen = !OS.WindowFullscreen;
            }
        }
        private void Nothing()
        {

        }
        private void OnExitPressed()
        {
            ButtonEffect lButton = (ButtonEffect)GetNode(backPath);
            if (!lButton.isGrabbed)
            {
                Visible = false;
                mainMenuCamera.Current = false;
                if (mainMenu)
                {                                      
                    MainMenu.GetInstance().mainMenuCamera.Current = true;
                    MainMenu.GetInstance().Visible = true;
                }
                else
                {
                    MainMenu.GetInstance().mainCamera.Current = true;
                }
                
            }
            
        }


        private void StartTween(TextureButton pButton, string pCallBack, Vector2 pScale)
        {
            //TouchButton lTouch = new TouchButton();
            //lTouch.StartTween(pButton, this, pCallBack, GetGlobalMousePosition(),pScale);
        }
    }
}