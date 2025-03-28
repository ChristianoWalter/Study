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
        if (weapon == null)
            return;

        switch (weapon.type)
        {
            case AttackType.Projectile:
                Attack _attackProjectile = Instantiate(weapon.ammo.GetComponent<Attack>(), attackSpawnPoint.position, Quaternion.identity);
                _attackProjectile.UpdateAttackSettings(weapon.damage, weapon.projectileSpeed, Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
                break;
            case AttackType.Instant:
                var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                position.Scale(new Vector3(1, 1, 0));
                Attack _attackObj = Instantiate(weapon.ammo.GetComponent<Attack>(), position, Quaternion.identity);
                _attackObj.UpdateAttackSettings(weapon.damage, weapon.projectileSpeed, _attackObj.gameObject.transform.position);
                break;
        }
    }
}
