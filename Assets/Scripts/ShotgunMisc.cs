using System;
using UnityEngine;

public class ShotgunMisc : MonoBehaviour {
    private Animation animationComponent;
    private ParticleSystem particleSystemComponent;
    private void Awake() {
        animationComponent = GetComponent<Animation>();
        particleSystemComponent = GetComponent<ParticleSystem>();
    }
    public void AnimatePoof() {
        ParticleSystem.MainModule mainModule = particleSystemComponent.main;
        Vector3 initialVelocity = transform.forward * 10f;
        mainModule.startSpeed = new ParticleSystem.MinMaxCurve(0f, initialVelocity.magnitude);
        animationComponent.Rewind();
        animationComponent.Play();
        particleSystemComponent.Simulate(1.5f);

    }
    public void PlayReloadSound() { 
        throw new NotImplementedException();
    }
    public void PlayShotSound() {
        throw new NotImplementedException();
    }
}
