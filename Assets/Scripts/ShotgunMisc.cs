using System;
using UnityEngine;

public class ShotgunMisc : MonoBehaviour {
    [SerializeField] private Animation animationComponent;
    [SerializeField] private ParticleSystem particleSystemComponent;
    
    public void AnimatePoof() {
        ParticleSystem particleSystemInstance = Instantiate(particleSystemComponent);
        Animation animationInstance = Instantiate(animationComponent);
        RunParticleSystem(particleSystemInstance, animationInstance);
    }
    
    
    private void RunParticleSystem(ParticleSystem particles, Animation animations)
    {
        ParticleSystem.MainModule mainModule = particles.main;
        particles.transform.position = particleSystemComponent.transform.position;
        

        particles.transform.rotation = Quaternion.LookRotation(transform.parent.right);


        mainModule.startSpeed = transform.parent.right.magnitude;
        animations.Rewind();
        Time.timeScale = 1f;
        animations.Play();
        particles.Play();

        float cleanupDelay = mainModule.duration + 0.5f; 

        StartCoroutine(DestroyAfterDelay(particles.gameObject, cleanupDelay));
        StartCoroutine(DestroyAfterDelay(animations.gameObject, cleanupDelay));
    }


    private System.Collections.IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }
    public void PlayReloadSound() { 
        throw new NotImplementedException();
    }
    public void PlayShotSound() {
        throw new NotImplementedException();
    }
}
