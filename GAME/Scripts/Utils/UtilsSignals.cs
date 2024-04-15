using Com.IsartDigital.Jam.MyGameObjects.MyEntities;
using Godot;
using System;

//Author : Baptiste SIMON
	
public class UtilsSignals : Node
{
    static private UtilsSignals instance;
		
    static public UtilsSignals GetInstance () {
	    if (instance == null) instance = new UtilsSignals();
		   return instance;
	}

    //--------------------------------------------------------------------------------------//

    [Signal] public delegate void OnCardSelected(Class pClass);
    [Signal] public delegate void OnStartFight();
    [Signal] public delegate void OnEnemyDead();
    [Signal] public delegate void OnEnemyAttackPlayer();

 //--------------------------------------------------------------------------------------//

    protected override void Dispose(bool pDisposing)
    {
        if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
    }
}
