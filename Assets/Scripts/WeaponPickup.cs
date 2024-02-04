using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    /// <summary>
    /// This function is called when the Collider other enters the trigger.
    /// If the Collider is not tagged as "Player", the function returns early.
    /// It then gets the PlayerController component from the Collider.
    /// If the PlayerController component is not found, it logs an error and returns.
    /// Otherwise, it calls the PickupWeapon method of the PlayerController and destroys the current object.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other) {

        if (!other.CompareTag("Player")) {
            return;
        }
        var playerController = other.GetComponent<PlayerController>();

        if (playerController == null) {
            Debug.LogError("No script PlayerController found");
            return;
        }

        playerController.PickupWeapon();

        Destroy(gameObject);
    }
}
