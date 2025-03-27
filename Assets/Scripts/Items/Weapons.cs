using UnityEngine;
public enum AttackType
{
    Projectile,
    Instant
}

[CreateAssetMenu(fileName = "Weapons", menuName = "Scriptable Objects/Weapons")]
public class Weapons : Item
{
    [Header("Weapon Settings")]
    public float damage;
    public float projectileSpeed;
    public Sprite weaponVisual;
    public AttackType type;
    public float cooldown;
    public GameObject ammo;
}
