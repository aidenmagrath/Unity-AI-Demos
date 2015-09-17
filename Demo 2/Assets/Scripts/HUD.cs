using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HUD : MonoBehaviour
{
	private static HUD hud;
	public Transform statsPanel;

	private void Awake()
	{
		hud = GetComponent<HUD> ();
	}

	private void Start()
	{

	}

	public static HUD GetHUD()
	{
		return hud;
	}

	public void ShowStats(bool showStats)
	{
		statsPanel.gameObject.SetActive (showStats);
	}

	public void SetStats(WorldObject worldObject)
	{
		Color color = worldObject.player.playerColor;

		Text nameText = statsPanel.FindChild ("Name").gameObject.GetComponent<Text> ();
		nameText.text = worldObject.objectName;
		nameText.color = color;

		if(worldObject.hasInventory)
		{
			statsPanel.FindChild("IronLabel").gameObject.SetActive(true);
			statsPanel.FindChild("IronAmount").gameObject.SetActive(true);
			Text ironAmount = statsPanel.FindChild("IronAmount").gameObject.GetComponent<Text>();
			ironAmount.text = worldObject.hitPoints.ToString("0.0");
			ironAmount.color = color;
		} else
		{
			statsPanel.FindChild("IronLabel").gameObject.SetActive(false);
			statsPanel.FindChild("IronAmount").gameObject.SetActive(false);
		}
	}
}

