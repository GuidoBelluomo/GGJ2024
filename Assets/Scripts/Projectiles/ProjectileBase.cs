using System;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Random = Unity.Mathematics.Random;

namespace Projectiles
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ProjectileBase : MonoBehaviour
    {
        [SerializeField] private GameObject bulletImpact;
        private float _velocity;
        private Rigidbody2D _rigidbody;
        private float _lifetime;
        private float _damage;
        private SpriteLibraryAsset _spriteLibraryAsset;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.gravityScale = 0;
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            _rigidbody.drag = 0;
            _rigidbody.angularDrag = 0;
            _rigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        private void Start()
        {
            _rigidbody.AddForce(_velocity * transform.right, ForceMode2D.Impulse);
        }

        private void Update()
        {
            _lifetime -= Time.deltaTime;
            if (_lifetime <= 0)
            {
                Destroy(gameObject);
            }
        }


        public static void CreateProjectile(
            GameObject owner,
            GameObject projectileClass,
            Vector3 position,
            Vector3 direction,
            float projectileDamage,
            float projectileVelocity,
            float velocityVariationPercentage,
            float bulletConeAngle,
            SpriteLibraryAsset spriteLibraryAsset,
            string projectileTag,
            float lifetime = 10)
        {
            GameObject newObj = Instantiate(projectileClass, position, Quaternion.identity);
            newObj.transform.right = direction;
            var bulletAngle = newObj.transform.eulerAngles;
            bulletAngle.z += UnityEngine.Random.Range(-bulletConeAngle, bulletConeAngle);
            newObj.transform.eulerAngles = bulletAngle;
            var projectileBase = newObj.GetComponent<ProjectileBase>();
            projectileBase._damage = projectileDamage;
            projectileBase._velocity = projectileVelocity;
            projectileBase._velocity *=
                1 + UnityEngine.Random.Range(-velocityVariationPercentage, velocityVariationPercentage) / 100;
            foreach (var collider2D in owner.GetComponentsInChildren<Collider2D>())
            {
                Physics2D.IgnoreCollision(collider2D, newObj.GetComponentInChildren<Collider2D>());
            }

            projectileBase._lifetime = lifetime;
            projectileBase._spriteLibraryAsset = spriteLibraryAsset;
            projectileBase.GetComponent<SpriteLibrary>().spriteLibraryAsset = spriteLibraryAsset;
            newObj.tag = projectileTag;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var healthManager = collision.GetComponent<HealthManager>();
            if (healthManager)
            {
                if (healthManager.enabled && !healthManager.CompareTag(tag))
                {
                    if (!healthManager.IsDead || healthManager.AbsorbBulletsWhenDead)
                    {
                        Destroy(gameObject);
                        BulletImpactEffect();
                    }

                    healthManager.ApplyDamage(_damage);
                }
            }
            else
            {
                Destroy(gameObject);
                BulletImpactEffect();
            }
        }

        private void BulletImpactEffect()
        {
            GameObject go = Instantiate(bulletImpact);
            var currentTransform = transform;
            go.transform.position = currentTransform.position;
            go.transform.rotation = currentTransform.rotation;
            go.GetComponent<SpriteLibrary>().spriteLibraryAsset = _spriteLibraryAsset;
            go.GetComponent<Animation>().Play();
        }
    }
}