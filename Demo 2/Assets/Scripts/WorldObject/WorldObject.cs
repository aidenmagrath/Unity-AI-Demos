using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldObject : MonoBehaviour
{
	public string objectName;
	public int objectId;
	public GameObject selectionRingPrefab;


	public float detectionRange = 200.0f;
	public float weaponRange = 10.0f;
	public float weaponRechargeTime = 1.0f;
	public float weaponAimSpeed = 1.0f;
	public int hitPoints = 100;
	public int maxHitPoints = 100;

	public Player player;
	public GameObject selectionRing = null;
	protected List<WorldObject> nearbyObjects;
	protected WorldObject target = null;
	private bool selected;
	public bool attacking;
	public bool movingIntoPosition;
	public bool aiming;

	private float timeSinceLastDecision = 0.0f;
	private float timeBetweenDecisions = 0.1f;
	private float currentWeaponChargeTime;
	private List<Material> oldMaterials = new List<Material>();	
	private int collisionCount;

	public int inventoryMaxCapacity = 60;
	public int inventoryCurrentCapacity = 0;
	public bool hasInventory = true;
	public Dictionary<ResourceType, int> inventory = new Dictionary<ResourceType, int>();

	public void SetSelected(bool selected)
	{
		this.selected = selected;

		if(selected)
		{

			UpdateHUD();
		} 
	}

	public int GetResource(ResourceType resource)
	{
		if(inventory.ContainsKey(resource))
		{
			return inventory[resource];
		} else
		{
			return 0;
		}
	}

	public bool IsSelected()
	{
		return selected;
	}

	public void AddResourceToInventory(ResourceType resourceType, int amount)
	{
		if(inventoryCurrentCapacity + amount > inventoryMaxCapacity)
		{
			amount = inventoryMaxCapacity - inventoryCurrentCapacity;
		}

		if(inventory.ContainsKey(resourceType))
		{
			inventory[resourceType] += amount;
		} else
		{
			inventory.Add(resourceType, amount);
		}
	}

	public void RemoveResourceFromInventory(ResourceType resourceType, int amount)
	{
		if(inventory.ContainsKey(resourceType))
		{
			if(inventory[resourceType] < amount)
			{
				amount = inventory[resourceType];
			}
			inventory[resourceType] -= amount;
		}
	}

	public void AddItemToInventory(ResourceType resourceType, int amount)
	{
		if(inventoryCurrentCapacity + amount <= inventoryMaxCapacity)
		{
			if(inventory.ContainsKey(resourceType))
			{
				inventory[resourceType] += amount;
			} else
			{
				inventory.Add(resourceType, amount);
			}
		}
	}

	protected virtual void Awake()
	{
		player = transform.root.GetComponent<Player> ();
	}

	protected virtual void Start()
	{
		
	}

	protected virtual void Update()
	{
		if(selected && selectionRing == null)
		{
			selectionRing = Instantiate(selectionRingPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
			selectionRing.transform.parent = gameObject.transform;
			selectionRing.transform.localPosition = new Vector3(0, -0.99f, 0);
		} else if(!selected && selectionRing != null)
		{
			Destroy(selectionRing);
		}

		if(ShouldMakeDecision())
		{
			DecideWhatToDo();
		}
		currentWeaponChargeTime += Time.deltaTime;
		if(attacking && !movingIntoPosition && !aiming)
		{
			//print("SHould attack");
			PerformAttack();
		}
	}

	protected virtual void OnGUI()
	{

	}

	private void UpdateHUD()
	{

		HUD.GetHUD ().SetStats (this);
		HUD.GetHUD ().ShowStats (true);
	}

	protected virtual bool ShouldMakeDecision()
	{
		if(!attacking && !movingIntoPosition && !aiming)
		{
			if(timeSinceLastDecision > timeBetweenDecisions)
			{
				//print("Should Make Decision: " + objectId);
				timeSinceLastDecision = 0.0f;
				return true;
			}
			timeSinceLastDecision += Time.deltaTime;
		}
		return false;
	}


	protected virtual void DecideWhatToDo()
	{
		Vector3 currentPos = transform.position;
		nearbyObjects = FindNearbyObjects (currentPos, detectionRange);
		if(CanAttack())
		{
			//print("Can Attack: " + objectId);
			List<WorldObject> enemyObjects = new List<WorldObject>();
			foreach(WorldObject nearbyObject in nearbyObjects)
			{
				if(nearbyObject.player != player)
				{
					enemyObjects.Add(nearbyObject);
				} else
				{
				}
			}
			WorldObject closestObject = FindNearestWorldObject(enemyObjects, currentPos);
			if(closestObject != null)
			{
				BeginAttack(closestObject);
			}
		}

	}

	protected virtual void AimAtTarget()
	{
		aiming = true;
	}

	protected virtual void UseWeapon()
	{
		currentWeaponChargeTime = 0.0f;
	}

	public virtual bool CanAttack()
	{
		return false;
	}

	public virtual bool CanMove()
	{
		return false;
	}

	public virtual void MouseClick(GameObject hitObject, Vector3 hitPoint, Player controller)
	{
		if(selected && hitObject && hitObject.tag != "Ground")
		{
			WorldObject worldObject = hitObject.transform.GetComponent<WorldObject>();
			if(worldObject)
			{
				Player owner = hitObject.transform.root.GetComponent<Player>();
				if(owner != null)
				{
					if(player != null && player.human)
					{
						if(!player.playerName.Equals(owner.playerName))
						{
							//print("Names are not equal");
							BeginAttack(worldObject);
						} else
						{
							//print("Names are equal");
							ChangeSelection(worldObject, controller);
						}
					} else
					{
					//	print("Player null or not human");
						ChangeSelection(worldObject, controller);
					}
				} else
				{
					//print("Owner null");
					ChangeSelection(worldObject, controller);
				}

			}
		}
	}

	public virtual void SetBuilding(Building project)
	{

	}

	public void SetTransparentMaterial(Material material, bool storeExistingMaterial)
	{
		if(storeExistingMaterial)
		{
			oldMaterials.Clear();
		}

		Renderer[] renderers = GetComponentsInChildren<Renderer> ();
		foreach(Renderer renderer in renderers)
		{
			if(storeExistingMaterial)
			{
				oldMaterials.Add(renderer.material);
			}
			renderer.material = material;
		}
	}

	public void SetCollidersAsTrigger(bool isTrigger)
	{
		Collider[] colliders = GetComponentsInChildren<Collider> ();
		foreach(Collider collider in colliders)
		{
			collider.isTrigger = isTrigger;
		}
	}

	private void BeginAttack(WorldObject target)
	{
		//print ("Begining Attack");
		this.target = target;
		if(TargetInRange())
		{
			//print("Target in range");
			attacking = true;
			PerformAttack();
		} else
		{
			AdjustPosition();
		}
	}

	private bool TargetInRange()
	{
		Vector3 targetLocation = target.transform.position;
		Vector3 direction = targetLocation - transform.position;
		if(direction.sqrMagnitude < weaponRange * weaponRange)
		{
			return true;
		}
		return false;
	}

	private bool TargetInFrontOfWeapon()
	{
		Vector3 targetLocation = target.transform.position;
		Vector3 direction = targetLocation - transform.position;
		if(direction.normalized == transform.forward.normalized)
		{
			return true;
		}
		return false;

	}

	private void PerformAttack()
	{
		if(target == null)
		{
			//print("target null");
			attacking = false;
			return;
		}

		if(!TargetInRange())
		{
			//print("Not in range");
			AdjustPosition();
		} 
//		else if(!TargetInFrontOfWeapon())
//		{
//			print("Aiming");
//			AimAtTarget();
//		}
		else if(ReadyToFire())
		{
			print("REady to fire");
			UseWeapon();
		}
	}

	private bool ReadyToFire()
	{
		if(currentWeaponChargeTime >= weaponRechargeTime)
		{
			return true;
		}
		return false;
	}

	private void AdjustPosition()
	{
		Unit self = this as Unit;
		if(self != null)
		{
			movingIntoPosition = true;
			Vector3 attackPosition = FindNearestAttackPosition();
			self.StartMove(attackPosition);
			attacking = true;
		} else
		{
			attacking = false;
		}
	}

	private Vector3 FindNearestAttackPosition()
	{
		Vector3 targetLocation = target.transform.position;
		Vector3 direction = targetLocation - transform.position;
		float targetDistance = direction.magnitude;
		float distanceToTravel = targetDistance - (0.9f * weaponRange);
		return Vector3.Lerp(transform.position, targetLocation, distanceToTravel / targetDistance);
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag != "Ground")
		{
			print("Trigger Enter");
			collisionCount++;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.tag != "Ground")
		{
			//print("Trigger Exit");
			collisionCount--;
		}
	}

	public bool IsColliding()
	{
		if(collisionCount > 0)
		{
			//print("Colliding");
			return true;
		} 
		
		return false;
	}

	public void TakeDamage(int damage)
	{
		hitPoints -= damage;
		if(hitPoints <= 0)
		{
			Destroy(gameObject);
		}
	}

	private void ChangeSelection(WorldObject worldObject, Player controller)
	{
		SetSelected(false);
		if(controller.selectedObject)
		{
			controller.selectedObject = worldObject;
		}
		worldObject.SetSelected (true);
	}



	public static List<WorldObject> FindNearbyObjects(Vector3 position, float range)
	{
		Collider[] hitColliders = Physics.OverlapSphere (position, range);
		HashSet<int> nearbyObjectIds = new HashSet<int> ();
		List<WorldObject> nearbyObjects = new List<WorldObject> ();
		for(int i = 0; i < hitColliders.Length; i++)
		{
			WorldObject colliderObject = hitColliders[i].GetComponent<WorldObject>();
			if(colliderObject != null && !nearbyObjectIds.Contains(colliderObject.objectId))
			{
				nearbyObjectIds.Add(colliderObject.objectId);
				nearbyObjects.Add(colliderObject);
			
			}
		}
		return nearbyObjects;
	}

	public static WorldObject FindNearestWorldObject(List<WorldObject> objects, Vector3 position)
	{
		if(objects == null || objects.Count == 0)
		{
			return null;
		}

		WorldObject nearestObject = objects[0];
		float distanceToNearestObject = Vector3.Distance (position, nearestObject.transform.position);
		for(int i = 0; i < objects.Count; i++) {
			float distanceToObject = Vector3.Distance(position, objects[i].transform.position);
			if(distanceToObject < distanceToNearestObject) {
				distanceToNearestObject = distanceToObject;
				nearestObject = objects[i];
			}
		}
		return nearestObject;
	}
}
