using Com.IsartDigital.Shmup.Manager;
using Godot;
using System;
using System.Linq;

// Author : Killian Ferreira Da Costa
namespace Com.IsartDigital.Jam.UI
{
	
	public class ButtonEffect : TextureButton
	{
        // Lights Settings
        [Export] private float lightsGlowFrequency = 0.1f;
        [Export] private float lightsGlowMinIntensity = 1.3f;
        [Export] private float lightsGlowMaxIntensity = 2.3f;
        [Export] private int spaceBetweenLights = 3;

        [Export] private float grabDelay = 0.2f;
        [Export] private float unGrabDelay = 0.2f;


        // NodePath
        [Export] private NodePath physicBodyPath = null;
        private RigidBody2D physicBody;
         [Export] private NodePath lockedPath = null;
        private Sprite locked;

        // Other
        private const string MOUSE_ENTERED = "mouse_entered";
		private const string MOUSE_EXITED = "mouse_exited";
        private const string TIMEOUT = "timeout";
        private int currentLightIndex = 0;
		private Timer grabTimer = new Timer();
		private Timer unGrabTimer = new Timer();
        private Vector2 grabOffset;
        private bool isOverred = false;
        public bool isGrabbed = false;


        public override void _Ready()
		{
            if (physicBodyPath != null)
            {
                physicBody = GetNode<RigidBody2D>(physicBodyPath);
            }
            if (lockedPath != null)
            {
                locked = GetNode<Sprite>(lockedPath);
            }

            Connect(MOUSE_ENTERED, this, nameof(OnButtonSelected));
			Connect(MOUSE_EXITED, this, nameof(OnButtonUnselected));

            AddChild(grabTimer);
            grabTimer.Connect(TIMEOUT, this, nameof(GrabDelay));
            grabTimer.WaitTime = grabDelay;
            grabTimer.OneShot = true;

            AddChild(unGrabTimer);
            unGrabTimer.Connect(TIMEOUT, this, nameof(UnGrabDelay));
            unGrabTimer.WaitTime = unGrabDelay;
            unGrabTimer.OneShot = true;
        }

        private void OnButtonSelected()
		{
            isOverred = true;
            SoundManager.GetInstance().PlaySfxSound(SfxName.HIGHLIGHT_BUTTON, 0, 0);
        }

        private void OnButtonUnselected()
        {
            isOverred = false;
        }

        private void GlowLight()
        {

        }

        private void GrabDelay() { isGrabbed = true; }
        private void UnGrabDelay() { isGrabbed = false; }

        public override void _Input(InputEvent @event)
        {
            if (physicBodyPath != null)
            {
                if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == (int)ButtonList.Left)
                {
                    if (mouseEvent.Pressed)
                    {
                        Vector2 mousePosition = GetGlobalMousePosition();
                        if (isOverred)
                        {
                            grabOffset = mousePosition - physicBody.GlobalPosition;
                            physicBody.Mode = RigidBody2D.ModeEnum.Kinematic;
                            grabTimer.Start();
                        }
                    }
                    else
                    {
                        physicBody.Mode = RigidBody2D.ModeEnum.Rigid;
                        unGrabTimer.Start();
                    }
                }
            }
        }

        public override void _Process(float delta)
        {
            if (physicBodyPath != null)
            {
                if (physicBody.Mode == RigidBody2D.ModeEnum.Kinematic)
                {
                    physicBody.GlobalPosition = GetGlobalMousePosition() - grabOffset;
                }
            }
        }
    }
}