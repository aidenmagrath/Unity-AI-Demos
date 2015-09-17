using UnityEngine;
using System.Collections;

public class TaskClean : Task
{
	public TaskMove taskMove;

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

		int cleanLevel = 0;
		while(cleanLevel < 10)
		{
			cleanLevel++;
			yield return new WaitForSeconds(1);
		}

		done = true;
		Finish ();
		Destroy (gameObject);
	}
}

