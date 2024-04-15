using Com.IsartDigital.Jam.Cards;
using Com.IsartDigital.Jam.Managers;
using Godot;
using System;

// Author : Baptiste Simon
namespace Com.IsartDigital.Jam
{

    public class InputHandler : Node
    {

        //--------------------- INPUTS HANDLER ---------------------//
        public override void _Input(InputEvent pEvent)
        {
            if (pEvent.IsActionReleased("LMB") && Card.GetInstance().Visible)
            {
                Card.GetInstance().HidePassport();
            }
        }
    }
}