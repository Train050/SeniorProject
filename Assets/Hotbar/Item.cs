using UnityEngine;

public enum ItemType
{
    Consumable,
    Material,
    Weapon,
    Trap,
    Firewood
}

public class Item : MonoBehaviour
{
    
	public enum Alignment
	{
		Player,
		Enemy,
		Environment
	}

    public int ID;
    public string itemName;
    public float imageScale = 2.5f;

	[Tooltip("This determines whose side the damager is on. If the player is wielding the weapon, then its alignment is Player")]
	public Alignment alignmnent = Alignment.Player;
	public ItemType itemType;
	// public WeaponType weaponType = WeaponType.Melee;
	public int healthValue = 0;

	public virtual void WeaponStart(Transform wielderPosition, Vector2 lastLookDirection) { }
	public virtual void WeaponStart(Transform wielderPosition, Vector2 lastLookDirection, Vector2 currentVelocity) { }

	public virtual void WeaponFinished() { }

    public AudioClip placeInWorldSound;
    public AudioClip useSound;
    public AudioClip pickupSound;
    //public int duration;
}