using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class WeaponData
{
    public string weaponName;
    public int strength;
    public AudioClip shotSound;
    public AudioClip reloadSound;
    public ParticleSystem particleSystem;
    public Sprite sprite;
    public WeaponData maxAmmo;
}
