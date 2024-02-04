using UnityEngine;
/// <summary>
/// Handles the particle system and animation for the weapon.
/// </summary>
public class WeaponMisc : MonoBehaviour {
    [SerializeField] private ParticleSystem particleSystemComponent;
    [SerializeField] private AudioSource shotSource;
    [SerializeField] private AudioSource reloadSource;

    private ParticleSystem CurrInstance;

    /// <summary>
    /// Instantiates and plays the particle system for the weapon, then destroys it after a delay.
    /// </summary>
    public void AnimatePoof(SOWeapon weapon) {
        var parTransform = particleSystemComponent.transform;
        CurrInstance = Instantiate(weapon.particleSystem, parTransform.position, parTransform.rotation);
        CurrInstance.Play();
        StartCoroutine(DestroyAfterDelay(CurrInstance, 3));
    }

    System.Collections.IEnumerator DestroyAfterDelay(Component obj, float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(obj.gameObject);
    }

    /// <summary>
    /// Plays the reload sound.
    /// </summary>
    public void PlayReloadSound() {
        reloadSource.time = 1.5f;
        reloadSource.Play();
    }

    /// <summary>
    /// Plays the shot sound.
    /// </summary>
    public void PlayShotSound(){
        shotSource.Play();
    }
}
