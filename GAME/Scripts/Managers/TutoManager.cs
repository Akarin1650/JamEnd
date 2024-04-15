using Godot;
using System;

//Author : Killian Ferreira Da Costa
namespace Com.IsartDigital.ProjectName{
	
    public class TutoManager : Node2D
    {
        #region singleton
        static private TutoManager instance;
		
		static public TutoManager GetInstance () {
			if (instance == null) instance = new TutoManager();
		    return instance;
		}

        private TutoManager ():base() {}
        #endregion

        [Export] private NodePath nextButtonPath = null;
        [Export] private NodePath closeButtonPath = null;
        private TextureButton nextButton;
        private TextureButton closeButton;

        [Export] private NodePath firstTutoPath = null;
        [Export] private NodePath secondTutoPath = null;
        private Node2D firstTuto;
        private Node2D secondTuto;

        [Export] private NodePath animationPath = null;
        private AnimationPlayer animation;
        [Export] private NodePath quitAnimationPath = null;
        private AnimationPlayer quitAnimation;

        public override void _Ready()
        {
            #region singleton ready
            if (instance != null){  
                QueueFree();
                GD.Print(nameof(TutoManager) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;
            #endregion

            nextButton = GetNode<TextureButton>(nextButtonPath);
            nextButton.Connect("pressed", this, nameof(NextPanel));
            closeButton = GetNode<TextureButton>(closeButtonPath);
            closeButton.Connect("pressed", this, nameof(QuitTuto));
            firstTuto = GetNode<Node2D>(firstTutoPath);
            secondTuto = GetNode<Node2D>(secondTutoPath);
            animation = GetNode<AnimationPlayer>(animationPath);
            quitAnimation = GetNode<AnimationPlayer>(quitAnimationPath);
        }

        public override void _Process(float pDelta)
        {

        }

        private void NextPanel()
        {
            firstTuto.Hide();
            secondTuto.Show();
            animation.Play("SecondPanelApparition");
        }

        private void QuitTuto()
        {
            quitAnimation.Play("QuitAnimation");
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }

        public void StartAnim()
        {
            animation.Play("TutoApparition");
        }
    }
}