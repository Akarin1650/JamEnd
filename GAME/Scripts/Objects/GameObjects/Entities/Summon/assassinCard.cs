using Com.IsartDigital.Jam.Managers;
using Com.IsartDigital.Jam.MyGameObjects;
using Com.IsartDigital.Jam.MyGameObjects.MyEntities;
using Com.IsartDigital.ProjectName;
using Com.IsartDigital.Shmup.Manager;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;


namespace Com.IsartDigital.Jam.Cards
{
    public class assassinCard : TextureButton
    {
        [Export] private NodePath areaPath;
        [Export] private NodePath animPath;

        private AnimationPlayer anim;

        public Area2D area;

        private Assassin myAssassin = new Assassin();
        private SetClass SetAssassin;
        private string attributeOne;
        private float myEfficiency;
        private int mystrength;
        private int myMagic;
        private int myCharisma;
        private int myArmor;

        public Vector2 initialPos;

        public bool isMouseEntered;
        public bool isDragged;

        private List<string> names = new List<string>() { "Baldwin", "Sybil", "Bellatrix", "Sihtric", "Sylvain" };

        public override void _Ready()
        {
            SetAssassin = new SetClass(myAssassin, 1);
            myEfficiency = myAssassin.Efficiency;
            mystrength = myAssassin.Strength;
            myMagic = myAssassin.Magic;
            myCharisma = myAssassin.Charisma;
            myArmor = myAssassin.Armor;

            EntityManager.GetInstance().summonText.Clear();
            EntityManager.GetInstance().SetSummonAttribute(SetAssassin, myAssassin);

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

                    GD.Print(GameManager.GetInstance().isDiscarding);

                    if (Teleport.GetInstance().isCardIn && !GameManager.GetInstance().isDiscarding)
                    {
                        RectGlobalPosition = Teleport.GetInstance().GlobalPosition - RectSize / 2;
                        anim.Play("Destroy");
                        SoundManager.GetInstance().PlaySfxSound(SfxName.CARD_PLAY, 0, -5);
                        SoundManager.GetInstance().PlaySfxSound(SfxName.SUMMON_FROG, 0, -5);
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
            GD.Print(pName);
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

        private void OnMouseEntered()
        {
            isMouseEntered = true;
            SoundManager.GetInstance().PlaySfxSound(SfxName.HIGHLIGHT_BUTTON, 0, 0);
        }
        private void OnMouseExited()
        {
            isMouseEntered = false;
        }

        private void DestroyInstance()
        {
            QueueFree();
        }

        public override void _Input(InputEvent pEvent)
        {
            if (isMouseEntered && pEvent.IsActionPressed("RMB"))
            {
                UtilsSignals.GetInstance().EmitSignal(nameof(UtilsSignals.OnCardSelected), myAssassin);
                isDragged = true;
            }
        }

        private void OnPressed()
        {
            Card.GetInstance().SetPassportInfos(names[GameManager.GetInstance().rand.RandiRange(0, names.Count - 1)], myEfficiency, mystrength,
            myMagic, myCharisma, myArmor, myAssassin.Element, this, myAssassin, attributeOne);

            Card.GetInstance().ShowPassport();
            SoundManager.GetInstance().PlaySfxSound(SfxName.CARD_ZOOM, 0, 0);

        }
    }
}
