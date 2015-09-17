using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinder : MonoBehaviour {

	private NavMeshAgent navAgent;

	private void Awake()
	{
		navAgent = GetComponent<NavMeshAgent> ();
	}

	public void Move(Vector3 position)
	{
		navAgent.SetDestination (position);
	}

	/// <summary>
	/// Check if Crew member has finished moving to its destination.
	/// </summary>
	/// <returns><c>true</c>, if in position, <c>false</c> otherwise.</returns>
	public bool InPosition()
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
