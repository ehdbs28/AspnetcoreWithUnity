using System;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
using UnityEngine.UIElements;

public class MainPlayer : Player
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _lookSpeed;
    
    private Rigidbody _rigidbody;
    private Transform _cameraTrm;
    private Transform _modelTrm;

    private float _rotationX;

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody>();
        _modelTrm = transform.Find("Model");
        _cameraTrm = _modelTrm.Find("SightCamera");
    }

    private void Update()
    {
        MovementUpdate();
        RotateUpdate();
    }

    private void MovementUpdate()
    {
        var xInput = Input.GetAxisRaw("Horizontal");
        var yInput = Input.GetAxisRaw("Vertical");
        var moveDirection = _modelTrm.right * xInput + _modelTrm.forward * yInput;
        moveDirection.Normalize();

        if (moveDirection.sqrMagnitude <= 0.01f)
        {
            return;
        }

        var moveVec = moveDirection.normalized * (_moveSpeed * Time.deltaTime);
        SetAndBroadcastPosition(transform.position + moveVec);
    }

    private void RotateUpdate()
    {
        var mouseX = Input.GetAxis("Mouse X") * _lookSpeed;
        var mouseY = Input.GetAxis("Mouse Y") * _lookSpeed;

        _rotationX -= mouseY;
        _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);
        _cameraTrm.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);

        _modelTrm.Rotate(Vector3.up * mouseX);
    }
}