using UnityEngine;
using System.Collections;

public class Soldier: Unit
{
	protected override void Start ()
	{
		base.Start ();
	}

	protected override void Update ()
	{
		base.Update ();
	}

	public override bool CanAttack ()
	{
		return true;
	}

	protected override void UseWeapon ()
	{
		base.UseWeapon ();
		target.TakeDamage (25);
//		print ("Weapon Fired!");
	}

	protected override void AimAtTarget () {
		base.AimAtTarget();

	}


}

