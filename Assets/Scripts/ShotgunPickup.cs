using UnityEngine;

public class ShotgunPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {

        if (!other.CompareTag("Player"))
            return;
        PlayerController playerController = other.GetComponent<PlayerController>();
        
        if (playerController == null) {
            Debug.LogError("No script PlayerController found");
            return;
        }
        
        playerController.PickupShotgun();
        
        Destroy(gameObject);
    }
}
