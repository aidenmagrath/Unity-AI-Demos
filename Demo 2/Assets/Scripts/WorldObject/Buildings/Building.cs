using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : WorldObject
{
	private bool needsBuilding;
	public int currentStep = 0;

	protected override void Awake ()
	{
		base.Awake ();
	}

	protected override void Start ()
	{
		base.Start ();
	}

	protected override void Update ()
	{
		base.Update ();
		if(needsBuilding)
		{
			int step = CalculateStep ();
			if(step > currentStep)
			{
				SetStep(step);
			}
		} else if(!needsBuilding && currentStep != GetAmountOfMeshes())
		{
			SetStep(GetAmountOfMeshes());
		}
	}

	public void StartContruction()
	{
		needsBuilding = true;
		hitPoints = 0;
		DisableMeshes ();
		SetStep (1);
	}

	public void Construct(int amount)
	{
		hitPoints += amount;
		if(hitPoints >= maxHitPoints)
		{
			hitPoints = maxHitPoints;
			needsBuilding = false;
		}
	}

	public bool UnderConstruction()
	{
		return needsBuilding;
	}

	public void DisableMeshes()
	{
		foreach(MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
		{
			renderer.enabled = false;
		}
	}

	private int GetAmountOfMeshes()
	{
		return GetComponentsInChildren<MeshRenderer> ().Length;
	}

	private double GetPercentage()
	{
		return (maxHitPoints / 100) * hitPoints;
	}

	private int CalculateStep()
	{
		return (int)((GetPercentage() / 100) * GetAmountOfMeshes());
	}

	private void SetStep(int step)
	{
		print ("Setting step: " + step);
		currentStep = step;
		MeshRenderer[] renders = GetComponentsInChildren<MeshRenderer> ();
		for(int i = 0; i < step; i++)
		{
			renders[i].enabled = true;
		}
	}


}