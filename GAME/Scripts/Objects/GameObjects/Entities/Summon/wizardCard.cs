using Com.IsartDigital.Jam.Managers;
using Com.IsartDigital.Jam.MyGameObjects;
using Com.IsartDigital.Jam.MyGameObjects.MyEntities;
using Com.IsartDigital.ProjectName;
using Com.IsartDigital.Shmup.Manager;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.IsartDigital.Jam.Cards
{
    public class wizardCard : TextureButton
    {
        [Export] private NodePath areaPath;
        [Export] private NodePath animPath;

        private AnimationPlayer anim;

        public Area2D area;

        public Wizzard myWizard = new Wizzard();
        SetClass SetWizard;
        private string attributeOne;
        private float myEfficiency;
        private int mystrength;
        private int myMagic;
        private int myCharisma;
        private int myArmor;

        public Vector2 initialPos;

        public bool isMouseEntered;
        public bool isDragged;

        private List<string> names = new List<string>() { "Guillaume", "Gwendoline", "Godrick", "Guenlievre", "Gerard" };

        public override void _Ready()
        {
            SetWizard = new SetClass(myWizard, 1);
            myEfficiency = myWizard.Efficiency;
            mystrength = myWizard.Strength;
            myMagic = myWizard.Magic;
            myCharisma = myWizard.Charisma;
            myArmor = myWizard.Armor;
            EntityManager.GetInstance().summonText.Clear();
            EntityManager.GetInstance().SetSummonAttribute(SetWizard, myWizard);
            for (int i = 0; i < EntityManager.GetInstance().summonText.Count; i++)
            {
                attributeOne = EntityManager.GetInstance().summonText[i];
            }

            area = GetNode<Area2D>(areaPath);
            anim = GetNode<AnimationPlayer>(animPath);
            anim.Connect(GlobalReferences.Signals.ANIMATION_FINISHED, this, nameof(OnFinished));
            Connect("mouse_entered", this, nameof(OnMouseEntered));
            Connect("mouse_exited", this, nameof(OnMouseExited));
            Connect(GlobalReferences.Signals.PRESSED, this, nameof(OnPressed));

            initialPos = RectGlobalPosition + RectSize / 4;
        }

        public override void _Process(float delta)
        {
            if (isDragged)
            {
                RectGlobalPosition = GetGlobalMousePosition() - RectSize / 2;
                if (Input.IsActionJustReleased("RMB"))
                {
                    isDragged = false;

                    RectGlobalPosition = initialPos;

                    if (Teleport.GetInstance().isCardIn && !GameManager.GetInstance().isDiscarding)
                    {
                        RectGlobalPosition = Teleport.GetInstance().GlobalPosition - RectSize / 2;
                        anim.Play("Destroy");
                        SoundManager.GetInstance().PlaySfxSound(SfxName.CARD_PLAY, 0, 0);
                        SoundManager.GetInstance().PlaySfxSound(SfxName.SUMMON_RABBIT, 0, 0);
                        Teleport.GetInstance().anim.Play("Destroy");
                        Teleport.GetInstance().isCardIn = false;
                        UtilsSignals.GetInstance().EmitSignal(nameof(UtilsSignals.OnStartFight));
                    }
                    else if (TrashArea.GetInstance().isCardIn && GameManager.GetInstance().isDiscarding)
                    {
                        RectGlobalPosition = TrashArea.GetInstance().GlobalPosition - RectSize / 2;


                        anim.Play("Destroy");
SoundManager.GetInstance().PlaySfxSound(SfxName.CARD_TRASH, 0, 0);
TrashArea.GetInstance().anim.PlayBackwards("Spawn");
                    }
                }
            }
        }
        private async void OnFinished(string pName)
        {
            if (pName == "Destroy")
            {
                if (GameManager.GetInstance().discardAllCard)
                {
                    GameManager.GetInstance().DiscardAll();
                }
                else if (TrashArea.GetInstance().isCardIn && GameManager.GetInstance().needToDiscard)
                {
                    TrashArea.GetInstance().Visible = false;
                    TrashArea.GetInstance().isCardIn = false;
                    GameManager.GetInstance().isDiscarding = false;
                    GameManager.GetInstance().SpawnCards();
                }
                else if (!GameManager.GetInstance().needToDiscard)
                {
                    GameManager.GetInstance().SpawnCards();
                }
                QueueFree();
            }
            if (pName == "Spawn")
            {
                anim.PlaybackSpeed = 1;
            }
            
        }

        private void OnPressed()
        {
            Card.GetInstance().SetPassportInfos(names[GameManager.GetInstance().rand.RandiRange(0, names.Count - 1)], myEfficiency, mystrength,
            myMagic, myCharisma, myArmor, myWizard.Element, this, myWizard, attributeOne);

            Card.GetInstance().ShowPassport();
            SoundManager.GetInstance().PlaySfxSound(SfxName.CARD_ZOOM, 0, 0);

        }
        private void OnMouseEntered()
        {
            isMouseEntered = true;
            SoundManager.GetInstance().PlaySfxSound(SfxName.HIGHLIGHT_BUTTON, 0, 0);
        }
        private void OnMouseExited()
        {
            isMouseEntered = false;
        }

        public override void _Input(InputEvent pEvent)
        {
            if (isMouseEntered && pEvent.IsActionPressed("RMB"))
            {
                isDragged = true;
                UtilsSignals.GetInstance().EmitSignal(nameof(UtilsSignals.OnCardSelected), myWizard);
            }
        }
    }
}
