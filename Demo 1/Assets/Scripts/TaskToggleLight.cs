using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TaskMove))]
public class TaskToggleLight : Task
{
	private TaskMove taskMove;

	public Light l;

	private void Awake()
	{
		taskMove = GetComponent<TaskMove> ();
	}

	public override IEnumerator Execute ()
	{
		print ("Executing");
		taskMove.SetCrew (crew);
		StartCoroutine (taskMove.Execute());
		while(!taskMove.done)
		{
			yield return new WaitForSeconds(1);
		}
		taskMove.done = false;
		
		l.enabled = !l.enabled;

		done = true;
		Finish ();
	}
}

