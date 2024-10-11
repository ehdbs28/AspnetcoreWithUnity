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

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _cameraTrm = transform.Find("SightCamera");
        _modelTrm = transform.Find("Model");
    }

    private void Update()
    {
        RotateUpdate();
    }

    private async void MovementUpdate()
    {
        
    }

    private async void RotateUpdate()
    {
        
    }
}