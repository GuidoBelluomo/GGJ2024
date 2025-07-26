using System;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Managers
{
    public class PawnAnimationController : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int DeadAnimIndex = Animator.StringToHash("DeadAnimIndex");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private SpriteRenderer _spriteRenderer;
        private static readonly int DieTrigger = Animator.StringToHash("Die");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Die()
        {
            _animator.SetTrigger(DieTrigger);
            _animator.SetBool(Dead, true);
            _animator.SetInteger(DeadAnimIndex, UnityEngine.Random.Range(0, 2));
            var currentTransform = transform;
            var transformPosition = currentTransform.position;
            transformPosition.z = -3;
            currentTransform.position = transformPosition;
        }

        public void Move(Vector2 movement)
        {
            if (movement.x != 0)
            {
                _spriteRenderer.flipX = movement.x < 0;
            }
            _animator.SetFloat(Speed, movement.magnitude);
        }
    }
}
