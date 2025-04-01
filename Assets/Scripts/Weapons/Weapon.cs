using System;
using System.Collections;
using Stats;
using UnityEngine;

namespace Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Transform shootingPoint;
        [SerializeField] private WeaponStats stats;
        [SerializeField] private ParticleSystem muzzleFlash;
        [SerializeField] private int pelletsPerShot = 1;
        [SerializeField] private float spreadAngle = 15f;
        private float _currentCooldown;
        private Coroutine _coroutine;
        private bool _isShooting;

        public Action OnFired;

        private void Awake()
        {
            _currentCooldown = stats.timeBetweenShots;
        }

        public void BeginShooting()
        {
            _coroutine ??= StartCoroutine(ShootLoop());
            _isShooting = true;
        }

        public void StopShooting()
        {
            _isShooting = false;
        }

        private IEnumerator ShootLoop()
        {
            while (_isShooting)
            {
                while (_currentCooldown < stats.timeBetweenShots)
                {
                    _currentCooldown += Time.deltaTime;
                    yield return null;
                }

                if (!_isShooting)
                {
                    _coroutine = null;
                    yield break;
                }
                Shoot();
                _currentCooldown = 0;
            }
            _coroutine = null;
        }

        public void Shoot()
        {
            muzzleFlash.Play();
            for (int i = 0; i < pelletsPerShot; i++)
            {
                Vector3 direction = GetSpreadDirection();
                Projectile p = Instantiate(stats.projectilePrefab, shootingPoint.position, Quaternion.LookRotation(direction));
                p.Init(direction);
            }
            OnFired?.Invoke();
        }

        private Vector3 GetSpreadDirection()
        {
            float yaw = UnityEngine.Random.Range(-spreadAngle, spreadAngle);
            float pitch = UnityEngine.Random.Range(-spreadAngle, spreadAngle);
            Quaternion spreadRotation = Quaternion.Euler(pitch, yaw, 0);
            return spreadRotation * shootingPoint.forward;
        }
    }
}
