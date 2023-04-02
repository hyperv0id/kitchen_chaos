using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;

/// …Ë÷√Ω«…´ ‰»Î
public struct KCCInput{
    public Vector3 moveInput;
    public Quaternion CameraRotation;
}

public class PlayerKCC : MonoBehaviour, ICharacterController
{
    public KinematicCharacterMotor Motor;

    [Header("Stable Movement")]
    public float MaxStableMoveSpeed = 10f;
    public float StableMovementSharpness = 15;
    public float OrientationSharpness = 10;

    [Header("Air Movement")]
    public float MaxAirMoveSpeed = 10f;
    public float AirAccelerationSpeed = 5f;
    public float Drag = 0.1f;

    [Header("Misc")]
    public bool RotationObstruction;
    public Vector3 Gravity = new Vector3(0, -30f, 0);
    public Transform MeshRoot;

    // Start is called before the first frame update
    void Start()
    {
        Motor.CharacterController = this;
    }
    Vector3 _moveInputVector;
    Vector3 _lookInputVector;

    // Update is called once per frame
    void Update()
    {

    }

    public void SetInput(ref KCCInput inputs)
    {
        // Clamp input
        Vector3 moveInputVector = Vector3.ClampMagnitude(inputs.moveInput, 1f);

        // Calculate camera direction and rotation on the character plane
        Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, Motor.CharacterUp).normalized;
        if (cameraPlanarDirection.sqrMagnitude == 0f)
        {
            cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.up, Motor.CharacterUp).normalized;
        }
        Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Motor.CharacterUp);

        // Move and look inputs
        _moveInputVector = cameraPlanarRotation * moveInputVector;
        _lookInputVector = cameraPlanarDirection;
    }
    /// <summary>
    /// (Called by KinematicCharacterMotor during its update cycle)
    /// This is where you tell your character what its rotation should be right now. 
    /// This is the ONLY place where you should set the character's rotation
    /// </summary>
    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        if (_lookInputVector != Vector3.zero && OrientationSharpness > 0f)
        {
            // Smoothly interpolate from current to target look direction
            Vector3 smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;

            // Set the current rotation (which will be used by the KinematicCharacterMotor)
            currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);
        }
    }

    /// <summary>
    /// (Called by KinematicCharacterMotor during its update cycle)
    /// This is where you tell your character what its velocity should be right now. 
    /// This is the ONLY place where you can set the character's velocity
    /// </summary>
    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        Vector3 targetMovementVelocity = Vector3.zero;
        if (Motor.GroundingStatus.IsStableOnGround)
        {
            // Reorient source velocity on current ground slope (this is because we don't want our smoothing to cause any velocity losses in slope changes)
            currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, Motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;

            // Calculate target velocity
            Vector3 inputRight = Vector3.Cross(_moveInputVector, Motor.CharacterUp);
            Vector3 reorientedInput = Vector3.Cross(Motor.GroundingStatus.GroundNormal, inputRight).normalized * _moveInputVector.magnitude;
            targetMovementVelocity = reorientedInput * MaxStableMoveSpeed;

            // Smooth movement Velocity
            currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1 - Mathf.Exp(-StableMovementSharpness * deltaTime));
        }
        else
        {
            // Add move input
            if (_moveInputVector.sqrMagnitude > 0f)
            {
                targetMovementVelocity = _moveInputVector * MaxAirMoveSpeed;

                // Prevent climbing on un-stable slopes with air movement
                if (Motor.GroundingStatus.FoundAnyGround)
                {
                    Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                    targetMovementVelocity = Vector3.ProjectOnPlane(targetMovementVelocity, perpenticularObstructionNormal);
                }

                Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, Gravity);
                currentVelocity += velocityDiff * AirAccelerationSpeed * deltaTime;
            }

            // Gravity
            currentVelocity += Gravity * deltaTime;

            // Drag
            currentVelocity *= (1f / (1f + (Drag * deltaTime)));
        }
    }
    /// <summary>
    /// This is called before the motor does anything
    /// </summary>
    public void BeforeCharacterUpdate(float deltaTime) { }
    /// <summary>
    /// This is called after the motor has finished its ground probing, but before PhysicsMover/Velocity/etc.... handling
    /// </summary>
    public void PostGroundingUpdate(float deltaTime) { }
    /// <summary>
    /// This is called after the motor has finished everything in its update
    /// </summary>
    public void AfterCharacterUpdate(float deltaTime) { }
    /// <summary>
    /// This is called after when the motor wants to know if the collider can be collided with (or if we just go through it)
    /// </summary>
    public bool IsColliderValidForCollisions(Collider coll)
    {
        return true;
    }
    /// <summary>
    /// This is called when the motor's ground probing detects a ground hit
    /// </summary>
    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) { }
    /// <summary>
    /// This is called when the motor's movement logic detects a hit
    /// </summary>
    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) { }
    /// <summary>
    /// This is called after every move hit, to give you an opportunity to modify the HitStabilityReport to your liking
    /// </summary>
    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {

    }
    /// <summary>
    /// This is called when the character detects discrete collisions (collisions that don't result from the motor's capsuleCasts when moving)
    /// </summary>
    public void OnDiscreteCollisionDetected(Collider hitCollider)
    {

    }
}
