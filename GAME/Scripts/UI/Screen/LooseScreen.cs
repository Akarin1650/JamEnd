using Com.IsartDigital.Jam.Managers;
using Godot;
using System;

//Author : Baptiste SIMON
namespace Com.IsartDigital.Jam.UI{
	
    public class LooseScreen : Node
    {
        static private LooseScreen instance;

        private Label scoreLabel;
        [Export] private NodePath scorePath, bumpScorePath;

        private int currentScore = 0;
        public bool stop = false;


        private AnimationPlayer bumpScore;


        static public LooseScreen GetInstance () {
			if (instance == null) instance = new LooseScreen();
		    return instance;
		}

        private LooseScreen ():base() {}

        public override void _Ready()
        {
            if (instance != null){  
                QueueFree();
                GD.Print(nameof(LooseScreen) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;

            scoreLabel = (Label)GetNode(scorePath);
            bumpScore = (AnimationPlayer)GetNode(bumpScorePath);
        }

        private async void MakeScore(string pAnim)
        {
            float lSec = .4f;
            float lSecTwo = .01f;
            int lKills = GameManager.GetInstance().killCount - 1;
            while (currentScore <= (lKills - 1))
            {
                if (currentScore <= (lKills / 2))
                {
                    await UtilsCoroutine.WaitForSeconds(lSecTwo);
                    if (lSecTwo <= .15f)
                    {
                        lSecTwo += 0.02f;
                    }
                }
                else if (currentScore <= lKills - 4)
                {
                    await UtilsCoroutine.WaitForSeconds(.2f);

                }
                else
                {
                    await UtilsCoroutine.WaitForSeconds(lSec);
                    lSec += 0.27f;
                }

                currentScore++;
                scoreLabel.Text = currentScore.ToString();
            }

            stop = true;
            ScoreEnd();
        }

        private void ScoreEnd()
        {
            bumpScore.Play("bump");
        }

        private void OnPressed()
        {
            GetTree().Quit();
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}