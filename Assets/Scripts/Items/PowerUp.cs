using UnityEngine;

public class PowerUp : Interactable
{
    public PowerUps powerUpType;
    [SerializeField] float powerUpValue;

    public override void Interact()
    {
        switch (powerUpType) 
        { 
            case PowerUps.Speed:
                PlayerController.instance.speed += powerUpValue; 
                break;
            case PowerUps.Healing:
                PlayerController.instance.TakeHeal(powerUpValue);
                break;
            case PowerUps.IncrementHealth:
                PlayerController.instance.maxHealth = powerUpValue;
                PlayerController.instance.TakeHeal(PlayerController.instance.maxHealth);
                break;
            case PowerUps.Force:
                break;
        }

        Destroy(gameObject);
    }
}
