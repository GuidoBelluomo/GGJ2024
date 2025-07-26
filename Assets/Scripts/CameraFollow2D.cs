using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow2D : MonoBehaviourSingleton<CameraFollow2D>
{
    public enum UpdateMethod
    {
        Update,
        FixedUpdate,
        LateUpdate
    }
    
    public GameObject Target => target;
    [field:SerializeField] public bool StartOnTarget { get; set; }
    [field:SerializeField] public UpdateMethod PreferredUpdateMethod { get; set; }
    [field:SerializeField] public Vector2 Offset { get; set; }
    [field:SerializeField] public float Distance { get; set; }
    [field:SerializeField] public bool FollowX { get; set; }
    [field:SerializeField] public bool FollowY { get; set; }
    [field:SerializeField] public bool EnableLag { get; set; }
    [field:SerializeField] public float MaxLagDistance { get; set; }
    [field:SerializeField] public float CatchupSpeed { get; set; }
    [field:SerializeField] public float MinCatchupDistance { get; set; }
    [SerializeField] private GameObject target;
    
    private Vector2 _oldOffset;

    public void SetTarget(GameObject newTarget, bool snap = false)
    {
        target = newTarget;
        if (snap)
        {
            SnapToTarget();
        }
    }
    
    public void SnapToTarget()
    {
        transform.position = CalcIdealPosition() + (Vector3)Offset;
    }
    
    private Camera _camera;

    public Camera Camera => _camera;

    public override void Awake()
    {
        base.Awake();
        _camera = GetComponent<Camera>();
    }
    
    void Start()
    {
        _oldOffset = Offset;
        if (Target)
        {
            SetTarget(Target);
            if (StartOnTarget)
            {
                SnapToTarget();
            }
        }
    }

    void UpdateCameraPosition()
    {
        if (!EnableLag)
        {
            SnapToTarget();
        }
        else
        {
            var idealPosition = CalcIdealPosition();
            var position = transform.position - (Vector3)Offset;
            var followPosition = new Vector3(position.x, position.y, Distance);
            var difference = idealPosition - followPosition;
            if (difference.magnitude > MaxLagDistance)
            {
                var clampMagnitude = Vector3.ClampMagnitude(difference, MaxLagDistance);
                followPosition = idealPosition - clampMagnitude;
            }
            else
            {
                if (difference.magnitude < MinCatchupDistance)
                {
                    difference = difference.normalized * MinCatchupDistance;
                }
                followPosition += difference * (Time.deltaTime * CatchupSpeed);
            }

            transform.position = followPosition + (Vector3)Offset;
        }
    }

    private Vector3 CalcIdealPosition()
    {
        if (!Target) return transform.position;
        
        return new Vector3(
            (FollowX ? Target.transform.position.x : transform.position.x),
            (FollowY ? Target.transform.position.y : transform.position.y),
            Distance
        );
    }

    // Update is called once per frame
    void Update()
    {
        CorrectOffset();
        
        if (PreferredUpdateMethod == UpdateMethod.Update)
        {
            UpdateCameraPosition();
        }
    }

    private void CorrectOffset()
    {
        var offsetDiff = _oldOffset - Offset;
        if (offsetDiff.magnitude > 0)
        {
            transform.position -= (Vector3)offsetDiff;
            _oldOffset = Offset;
        }
    }

    void FixedUpdate()
    {
        if (PreferredUpdateMethod == UpdateMethod.FixedUpdate)
        {
            UpdateCameraPosition();
        }
    }

    void LateUpdate()
    {
        if (PreferredUpdateMethod == UpdateMethod.LateUpdate)
        {
            UpdateCameraPosition();
        }
    }
}
