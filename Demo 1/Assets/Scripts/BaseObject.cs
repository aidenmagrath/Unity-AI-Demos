using UnityEngine;
using System.Collections;

public class BaseObject : MonoBehaviour
{
	/// <summary>
	/// The type of Task the object will use. The GameObject must contain an instance of Task with the given name.
	/// </summary>
	protected string taskType = "Task";

	protected virtual void Update(){
		if (Input.GetMouseButtonDown(0)){ // if left button pressed...
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)){
				if(hit.transform == this.transform)
				{
					Debug.Log("Hit: " + hit.transform.name);
					AddTask((Task)GetComponent(taskType));
				}
			}
		}
	}

	/// <summary>
	/// Adds the task to the TaskManager.
	/// </summary>
	/// <seealso cref="TaskManager"/>
	/// <param name="task">Task.</param>
	protected void AddTask(Task task)
	{
		TaskManager.GetManager ().AddTask (task);
	}
}

