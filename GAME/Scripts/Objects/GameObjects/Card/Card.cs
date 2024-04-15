using Com.IsartDigital.Jam.Managers;
using Com.IsartDigital.Jam.MyGameObjects;
using Com.IsartDigital.Jam.MyGameObjects.MyEntities;
using Godot;
using Microsoft.SqlServer.Server;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;

namespace Com.IsartDigital.Jam.Cards
{

    public class Card : Control
    {
        static private Card instance;

        static public Card GetInstance()
        {
            if (instance == null) instance = new Card();
            return instance;
        }
        private Card() : base() { }

        [Export] private NodePath animPath;

        public AnimationPlayer anim;
        [Export] private NodePath efficiencyBarPath;
        [Export] private NodePath strengthBarPath;
        [Export] private NodePath magicBarPath;
        [Export] private NodePath charismaBarPath;
        [Export] private NodePath defenseBarPath;
        [Export] private NodePath attributeLabelPath;
        [Export] private NodePath attributeLabel2Path;

        [Export] private NodePath spriteClassPath;
        private AnimatedSprite spriteClass;
        
        [Export] private NodePath spriteTypePath;
        private AnimatedSprite spriteType;

        [Export] private NodePath namePath;
        private Label name;

        [Export] private NodePath gentilsPath;
        private Node2D gentils;
        
        [Export] private NodePath mechonPath;
        private Node2D mechon;
        [Export] private NodePath descriptionPath;
        private Label description;
        [Export] private NodePath attributeLabel3Path;
        private Label attributeLabel3;

        private TextureProgress efficiencyBar;
        private TextureProgress strengthBar;
        private TextureProgress magicBar;
        private TextureProgress charismaBar;
        private TextureProgress defenseBar;

        private Label attributeLabel;
        private Label attributeLabel2;
        private TextureButton currentCard;
        public Class currentClass;

        public override void _Ready()
        {
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(Card) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;

            efficiencyBar = GetNode<TextureProgress>(efficiencyBarPath);
            strengthBar = GetNode<TextureProgress>(strengthBarPath);
            magicBar = GetNode<TextureProgress>(magicBarPath);
            charismaBar = GetNode<TextureProgress>(charismaBarPath);
            defenseBar = GetNode<TextureProgress>(defenseBarPath);
            attributeLabel = GetNode<Label>(attributeLabelPath);
            attributeLabel2 = GetNode<Label>(attributeLabel2Path);
            name = GetNode<Label>(namePath);

            anim = GetNode<AnimationPlayer>(animPath);
            spriteClass = GetNode<AnimatedSprite>(spriteClassPath);
            spriteType = GetNode<AnimatedSprite>(spriteTypePath);

            gentils = GetNode<Node2D>(gentilsPath);
            mechon = GetNode<Node2D>(mechonPath);
            description = GetNode<Label>(descriptionPath);
            attributeLabel3 = GetNode<Label>(attributeLabel3Path);
        }

        public void SetPassportInfos(string pName, float pEfficiencyValue, int pStrengthValue, int pMagicValue,
            int pCharismaValue, int pDefenseValue, string pElement, TextureButton pCard, Class pClass, string pAttributeText = null, string pAttributeTwo = null)
        {
            if(!gentils.Visible)
            {
                gentils.Visible = true;
                mechon.Visible = false;
            }

            SelectFrame(pClass);
            SelectType(pElement);

            name.Text = pName;

            efficiencyBar.Value = pEfficiencyValue;
            strengthBar.Value = pStrengthValue;
            magicBar.Value = pMagicValue;
            charismaBar.Value = pCharismaValue;
            defenseBar.Value = pDefenseValue;
            attributeLabel.Text = pAttributeText;
            attributeLabel2.Text = pAttributeTwo;
            currentCard = pCard;
            currentClass = pClass;
        }
        public void SetPassportInfos(Class pClass, string pDescription, string pName, string pElement, string pAttributeText1, string pAttributeText2 = null, string pAttributeText3 = null)
        {
            if(!mechon.Visible)
            {
                mechon.Visible= true;
                gentils.Visible= false;
            }

            SelectFrame(pClass);
            SelectType(pElement);

            name.Text = pName;

            description.Text = pDescription;

            attributeLabel.Text = pAttributeText1;
            attributeLabel2.Text = pAttributeText2;
            attributeLabel3.Text = pAttributeText3;
        }

        private void SelectFrame(Class pClass)
        {
            if (pClass is Knight) spriteClass.Frame = 0;
            else if(pClass is Archer) spriteClass.Frame = 1;
            else if(pClass is Assassin) spriteClass.Frame = 2;
            else if(pClass is Wizzard) spriteClass.Frame = 3;
            else if(pClass is Tank) spriteClass.Frame = 4;
            else if(pClass is Dragon) spriteClass.Frame = 5;
            else if(pClass is Demon) spriteClass.Frame = 6;
            else if(pClass is Squelette) spriteClass.Frame = 7;
            else if(pClass is Sorcier) spriteClass.Frame = 8;
            else if(pClass is Zombie) spriteClass.Frame = 9;
        }

        private void SelectType(string pElement)
        {
            if (pElement == "Feu") spriteType.Frame = 0;
            else if (pElement == "Nature") spriteType.Frame = 1;
            else spriteType.Frame = 2;
        }

        public void ShowPassport()
        {
            Visible = true;
            anim.Play("Spawn");
        }
        public async void HidePassport()
        {
            anim.Play("Despawn");
            await UtilsCoroutine.WaitForSeconds(0.5f);
            Visible = false;
        }

        private void OnCardPressed()
        {
            Visible = false;
            //UtilsSignals.GetInstance().EmitSignal(nameof(UtilsSignals.OnCardSelected), currentClass);
        }
    }
}