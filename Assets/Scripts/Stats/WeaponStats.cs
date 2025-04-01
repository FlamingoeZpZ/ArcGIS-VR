using UnityEngine;
using Weapons;

namespace Stats
{
    [CreateAssetMenu(fileName = "WeaponStats", menuName = "Scriptable Objects/WeaponStats")]
    public class WeaponStats : ScriptableObject
    {
        public float timeBetweenShots = 0.5f;
        public Projectile projectilePrefab;
    }
}
