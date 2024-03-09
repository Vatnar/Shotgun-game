using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Handles them weapons
/// </summary>
public class WeaponMisc : MonoBehaviour {
    [SerializeField] private List<WeaponData> weaponsList = new List<WeaponData>();

    public WeaponData GetWeaponByIndex(int index)
    {
        if (index >= 0 && index < weaponsList.Count)
        {
            return weaponsList[index];
        }
        else
        {
            Debug.LogWarning("Invalid weapon index: " + index);
            return null;
        }
    }
    private ParticleSystem CurrInstance;

    /// <summary>
    /// Instantiates and plays the particle system for the weapon, then destroys it after a delay.
    /// </summary>
    public void AnimatePoof(int index) {
        var weapon = GetWeaponByIndex(index);

        CurrInstance = Instantiate(weapon.particleSystem, transform.position, transform.rotation);
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
        //reloadSource.time = 1.5f;
        //reloadSource.Play();
    }

    /// <summary>
    /// Plays the shot sound.
    /// </summary>
    public void PlayShotSound(){
        //shotSource.Play();
    }
}
