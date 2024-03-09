using UnityEngine;
[System.Serializable]
public class WeaponData
{
    public string weaponName;
    public int strength;
    public AudioClip shotSound;
    public AudioClip reloadSound;
    public ParticleSystem poofParticle;
    public Sprite sprite;
}
