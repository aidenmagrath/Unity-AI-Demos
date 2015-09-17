using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Task : MonoBehaviour
{
	protected Crew crew;
	public List<CrewType> crewTypes = new List<CrewType>();
	public bool done = false;
	public short priority = 1;
	public bool rootTask = false;

	/// <summary>
	/// Codes actions which need to be executed for the task to be completed.
	/// </summary>
	public abstract IEnumerator Execute();

	/// <summary>
	/// Checks to see if the task is a root task, and if so it sets the crew member as idle.
	/// </summary>
	protected void Finish()
	{
		if(rootTask)
		{
			print("Making idle");
			crew.idle = true;
			done = false;
		}
	}

	/// <summary>
	/// Sets the crew who will perform the task.
	/// </summary>
	/// <param name="crew">Crew.</param>
	public void SetCrew(Crew crew)
	{
		this.crew = crew;
	}
	
	private void Reset()
	{
		crewTypes.Add (CrewType.Anyone);
	}

}



