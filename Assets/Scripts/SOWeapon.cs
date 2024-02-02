
using UnityEngine;

[CreateAssetMenu(fileName = "SOWeapon", menuName = "SOWeapon")]
public class SOWeapon : ScriptableObject {
    public Texture2D texture;
    public ParticleSystem particleSystem;
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public int shotDamage;
    public int totalAmmo;
    public float shootStrength;
}