using UnityEngine;

public class ShotgunMisc : MonoBehaviour {
    //[SerializeField] private Animation animationComponent;
    [SerializeField] private ParticleSystem particleSystemComponent;
    [SerializeField] private AudioSource shotSource;
    [SerializeField] private AudioSource reloadSource;
    
    private ParticleSystem currInstance;
    public void AnimatePoof() {
        Transform parTransform = particleSystemComponent.transform;
        currInstance = Instantiate(particleSystemComponent, parTransform.position,
            parTransform.rotation);
        Debug.Log("playing instantiated system");
        currInstance.Play();
        StartCoroutine(DestroyAfterDelay(currInstance, 3));
    }



    private System.Collections.IEnumerator DestroyAfterDelay(ParticleSystem obj, float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(obj.gameObject);
    }
    public void PlayReloadSound() {
        reloadSource.Play();
    }
    public void PlayShotSound(){
    shotSource.Play();
    }
}
