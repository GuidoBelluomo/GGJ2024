using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D.Animation;
using Weapons;

namespace Managers
{
    public class WeaponManager : MonoBehaviour
    {
        public SpriteLibraryAsset EffectLibrary => _effectLibrary;

        [SerializeField] private Transform weaponContainer;
        [SerializeField] private WeaponBase currentWeapon;
        [SerializeField] private SpriteLibraryAsset friendlyEffectLibrary;
        [SerializeField] private SpriteLibraryAsset[] enemyEffectLibraries;
        private SpriteLibraryAsset _effectLibrary;
        private AimManager _aimManager;

        void Awake()
        {
            RefreshEffectLibrary();

            _aimManager = GetComponent<AimManager>();
        }

        public void RefreshEffectLibrary()
        {
            if (CompareTag("Enemy"))
            {
                _effectLibrary = enemyEffectLibraries[Random.Range(0, enemyEffectLibraries.Length)];
            }
            else
            {
                _effectLibrary = friendlyEffectLibrary;
            }
        }

        public void PrimaryFire()
        {
            if (currentWeapon)
            {
                currentWeapon.OnPrimaryFire();
            }
        }

        public bool IsPrimaryFireAutomatic()
        {
            if (currentWeapon)
            {
                return currentWeapon.Automatic;
            }

            return false;
        }

        public void PrimaryFireReleased()
        {
            if (currentWeapon)
            {
                currentWeapon.OnPrimaryFireReleased();
            }
        }

        public void Update()
        {
            weaponContainer.transform.right = _aimManager.GetAimDirection();
        }

        public void DestroyWeapon()
        {
            if (currentWeapon)
            {
                Destroy(currentWeapon.gameObject);
            }
        }

        public void SetCurrentWeapon(WeaponBase weaponBase)
        {
            currentWeapon = weaponBase;
        }
    }
}
