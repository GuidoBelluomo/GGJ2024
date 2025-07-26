using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Physics
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class ArcOverlapper : MonoBehaviour
    {
        public float Angle
        {
            get => _angle;
            set
            {
                _angle = value;
                UpdateCollider();
            }
        }
    
        public float Radius
        {
            get => _radius;
            set
            {
                _radius = value;
                UpdateCollider();
            }
        }
    
        public int Samples
        {
            get => _samples;
            set
            {
                _samples = value;
                UpdateCollider();
            }
        }
    
        [SerializeField][Range(0, 360)] private float _angle = 120;
        [SerializeField][Range(0, 10)] private float _radius = 2;
        [SerializeField][Range(2, 64)] private int _samples = 12;
        private PolygonCollider2D _polygonCollider2D;
        private HashSet<GameObject> overlappingObjects = new();
    
        void Awake()
        {
            _polygonCollider2D = GetComponent<PolygonCollider2D>();
            _polygonCollider2D.isTrigger = true;
        }

        private void Start()
        {
            UpdateCollider();
        }

        private void Update()
        {
            UpdateCollider();
        }

        void UpdateCollider()
        {
            Vector2[] points = new Vector2[_samples + 2];
            float angle = -_angle / 2;
            for (int i = 0; i < _samples + 1; i++)
            {
                points[i + 1] = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * Radius, Mathf.Sin(angle * Mathf.Deg2Rad) * Radius);
                angle += _angle / Samples;
            }

            _polygonCollider2D.points = points;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            overlappingObjects.Add(other.gameObject);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            overlappingObjects.Remove(other.gameObject);
        }

        public GameObject[] GetOverlappingObjects()
        {
            return overlappingObjects.ToArray();
        }
    }
}
