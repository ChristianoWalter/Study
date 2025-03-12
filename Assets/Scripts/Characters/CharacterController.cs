using System.Collections;
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
    [SerializeField] GameObject visual;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    protected virtual void FixedUpdate()
    {
        Movement();
    }

    protected virtual void Movement()
    {
        rb.linearVelocity = direction * speed;
        if ((rb.linearVelocity.x > 0 && visual.transform.localScale.x < 0) || (rb.linearVelocity.x < 0 && visual.transform.localScale.x > 0))
        {
            Vector2 _localScale = visual.transform.localScale;
            _localScale.x *= -1f;
            visual.transform.localScale = _localScale;
        }

        anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x) + Mathf.Abs(rb.linearVelocity.y));
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);

        if (currentHealth == 0)
        {
            StartCoroutine(DeathHandle());
        }
    }

    public void TakeHeal(float heal)
    {
        currentHealth = Mathf.Min(currentHealth + heal, maxHealth);
    }

    protected virtual IEnumerator DeathHandle()
    {
        yield return null;
    }
}
