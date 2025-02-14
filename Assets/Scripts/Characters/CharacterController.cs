using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Movements Variables")]
    public float speed;
    [SerializeField] protected Vector2 direction;

    [Header("Life Variables")]
    public float maxHealth;
    protected float currentHealth;

    [Header("Components")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Animator anim;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    protected void Movement()
    {
        rb.linearVelocity = direction * speed;
        if ((rb.linearVelocity.x > 0 && transform.localScale.x < 0) || (rb.linearVelocity.x < 0 && transform.localScale.x > 0))
        {
            Vector2 _localScale = transform.localScale;
            _localScale.x *= -1f;
            transform.localScale = _localScale;
        }

        anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x) + Mathf.Abs(rb.linearVelocity.y));
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
    }

    public void TakeHeal(float heal)
    {
        currentHealth = Mathf.Min(currentHealth + heal, maxHealth);
    }

    
}
