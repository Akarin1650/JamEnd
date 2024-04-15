using Com.IsartDigital.Jam.Managers;
using Godot;
using System;
using System.Collections.Generic;

// Author : Baptiste Simon
namespace Com.IsartDigital.Jam.MyGameObjects.MyEntities
{

    public abstract class Class : Entities
    {
        public Class(int pHealth, int pStrength, int pMagic, float pEfficiency, int pArmor, int pCharisma, string pElement) :
            base(pHealth, pStrength, pMagic, pEfficiency, pArmor, pCharisma, pElement)
        {

        }

        public Class() : this(0, 0, 0, 0, 0, 0, GameManager.GetInstance().RandomElement()) { }

        public virtual Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>
        {
            { "-2:strength", "ATTRIBUTES_ALLIES_0"},
            { "-2:magic", "ATTRIBUTES_ALLIES_1" },
            { "+2:armor", "ATTRIBUTES_ALLIES_2" },
            { "-2:efficienty", "ATTRIBUTES_ALLIES_3" },
            { "-2:charism", "ATTRIBUTES_ALLIES_4" },
            { "-1:strength, -1:efficienty", "ATTRIBUTES_ALLIES_5" },
            { "+2:strength, -2:magic", "ATTRIBUTES_ALLIES_6" },
            { "-2:charism, -2:efficienty", "ATTRIBUTES_ALLIES_7" },
            { "-2:armor, +2:charism", "ATTRIBUTES_ALLIES_8" },
            { "-2:armor, +2:magic", "ATTRIBUTES_ALLIES_9" },
            { "roll magic, strength, charism", "ATTRIBUTES_ALLIES_10" },
            { "folie", "ATTRIBUTES_ALLIES_12" },
            { "win or loose", "ATTRIBUTES_ALLIES_13" },
        };
    }
}