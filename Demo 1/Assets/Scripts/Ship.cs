using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
	public float power;
	public UnityEngine.UI.Text txtPower;

	private void Update()
	{
		txtPower.text = "Power: " + power.ToString("n1") + "%";
	}
}

