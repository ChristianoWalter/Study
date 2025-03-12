using System;
using System.Collections;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : CharacterController
{
    public static PlayerController instance;

    [SerializeField] Interactable interactableTrigged;

    [Header("Attack variables")]
    public GameObject attackHitbox;
    public float attackDuration;
    private bool canAttack = true;
    private bool holdingMouse = false;

    protected override void Awake()
    {
        base.Awake();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }
    }

    private void Update()
    {
        if (attackHitbox != null && canAttack)
        {
            RotateAttackHitboxTowardsMouse();

            if (Input.GetMouseButtonDown(0) && !holdingMouse)
            {
                holdingMouse = true;
                StartCoroutine(PerformAttack());
            }
        }

        if (Input.GetMouseButtonUp(0) && holdingMouse)
        {
            holdingMouse = false;
        }
    }


    private void RotateAttackHitboxTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector2 direction = (mousePosition - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        attackHitbox.transform.rotation = Quaternion.Euler(0, 0, angle);

        float hitboxDistance = 1.5f;
        attackHitbox.transform.position = transform.position + (Vector3)direction * hitboxDistance;
    }

    IEnumerator PerformAttack()
    {
        canAttack = false;

        attackHitbox.SetActive(true);

        yield return new WaitForSeconds(attackDuration);

        attackHitbox.SetActive(false);

        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Interactable>(out var interactable))
        {
            switch (interactable.type) 
            { 
                case InteractableType.Auto:
                    interactable.Interact();
                    break;
                case InteractableType.Manual:
                    interactable.canInteract = true;
                    interactableTrigged = interactable;
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<Interactable>(out var interactable))
        {
            switch (interactable.type)
            {
                case InteractableType.Auto:
                    break;
                case InteractableType.Manual:
                    interactable.canInteract = false;
                    interactableTrigged = null;
                    break;
            }
        }
    }

    public void MovementInput(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }
    
    public void Interact(InputAction.CallbackContext context)
    {
        if (interactableTrigged != null)
        {
            interactableTrigged.Interact();
        }
    }

    protected override IEnumerator DeathHandle()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");

        Collider2D collider = gameObject.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        PlayerController playerController = gameObject.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        yield return new WaitForSeconds(1.5f);

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
