using Com.IsartDigital.Jam.Managers;
using Godot;
using System;
using System.Collections.Generic;

// Author : Baptiste Simon
namespace Com.IsartDigital.Jam.MyGameObjects.MyEntities {
	
	public class Archer : Class
	{
        public Archer(int pHealth, int pStrength, int pMagic, float pEfficiency, int pArmor, int pCharisma, string pElement) :
                 base(pHealth, pStrength, pMagic, pEfficiency, pArmor, pCharisma, pElement)
        {

        }
        public Archer() : this(0,
            3 + GameManager.GetInstance().rand.RandiRange(0, 5),
            3 + GameManager.GetInstance().rand.RandiRange(0, 5),
            0.8f + GameManager.GetInstance().rand.RandfRange(0, 1),
            0 + GameManager.GetInstance().rand.RandiRange(0, 5),
            0 + GameManager.GetInstance().rand.RandiRange(0, 5),
            GameManager.GetInstance().RandomElement()
            )
        { }
    }
}