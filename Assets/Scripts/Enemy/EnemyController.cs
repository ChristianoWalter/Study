using System;
using System.Collections;
using UnityEngine;

public enum EnemyStates
{
    Patrol,
    Attack,
    Search
}

public class EnemyController : CharacterController
{
    public EnemyStates enemyState;

    public Transform[] walkPoints;
    public float stopTime;
    public float detectionRange;
    public float SearchDuration;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    public int rayCount;
    public float coneAngle;

    private int currentWalkPointIndex;
    private bool isWaiting;
    private Transform player;
    private Vector2 lastSeenPosition;
    private float searchTimer;
    private Vector2 lastMovementDirection = Vector2.right;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (walkPoints.Length > 0)
        {
            transform.position = walkPoints[0].position;
        }
    }

    protected override void Movement()
    {
        DetectPlayer();

        switch (enemyState)
        {
            case EnemyStates.Patrol:
                if (!isWaiting && walkPoints.Length > 0)
                {
                    MoveToWalkPoint();
                }
                break;
            case EnemyStates.Attack:
                if (player != null)
                {
                    MoveToPlayer();
                }
                break;
            case EnemyStates.Search:
                SearchForPlayer();
                break;
        }
    }

    private void SearchForPlayer()
    {
        Vector2 newPosition = Vector2.MoveTowards(rb.position, lastSeenPosition, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        if (Vector2.Distance(rb.position, lastSeenPosition) < .1f)
        {
            searchTimer -= Time.fixedDeltaTime; 
            if (searchTimer <= 0)
            {
                enemyState = EnemyStates.Patrol;
            }
        }
    }

    private void MoveToPlayer()
    {
        if (player == null) return;
        Vector2 targetPosition = player.position;
        Vector2 direction = (targetPosition - rb.position).normalized;
        lastMovementDirection = direction;
        Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
    }

    private void MoveToWalkPoint()
    {
        Vector2 targetPosition = walkPoints[currentWalkPointIndex].position;
        Vector2 direction = (targetPosition - rb.position).normalized;
        lastMovementDirection = direction;
        Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        if (Vector2.Distance(rb.position, targetPosition) < 0.1f)
        {
            StartCoroutine(WaitAtWalkPoint());
        }
    }

    IEnumerator WaitAtWalkPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(stopTime);
        currentWalkPointIndex = (currentWalkPointIndex + 1) % walkPoints.Length;
        isWaiting = false;
    }

    private void DetectPlayer()
    {
        bool playerDetected = false;
        float angleStep = coneAngle / (rayCount - 1);
        float startAngle = -coneAngle / 2;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = startAngle + (angleStep * i);
            Vector2 rayDirection = Quaternion.Euler(0, 0, angle) * lastMovementDirection;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, detectionRange, playerLayer | obstacleLayer);
            Debug.DrawRay(transform.position, rayDirection, Color.yellow);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Hit: " + hit.collider.name);
                    player = hit.collider.transform;
                    lastSeenPosition = player.position;
                    enemyState = EnemyStates.Attack;
                    searchTimer = 0;
                    playerDetected = true;
                    break;
                }
                else if (((1 << hit.collider.gameObject.layer) & obstacleLayer) != 0)
                {
                    continue;
                }
            }
        }

        if (!playerDetected && enemyState == EnemyStates.Attack)
        {
            player = null;
            enemyState = EnemyStates.Search;
            searchTimer = SearchDuration;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<CharacterController>().TakeDamage(100);
        }
    }

    protected override IEnumerator DeathHandle()
    {
        Destroy(gameObject);
        return base.DeathHandle();
    }
}
