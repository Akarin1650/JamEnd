using Com.IsartDigital.Jam.Managers;
using Godot;
using System;

// Author : Baptiste Simon
namespace Com.IsartDigital.Jam.MyGameObjects.MyEntities
{

	public class Witcher : Enemy
	{
		public Witcher(int pHealth, int pStrength, int pMagic, int pEfficiency, int pArmor, int pCharisma, string pElement) :
						   base(pHealth, pStrength, pMagic, pEfficiency, pArmor, pCharisma, pElement)
		{

		}
		public Witcher() : this(
						(int)GD.RandRange(6, 8),
						(int)GD.RandRange(0, 1),
						(int)GD.RandRange(4, 7),
						0,
						0,
						(int)GD.RandRange(1, 2),
						GameManager.GetInstance().RandomElement()
					)
		{ }
	}
}