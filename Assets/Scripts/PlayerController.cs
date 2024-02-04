using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [Serializable]
    public struct RefStruct {
        public GameObject playerShotgun;
        public Crosshair crosshair;
        public LineRenderer lineRenderer;
        public DynNumber shootCounter;
        public WeaponMisc weaponMisc;
        public SOWeapon startingWeapon;
    }

    [SerializeField] RefStruct references;


    private SOWeapon CurrentWeapon;
    private Rigidbody2D Rb;
    private Vector3 PreviousMousePosition;
    private Vector2 MouseDir;
    private bool HasShotgun;
    private bool CanShoot;
    private int CurrentShells = 5;
    public bool canReload;

    /// <summary>
    /// Subscribe to the InputSystem's onAfterUpdate event to rotate the weapon based on mouse movement.
    /// </summary>
    private void OnEnable()
    {
        InputSystem.onAfterUpdate += RotateWeaponOnMouseMove;
    }

    /// <summary>
    /// Unsubscribes the RotateWeaponOnMouseMove method from the InputSystem.onAfterUpdate event.
    /// </summary>
    private void OnDisable()
    {
        InputSystem.onAfterUpdate -= RotateWeaponOnMouseMove;
    }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake() {

        references = new RefStruct {
            startingWeapon = default,
        };
        Rb = GetComponent<Rigidbody2D>();
        references.playerShotgun.SetActive(false);
        PreviousMousePosition = GetMouseWorldPosition();
        InitializeWeapon(references.startingWeapon);
    }
    /// <summary>
    /// Initializes the weapon by instantiating the given SOWeapon.
    /// </summary>
    private void InitializeWeapon(SOWeapon weapon) {
        CurrentWeapon = Instantiate(weapon);
    }
    /// <summary>
    /// Picks up the weapon and enables it.
    /// </summary>
    public void PickupWeapon() {

        Debug.Log("Player picked up the shotgun!");
        EnableWeapon();
    }
    /// <summary>
    /// Drops weapon and disables it
    /// </summary>
    public void DropWeapon() {
        if (!HasShotgun) {
            return;
        }
        Debug.Log(("Player dropped shotgun"));
        DisableWeapon();

    }
/// <summary>
/// Enables weapon model. Ticks HasShotgun, CanShoot and CanReload booleans
/// </summary>
    private void EnableWeapon()
    {
        references.playerShotgun.SetActive(true);
        HasShotgun = true;
        CanShoot = true;
        canReload = true;
    }
/// <summary>
/// Disables weapon model. Un-ticks HasShotgun, CanShoot and CanReload booleans
/// </summary>
    private void DisableWeapon() {
        references.playerShotgun.SetActive(false);
        HasShotgun = false;
        CanShoot = false;
        canReload = false;
    }

 /// <summary>
 /// Rotates the weapon based on the movement of the mouse.
 /// Calculates the angle between the mouse position and the player position.
 /// Creates a rotation based on the angle and applies it to the player's shotgun.
 /// Updates the PreviousMousePosition variable.
 /// Updates the line renderer.
 /// </summary>
    private void RotateWeaponOnMouseMove()
    {
        var mouse = Mouse.current;

        if (mouse == null)
        {
            Debug.LogError("Mouse is null");
            return;
        }

        var currentMousePosition = GetMouseWorldPosition();
        references.crosshair.SetCrosshair(currentMousePosition);

        if (currentMousePosition == PreviousMousePosition)
        {
            return;
        }

        var playerPosition = transform.position;
        MouseDir = currentMousePosition - playerPosition;

        var angle = Vector2.SignedAngle(Vector2.right, MouseDir);
        var rotation = Quaternion.Euler(0f, 0f, angle);
        references.playerShotgun.transform.rotation = rotation;

        PreviousMousePosition = currentMousePosition;


        UpdateLineRenderer();
    }
 /// <summary>
 /// Checks if reloading is available and initiates ReloadWithDelay()
 /// </summary>
 /// <param name="ctx"> InputAction.CallbackContext, gives state of input</param>
    public void OnReload(InputAction.CallbackContext ctx) {

        if (ctx.performed && (canReload && (CurrentShells != CurrentWeapon.totalAmmo))) {
            StartCoroutine(ReloadWithDelay());
        }
    }

    /// <summary>
    /// Reloads with a delay and updates the current shells and shoot ability.
    /// </summary>
    IEnumerator ReloadWithDelay() {
        canReload = false;
        CanShoot = false;
        references.weaponMisc.PlayReloadSound();
        //shotgunMisc.playReloadAnimation();
        yield return new WaitForSeconds(.5f);
        CurrentShells = CurrentWeapon.totalAmmo;
        references.shootCounter.SetNumber(CurrentWeapon.totalAmmo);
        CanShoot = true;

    }

    /// <summary>
    /// Checks if firing is available, uses mouse direction to add force to the player's rigidbody
    /// and decreases the number of shells available.
    /// Plays animations and sounds.
    /// </summary>
    /// <param name="ctx"></param>
    public void OnFire(InputAction.CallbackContext ctx)
    {
        if (!HasShotgun) {
            return;
        }

        if (!ctx.performed) {
            return;
        }

        if (!CanShoot) {
            return;
        }
        if (CurrentShells <= 0) {
            return;
        }
        MouseDir = MouseDir.normalized;
        Rb.AddForce(-MouseDir * CurrentWeapon.shootStrength);
        CurrentShells--;
        references.shootCounter.DecreaseNumber();
        references.weaponMisc.AnimatePoof(default);
        references.weaponMisc.PlayShotSound();
    }
    /// <summary>
    /// Adds ammo to the current shells
    /// </summary>
    /// <param name="ammo">ammo to add</param>
    private void AddShells(int ammo) {
        CurrentShells += ammo;
    }
    /// <summary>
    /// Draws line from the player to the crosshair
    /// </summary>
    private void UpdateLineRenderer() {
        // Set the positions for the line renderer
        references.lineRenderer.SetPosition(1, transform.position);
        if (Camera.main != null) {
            references.lineRenderer.SetPosition(0, Camera.main.ScreenToWorldPoint(references.crosshair.transform.position));
        }
    }
/// <summary>
/// Checks if there is an active camera.
/// </summary>
/// <returns> The world position of the mouse.</returns>
    private static Vector3 GetMouseWorldPosition() {
        if (Camera.main != null) {
            return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }

        return default;
    }

}
