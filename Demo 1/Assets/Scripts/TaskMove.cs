using UnityEngine;
using System.Collections;

public class TaskMove : Task
{
	public override IEnumerator Execute ()
	{
		print ("Moving");
		Pathfinder pathfinder = crew.gameObject.GetComponent<Pathfinder> ();
		pathfinder.Move (transform.position);
		while(!pathfinder.InPosition())
		{
			yield return new WaitForSeconds(1);
		}
		print ("In pos");
		done = true;
		Finish ();
	}
}

