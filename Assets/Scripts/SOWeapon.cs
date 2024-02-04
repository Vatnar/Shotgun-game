
using UnityEngine;

/// <summary>
/// Represents a ScriptableObject for defining weapon properties such as texture, particle system, sound clips, damage, ammo, and shoot strength.
/// </summary>
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
