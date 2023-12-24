using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int totalShells = 5;
    [SerializeField] private float shootStrength;
    
    [SerializeField] private GameObject playerShotgun;
    [SerializeField] private Crosshair crosshair;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private DynNumber shootCounter;
    [SerializeField] private ShotgunMisc shotgunMisc;
    private Rigidbody2D rb;
    private Vector3 previousMousePosition;
    private Vector2 mouseDir;
    private bool hasShotgun;
    private int currentShells = 5;
    public bool canReload = true;
    
    private void OnEnable()
    {
        InputSystem.onAfterUpdate += RotateShotgunOnMouseMove;
    }

    private void OnDisable()
    {
        InputSystem.onAfterUpdate -= RotateShotgunOnMouseMove;
    }

    private void Awake() {
        shootStrength *= 50000;
        rb = GetComponent<Rigidbody2D>();
        playerShotgun.SetActive(false);
        previousMousePosition = GetMouseWorldPosition();
    }

    public void PickupShotgun() {
        if (hasShotgun) shootStrength *= 2;
        
        Debug.Log("Player picked up the shotgun!");
        EnableShotgun();
    }
    public void DropShotgun() {
        if (!hasShotgun) return;
        Debug.Log(("Player dropped shotgun"));
        DisableShotgun();
        canReload = false;
    }

    private void EnableShotgun()
    {
        playerShotgun.SetActive(true);
        hasShotgun = true;
    }
    private void DisableShotgun() {
        playerShotgun.SetActive(false);
        hasShotgun = false;
    }

    private void RotateShotgunOnMouseMove()
    {
        Mouse mouse = Mouse.current;

        if (mouse == null)
        {
            Debug.LogError("Mouse is null");
            return;
        }

        Vector3 currentMousePosition = GetMouseWorldPosition();
        crosshair.SetCrosshair(currentMousePosition);

        if (currentMousePosition == previousMousePosition)
        {
            return;
        }

        Vector3 playerPosition = transform.position;
        mouseDir = currentMousePosition - playerPosition;

        float angle = Vector2.SignedAngle(Vector2.right, mouseDir);
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        playerShotgun.transform.rotation = rotation;

        previousMousePosition = currentMousePosition;

        // Update the line renderer to draw a line from the player to the crosshair
        UpdateLineRenderer();
    }
    public void OnReload(InputAction.CallbackContext ctx) {
        
        if (ctx.performed && canReload) {
                currentShells = totalShells;
                shootCounter.SetNumber(totalShells);
                //shotgunMisc.PlayReloadSound();
        }
    } 
    public void OnFire(InputAction.CallbackContext ctx)
    {
        if (!hasShotgun) return;

        if (ctx.performed) {
            Shoot();
        }
    }
    private void Shoot() {

        if (currentShells > 0) {
            mouseDir = mouseDir.normalized;
            rb.AddForce(-mouseDir * shootStrength);
            currentShells--;
            shootCounter.DecreaseNumber();
            shotgunMisc.AnimatePoof();
            //shotgunMisc.PlayShotSound();
        }
        Debug.Log("current shells: " + currentShells);
    }
    private void AddShells(int shells) {
        currentShells += shells;
    }
    private void UpdateLineRenderer() {
        // Set the positions for the line renderer
        lineRenderer.SetPosition(1, transform.position);
        if (Camera.main != null)
            lineRenderer.SetPosition(0, Camera.main.ScreenToWorldPoint(crosshair.transform.position));
    }

    private static Vector3 GetMouseWorldPosition() {
        if (Camera.main != null)
            return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        return default;
    }
}
