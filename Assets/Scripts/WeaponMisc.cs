using UnityEngine;
public class WeaponMisc : MonoBehaviour {
    //[SerializeField] private Animation animationComponent;
    [SerializeField] private ParticleSystem particleSystemComponent;
    [SerializeField] private AudioSource shotSource;
    [SerializeField] private AudioSource reloadSource;
    
    private ParticleSystem currInstance;
    public void AnimatePoof(SOWeapon weapon) {
        Transform parTransform = particleSystemComponent.transform;
        currInstance = Instantiate(weapon.particleSystem, parTransform.position,
            parTransform.rotation);
        currInstance.Play();
        StartCoroutine(DestroyAfterDelay(currInstance, 3));
    }



    private System.Collections.IEnumerator DestroyAfterDelay(ParticleSystem obj, float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(obj.gameObject);
    }
    public void PlayReloadSound() {
        reloadSource.time = 1.5f;
        reloadSource.Play();
    }
    public void PlayShotSound(){
    shotSource.Play();
    }
}
