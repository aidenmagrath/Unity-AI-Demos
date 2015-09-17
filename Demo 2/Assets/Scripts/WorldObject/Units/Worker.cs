using UnityEngine;
using System.Collections;

public class Worker : Unit
{
	public Building currentProject;
	public int buildSpeed = 5;
	public bool building = false;
	public float amountBuilt = 0.0f;

	protected override void Start ()
	{
		base.Start ();
		inventory.Add (ResourceType.Iron, 5);
		inventory.Add (ResourceType.Uranium, 8);
	}
	
	protected override void Update ()
	{
		base.Update ();
		if(!moving)
		{
			//print("Not moving");
			if(building && currentProject != null && currentProject.UnderConstruction())
			{
				//print("Under construction");
				amountBuilt += buildSpeed * Time.deltaTime;
				int amount = Mathf.FloorToInt(amountBuilt);
				if(amount > 0)
				{
					amountBuilt -= amount;
					currentProject.Construct(amount);
					if(!currentProject.UnderConstruction())
					{
						building = false;
					}
				}
			}
		}
	}

	public override bool CanAttack ()
	{
		return false;
	}
	
	protected override void AimAtTarget () {
		base.AimAtTarget();
		
	}

	public override void MouseClick (GameObject hitObject, Vector3 hitPoint, Player controller)
	{
		bool doBase = true;
		//print ("Called worker mouse click");
		if(player != null && player.human && IsSelected() && hitObject.tag != "Ground")
		{
			//print("Was not ground");
			Building building = hitObject.transform.GetComponent<Building>();
			if(building != null)
			{
				//print("Building not null");
				SetBuilding(building);
				doBase = false;
			} else
			{
				//print("Building null: " + hitObject.transform.parent.name);
			}
		}

		if(doBase)
		{
			base.MouseClick (hitObject, hitPoint, controller);
		}
	}

	public override void SetBuilding (Building project)
	{
		base.SetBuilding (project);
		currentProject = project;
		StartMove (currentProject.transform.position);
		building = true;
	}

	public override void StartMove (Vector3 destination)
	{
		base.StartMove (destination);
		amountBuilt = 0.0f;
		building = false;
	}
}

