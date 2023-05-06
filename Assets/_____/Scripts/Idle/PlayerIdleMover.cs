using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerIdleMover : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _rotator;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _cameraTarget;

    private bool _IsSharingView;
    private Transform _sharingViewObject;

    private bool _IsStopped;
    private float _resourcesMiningSpeedMod = 1f;
    private JoystickController _joystickConstroller;

    [Inject]
    private void Construct(JoystickController joystickConstroller)
    {
        Debug.Log("Construck player");
        _joystickConstroller = joystickConstroller;
    }

    private void FixedUpdate()
    {
        MoveJoystick();
    }

    private void MoveJoystick()
    {
        if (_IsStopped)
        {
            _rigidBody.velocity = Vector3.zero;
            _cameraTarget.position = this.transform.position;
            if (_animator) _animator.SetBool("Running", false);

            return;
        }
        Vector3 joystickValue = _joystickConstroller.TransformedValue;
        if (joystickValue.sqrMagnitude > 0.05f)
        {
            _rotator.rotation = Quaternion.RotateTowards(_rotator.rotation, Quaternion.LookRotation(joystickValue), Time.fixedDeltaTime * 720f);
            if(_animator) _animator.SetBool("Running", true);
            if (_animator) _animator.speed = _joystickConstroller.RawValue.magnitude * 1.2f * _speed;
        }
        else
        {
            if (_animator) _animator.SetBool("Running", false);
            joystickValue = Vector3.zero;
        }
        _rigidBody.velocity = joystickValue * 5f * _speed * _resourcesMiningSpeedMod;
        _cameraTarget.localPosition = Vector3.forward * joystickValue.magnitude * 5f;
        if (_IsSharingView)
        {
            _cameraTarget.position = this.transform.position + (_sharingViewObject.transform.position - this.transform.position) * 0.75f;
        }
        else
        {
            _cameraTarget.localPosition = Vector3.forward * joystickValue.magnitude * 5f;
        }
    }

    internal void SetSpeedCoef(float speedMod)
    {
        _resourcesMiningSpeedMod = speedMod;
    }

    internal void Stop()
    {
        _IsStopped = true;
    }


}
