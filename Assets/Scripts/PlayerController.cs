using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour {
    [SerializeField] private SOWeapon startingWeapon = default;
    private SOWeapon currentWeapon;
    
    [SerializeField] private GameObject playerShotgun;
    [SerializeField] private Crosshair crosshair;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private DynNumber shootCounter;
    [SerializeField] private WeaponMisc weaponMisc;
    
    private Rigidbody2D rb;
    private Vector3 previousMousePosition;
    private Vector2 mouseDir;
    private bool hasShotgun;
    private bool canShoot;
    private int currentShells = 5;
    public bool canReload;
    
    private void OnEnable()
    {
        InputSystem.onAfterUpdate += RotateWeaponOnMouseMove;
    }

    private void OnDisable()
    {
        InputSystem.onAfterUpdate -= RotateWeaponOnMouseMove;
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerShotgun.SetActive(false);
        previousMousePosition = GetMouseWorldPosition();
        InitializeWeapon(startingWeapon);
    }
    private void InitializeWeapon(SOWeapon weapon) {
        currentWeapon = Instantiate(weapon);
    }
    public void PickupWeapon() {
       
        Debug.Log("Player picked up the shotgun!");
        EnableWeapon();
    }
    public void DropWeapon() {
        if (!hasShotgun) return;
        Debug.Log(("Player dropped shotgun"));
        DisableWeapon();
       
    }

    private void EnableWeapon()
    {
        playerShotgun.SetActive(true);
        hasShotgun = true;
        canShoot = true;
        canReload = true;
    }
    private void DisableWeapon() {
        playerShotgun.SetActive(false);
        hasShotgun = false;
        canShoot = false;
        canReload = false;
    }

    private void RotateWeaponOnMouseMove()
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
        
        if (ctx.performed && (canReload && (currentShells != currentWeapon.totalAmmo))) {
            StartCoroutine(ReloadWithDelay());
        }
    } 
    IEnumerator ReloadWithDelay() {
        canReload = false;
        canShoot = false;
        weaponMisc.PlayReloadSound();
        //shotgunMisc.playReloadAnimation();
        yield return new WaitForSeconds(.5f); 
        currentShells = currentWeapon.totalAmmo;
        shootCounter.SetNumber(currentWeapon.totalAmmo);
        canShoot = true;

    }
    public void OnFire(InputAction.CallbackContext ctx)
    {
        if (!hasShotgun) return;

        if (ctx.performed) {
            if (!canShoot) return;
            if (currentShells > 0) {
                mouseDir = mouseDir.normalized;
                rb.AddForce(-mouseDir * currentWeapon.shootStrength);
                currentShells--;
                shootCounter.DecreaseNumber();
                weaponMisc.AnimatePoof(default);
                weaponMisc.PlayShotSound();
            }
        }
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
