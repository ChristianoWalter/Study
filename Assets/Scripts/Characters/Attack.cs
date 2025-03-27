using UnityEngine;

public class Attack : MonoBehaviour
{
    [HideInInspector] public float damage;
     public float speed;
    [HideInInspector] public Vector2 direction;
    [SerializeField] GameObject effect;
    [SerializeField] Rigidbody2D rb;

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyController>(out var enemy))
        {
            enemy.GetComponent<CharacterController>().TakeDamage(damage);
        }
        if (effect != null) Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void UpdateAttackSettings(float _newDamage, float _newSpeed, Vector2 _direction)
    {
        damage = _newDamage;
        speed = _newSpeed;
        direction = _direction;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        if (rb != null) rb.linearVelocity = _direction.normalized * speed;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
