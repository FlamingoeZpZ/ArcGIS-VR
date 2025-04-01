using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(fileName = "ProjectileStats", menuName = "Scriptable Objects/ProjectileStats")]
    public class ProjectileStats : ScriptableObject
    {
        public ParticleSystem hitParticle;
        public float damage = 25;
        public float speed = 50;
        public float lifeTime = 2;
    }
}
