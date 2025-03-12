using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyController>(out var enemy))
        {
            enemy.GetComponent<CharacterController>().TakeDamage(damage);
        }
    }
}
