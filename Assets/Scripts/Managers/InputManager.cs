using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        private Movement2D _movement2D;
        private AimManager _aimManager;
        private WeaponManager _weaponManager;
        private PlayerInput _playerInput;

        private void Awake()
        {
            _movement2D = GetComponent<Movement2D>();
            _aimManager = GetComponent<AimManager>();
            _weaponManager = GetComponent<WeaponManager>();
            _playerInput = GetComponent<PlayerInput>();
        }

        void OnMove(InputValue value)
        {
            var movement = value.Get<Vector2>();
            _movement2D.Move(movement);
        }

        void OnAim(InputValue value)
        {
            var dir = value.Get<Vector2>();
            _aimManager.Aim(dir, AimManager.AimMode.Stick);
        }

        void OnMouseAim(InputValue value)
        {
            var mousePos = value.Get<Vector2>();
            _aimManager.Aim(mousePos, AimManager.AimMode.Mouse);
        }

        void OnFire(InputValue value) { } // Do not delete, unity complains

        private void Update()
        {
            if (_weaponManager && _playerInput.enabled)
            {
                if (_playerInput.actions["fire"].IsPressed())
                {
                    _weaponManager.PrimaryFire();
                }
        
                if (_playerInput.actions["fire"].WasReleasedThisFrame())
                {
                    _weaponManager.PrimaryFireReleased();
                }
            }
        }
    }
}
