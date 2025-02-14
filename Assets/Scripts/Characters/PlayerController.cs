using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterController
{
    public static PlayerController instance;

    [SerializeField] Interactable interactableTrigged;

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
}
