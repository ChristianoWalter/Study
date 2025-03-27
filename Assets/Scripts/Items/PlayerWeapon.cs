using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public Weapons weapon;
    [SerializeField] Transform attackSpawnPoint;
    [SerializeField] SpriteRenderer visual;


    public void ChangeWeapon(Weapons _newWeapon)
    {
        weapon = _newWeapon;
        visual.sprite = _newWeapon.weaponVisual;
    }

    public void Attack()
    {
        switch (weapon.type)
        {
            case AttackType.Projectile:
                Attack _attackProjectile = Instantiate(weapon.ammo.GetComponent<Attack>(), attackSpawnPoint.position, Quaternion.identity);
                _attackProjectile.UpdateAttackSettings(weapon.damage, weapon.projectileSpeed, Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
                break;
            case AttackType.Instant:
                Attack _attackObj = Instantiate(weapon.ammo.GetComponent<Attack>(), Input.mousePosition, Quaternion.identity);
                _attackObj.UpdateAttackSettings(weapon.damage, weapon.projectileSpeed, _attackObj.gameObject.transform.position);
                break;
        }
    }
}
