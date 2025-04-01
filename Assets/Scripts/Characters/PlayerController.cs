using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : CharacterController
{
    public static PlayerController instance;

    [SerializeField] Interactable interactableTrigged;

    [Header("Attack variables")]
    public GameObject weapon;
    public Item equippedItem;
    private bool canAttack = true;
    private bool isReloading;
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

        if (weapon == null)
        {
            weapon.SetActive(false);
            canAttack = false;
        }
    }

    private void Update()
    {
        if (weapon != null)
        {
            RotateWeaponTowardsMouse();

            if (holdingMouse && !canAttack) return;
            if (Input.GetMouseButtonDown(0))
            {
                holdingMouse = true;
                Attacking();
            }
        }

        if (Input.GetMouseButtonUp(0) && holdingMouse)
        {
            holdingMouse = false;
        }
    }

    #region AttackSystem
    private void RotateWeaponTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector2 direction = (mousePosition - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        weapon.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void Attacking()
    {
        canAttack = false;
        weapon.GetComponent<PlayerWeapon>().Attack();

        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        if (equippedItem is Weapons _weapon)
        {
            isReloading = true;
            yield return new WaitForSeconds(_weapon.cooldown);
            isReloading = false;
            canAttack = true;
        }
    }

    public void UpdateEquippedItem(Item _newItem)
    {
        if (_newItem == null)
        {
            weapon.SetActive(false);
            canAttack = false;
        }
        else
        {
            if (_newItem is not Weapons) return;
            weapon.SetActive(true);
            if (!isReloading) canAttack = true;
        }
        if (_newItem is Weapons _weapon) weapon.GetComponent<PlayerWeapon>().ChangeWeapon(_weapon);
        equippedItem = _newItem;
    }
    #endregion

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

        speed = 0;

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
