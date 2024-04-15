using Com.IsartDigital.Jam.Managers;
using Godot;
using System;
using System.Collections.Generic;

// Author : Baptiste Simon
namespace Com.IsartDigital.Jam.MyGameObjects.MyEntities
{
	
	public class Tank : Class
	{
        public Tank(int pHealth, int pStrength, int pMagic, float pEfficiency, int pArmor, int pCharisma, string pElement) :
                base(pHealth, pStrength, pMagic, pEfficiency, pArmor, pCharisma, pElement)
        {

        }
        public Tank() : this(0,
            2 + GameManager.GetInstance().rand.RandiRange(0, 3),
            0 + GameManager.GetInstance().rand.RandiRange(0, 3),
            0.5f + GameManager.GetInstance().rand.RandfRange(0, 0.7f),
            4 + GameManager.GetInstance().rand.RandiRange(0, 4),
            0 + GameManager.GetInstance().rand.RandiRange(0, 5),
            GameManager.GetInstance().RandomElement()) { }
    }
}