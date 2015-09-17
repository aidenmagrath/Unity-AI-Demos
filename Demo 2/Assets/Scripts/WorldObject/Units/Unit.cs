using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : WorldObject
{
	private NavMeshAgent navAgent;

	private Vector3 destination;
	private Quaternion targetRotation;

	protected bool moving;

	protected override void Awake()
	{
		base.Awake ();

		navAgent = GetComponent<NavMeshAgent>();
	}

	protected override void Update()
	{
		base.Update ();

		if(moving)
		{
			navAgent.SetDestination(destination);
			if(InPosition())
			{
				movingIntoPosition = false;
				moving = false;
			}
		}
	}

	protected override bool ShouldMakeDecision ()
	{
		if(!InPosition())
		{
			return false;
		}
		return base.ShouldMakeDecision ();
	}

	public override void MouseClick (GameObject hitObject, Vector3 hitPoint, Player controller)
	{
		base.MouseClick (hitObject, hitPoint, controller);
//		print ("MouseClick");
		if(player != null && player.human && IsSelected())
		{
			//print("Is Human: " + player.playerName);
			if(hitObject.tag == "Ground" && hitPoint != ResourceManager.INVALID_POSITION)
			{
				StartMove(hitPoint);
			}
		}
	}

	public virtual void StartMove(Vector3 destination)
	{
		this.destination = destination;
		moving = true;
	}

	private bool InPosition()
	{
		if (!navAgent.pathPending)
		{
			if (navAgent.remainingDistance <= navAgent.stoppingDistance)
			{
				if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f)
				{
					return true;
				}
			}
		}
		
		return false;
	}
}

