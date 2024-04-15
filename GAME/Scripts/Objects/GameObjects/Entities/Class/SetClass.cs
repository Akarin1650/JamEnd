using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

// Author : Baptiste SIMON

namespace Com.IsartDigital.Jam.MyGameObjects.MyEntities
{
    public class SetClass
    {
        public Dictionary<string, string> SelectedAttributes { get; } = new Dictionary<string, string>();

        public SetClass(Class pCharacterClass, int pNAttributes)
        {
            Random lRandom = new Random();
            int lMaxIndex = Math.Min(pNAttributes, pCharacterClass.Attributes.Count);

            while (SelectedAttributes.Count < lMaxIndex)
            {

                int lIndex = lRandom.Next(0, pCharacterClass.Attributes.Count);
                string lAttribute = pCharacterClass.Attributes.Keys.ElementAt(lIndex);
                string lDescription = pCharacterClass.Attributes[lAttribute];


                if (!SelectedAttributes.ContainsKey(lAttribute))
                    SelectedAttributes.Add(lAttribute, lDescription);
            }
        }
    }
}
