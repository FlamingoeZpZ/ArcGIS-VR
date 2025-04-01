using System.Collections;
using Stats;
using UnityEngine;
using UnityEngine.AI;
using Weapons;

namespace Character
{
    public class GooberWarrior : MonoBehaviour, IDamagable
    {
        [SerializeField] private CharacterStats stats;
        [SerializeField] private Weapon w;
        [SerializeField] private AnimationClip deathAnim;
        private Animator _animator;
        private NavMeshAgent _ai;
        private Transform _target;
        private float _currentHealth;

        private void Awake()
        {
            _ai = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();

            w.OnFired += () => _animator.SetTrigger(StaticUtility.ShootAnimID);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _target = PlayerWarrior.instance.transform;
            Respawn();
        }

        public void Respawn()
        {
            transform.position = AIManager.instance.GetGoobyPoint();
            
            _currentHealth = stats.health;
            enabled = true;
            _ai.enabled = true;
            _animator.enabled = true;
            _animator.Rebind();
        }

        // Update is called once per frame
        void Update()
        {

            if (HasLineOfSight())
            {
                w.BeginShooting();
                _ai.isStopped = true;
                transform.LookAt(_target, Vector3.up);
            }
            else
            {
                w.StopShooting();
                _ai.isStopped = false;
                _ai.SetDestination(_target.position);
            }
            
            _animator.SetBool(StaticUtility.IsMovingAnimID, !_ai.isStopped && _ai.velocity != Vector3.zero);

        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!_target) return;
            Vector3 distance = _target.position - transform.position;
            float dist = distance.magnitude;
            Vector3 norm = distance / dist;
            Debug.DrawRay(transform.position, norm * dist, HasLineOfSight()?Color.green:Color.red);
        }
        #endif

        private bool HasLineOfSight()
        {
            Vector3 distance = _target.position - transform.position;
            float dist = distance.magnitude;

            if (dist <= Mathf.Epsilon) return false; // Prevent division by zero
            if (dist > stats.minDistance) return false;

            return Physics.Raycast(transform.position, distance.normalized, out RaycastHit hit, dist, StaticUtility.BlockLayers)
                   && ((StaticUtility.PlayerLayer & (1 << hit.transform.gameObject.layer)) != 0);
        }

        public void TakeDamage(float damage)
        {
            if (!enabled) return;
            
            _currentHealth -= damage;
            if (_currentHealth < 0)
            {
                _animator.SetTrigger(StaticUtility.DieAnimID);
                Die();
            }
            else
            {
                _animator.SetTrigger(StaticUtility.HurtAnimID);
            }
        }

        public void Die()
        {
            StartCoroutine(Sink());
        }

        private IEnumerator Sink()
        {
            w.StopShooting();

            enabled = false;
            _ai.enabled = false;
            yield return new WaitForSeconds(deathAnim.length * 5);
            Respawn();
            
        }
    }
}
