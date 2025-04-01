using Character;
using Stats;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private ProjectileStats stats;
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void Init(Vector3 shootingPointForward)
        {
            _rb.linearVelocity = shootingPointForward * stats.speed;
            Destroy(gameObject, stats.lifeTime);
        }

        private void OnCollisionEnter(Collision other)
        {
            Rigidbody rb = other.rigidbody;
            if (rb && rb.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(stats.damage);
              
            }
            
            ParticleSystem ps = Instantiate(stats.hitParticle, transform.position, Quaternion.identity);
            Destroy(ps.gameObject, ps.main.duration);
            Destroy(gameObject);
        }
    }
}
