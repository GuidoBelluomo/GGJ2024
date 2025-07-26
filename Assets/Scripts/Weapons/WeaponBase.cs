using System;
using Managers;
using Physics;
using Pickups;
using Projectiles;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D.Animation;
using Random = Unity.Mathematics.Random;

namespace Weapons
{
    public class WeaponBase : MonoBehaviour
    {
        public enum WeaponType
        {
            Firearm,
            Melee
        }

        public float Rps => rps;
        public bool Automatic => automatic;
        public GameObject ProjectileClass => projectileClass;
        public int Ammo => ammo;
        public WeaponType Type => type;

        protected AimManager AimManager =>
            _aimManager ? _aimManager : _aimManager = transform.parent.parent.GetComponent<AimManager>();
        protected WeaponManager WeaponManager => _weaponManager
            ? _weaponManager
            : _weaponManager = transform.parent.parent.GetComponent<WeaponManager>();

        protected ArcOverlapper MeleeOverlapper => _meleeOverlapper
            ? _meleeOverlapper
            : _meleeOverlapper = transform.parent.parent.GetComponent<ArcOverlapper>();

        [SerializeField] GameObject projectileClass;
        [SerializeField] GameObject muzzleFlash;
        [SerializeField] GameObject sprite;
        [SerializeField] float weaponDamage = 25;
        [SerializeField] float rps;
        [SerializeField] float bulletConeAngle;
        [SerializeField] private float projectileVelocity;
        [SerializeField] float bulletVelocityVariationPercentage;
        [SerializeField] int bulletsPerShot = 1;
        [SerializeField] bool automatic;
        [SerializeField] int ammo = 0;
        [SerializeField] WeaponType type = WeaponType.Firearm;
        [SerializeField] float meleeRange = 1;
        [SerializeField] float meleeAngle = 90;
        [SerializeField] private AudioClipCollection primaryFireSounds;

        private float _nextShotDelay = -999;
        private bool _holdingPrimaryFire;
        private float _oldMeleeRange;
        private float _oldMeleeAngle;
        private AimManager _aimManager; // Don't use _aimManager, not even internally
        private WeaponManager _weaponManager;
        private ArcOverlapper _meleeOverlapper;
        private SpriteLibrary _muzzleFlashSpriteLibrary;
        private Animation _muzzleFlashAnimation;
        private Animator _animator;
        private static readonly int Attack = Animator.StringToHash("Attack");
        

        private void Awake()
        {
            _oldMeleeAngle = meleeAngle;
            _oldMeleeRange = meleeRange;
            _animator = GetComponent<Animator>();
            
            if (muzzleFlash)
            {
                _muzzleFlashSpriteLibrary = muzzleFlash.GetComponent<SpriteLibrary>();
                _muzzleFlashAnimation = muzzleFlash.GetComponent<Animation>();
            }
            
            GameObject tempObject = new GameObject("MeleeOverlapper")
            {
                transform =
                {
                    parent = transform,
                    localPosition = Vector3.zero
                }
            };

            if (type == WeaponType.Melee)
            {
                tempObject.layer = LayerMask.NameToLayer("Melee");
                _meleeOverlapper = tempObject.AddComponent<ArcOverlapper>();
                _meleeOverlapper.Angle = meleeAngle;
                _meleeOverlapper.Radius = meleeRange;
            }
        }

        public void OnPrimaryFire()
        {
            if (!Automatic && _holdingPrimaryFire)
            {
                return;
            }

            if (_nextShotDelay <= 0)
            {
                _nextShotDelay = 1 / Rps;
                DoPrimaryFire();
            }

            _holdingPrimaryFire = true;
        }

        public virtual void DoPrimaryFire()
        {
            if (WeaponManager.gameObject.CompareTag("Player"))
            {
                SoundManager.PlayUISound(primaryFireSounds.GetRandomClipOrDefault());
            }
            else
            {
                SoundManager.Play2DSpatialSound(AimManager.transform.position, primaryFireSounds.GetRandomClipOrDefault());
            }
            AttackAnimation();

            if (Type == WeaponType.Firearm)
            {
                for (int i = 0; i < bulletsPerShot; i++)
                {
                    var aimDirection = AimManager.GetAimDirection();
                    var transformPosition = AimManager.transform.position;
                    transformPosition.z = 0;
                    ProjectileBase.CreateProjectile(WeaponManager.gameObject, projectileClass, muzzleFlash.transform.position,
                        aimDirection, weaponDamage, projectileVelocity, bulletVelocityVariationPercentage,
                        bulletConeAngle, WeaponManager.EffectLibrary, WeaponManager.tag);
                }

                MuzzleFlash();
            }
            else if (Type == WeaponType.Melee)
            {
                if (MeleeOverlapper)
                {
                    foreach (var overlappingObject in MeleeOverlapper.GetOverlappingObjects())
                    {
                        var healthManager = overlappingObject.GetComponent<HealthManager>();
                        if (healthManager)
                        {
                            healthManager.ApplyDamage(weaponDamage);
                        }
                    }
                }

                SwingEffect();
            }
        }

        private void AttackAnimation()
        {
            if (_animator)
            {
                _animator.SetTrigger(Attack);
            }
        }

        private void SwingEffect()
        {
        }

        private void MuzzleFlash()
        {
            _muzzleFlashSpriteLibrary.spriteLibraryAsset = _weaponManager.EffectLibrary;
            _muzzleFlashAnimation.Play();
        }

        public void OnPrimaryFireReleased()
        {
            _holdingPrimaryFire = false;
        }

        private void Update()
        {
            if (_nextShotDelay > 0)
            {
                _nextShotDelay -= Time.deltaTime;
            }

            if (_oldMeleeRange != meleeRange || _oldMeleeAngle != meleeAngle)
            {
                _meleeOverlapper.Angle = meleeAngle;
                _meleeOverlapper.Radius = meleeRange;
                _oldMeleeAngle = meleeAngle;
                _oldMeleeRange = meleeRange;
            }

            if (transform.eulerAngles.z is <= 90 and >= 0 or <= 360 and >= 270)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, -1, 1);
            }
        }
    }
}