using Com.IsartDigital.Jam.Managers;
using Godot;
using System;
using System.Collections.Generic;

// Author : Baptiste Simon
namespace Com.IsartDigital.Jam.MyGameObjects.MyEntities {
	
	public class Assassin : Class
	{
        public Assassin(int pHealth, int pStrength, int pMagic, float pEfficiency, int pArmor, int pCharisma, string pElement) :
                           base(pHealth, pStrength, pMagic, pEfficiency, pArmor, pCharisma, pElement)
        {

        }
        public Assassin() : this(0,
            0 + GameManager.GetInstance().rand.RandiRange(0, 5),
            0 + GameManager.GetInstance().rand.RandiRange(0, 5),
            1 + GameManager.GetInstance().rand.RandfRange(0, 1),
            0 + GameManager.GetInstance().rand.RandiRange(0, 5),
            5 + GameManager.GetInstance().rand.RandiRange(0, 5),
            GameManager.GetInstance().RandomElement()) { }
    }
}