using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Managers
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInput))]
    public class Movement2D : MonoBehaviour
    {
        [Serializable] public class MoveEvent : UnityEvent<Vector2> {}

        public bool CanMove { get; set; } = true;
        public MoveEvent OnMove;
        public Vector2 Velocity => _velocity;
        public Vector2 NormalizedVelocity => _normalizedVelocity;
        public BoxCollider2D Collider => _collider;
        public Rigidbody2D Rigidbody => _rigidbody;

        private Vector2 _velocity;
        private Vector2 _normalizedVelocity;
        private BoxCollider2D _collider;
        private Rigidbody2D _rigidbody;
        

        [SerializeField] protected float speedMultiplier = 5;

        // Start is called before the first frame update
        void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Move(Vector2 direction)
        {
            _velocity = direction * speedMultiplier;
            _normalizedVelocity = _velocity.normalized;
            OnMove.Invoke(_velocity);
        }

        public void FixedUpdate()
        {
            Rigidbody.velocity = Velocity;
        }
    }
}