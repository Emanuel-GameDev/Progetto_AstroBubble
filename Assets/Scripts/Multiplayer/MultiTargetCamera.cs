using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultiTargetCamera : MonoBehaviour
{
    private List<Transform> _targets;
    
    [Header("MOVE")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime = .5f;
    
    [Header("ZOOM")]
    [SerializeField] private float minZoom = 40f;
    [SerializeField] private float maxZoom = 10f;
    [SerializeField] private float zoomLimiter = 50f;
    [SerializeField] private float zoomSpeed = 5f;
    
    private Camera _mainCamera;
    private Bounds _bounds;
    private Vector3 _velocity;

    private void Start()
    {
        _targets = new List<Transform>();   
        _mainCamera = Camera.main;
        
        List<PlayerInputHandler> inputs = PlayerConfigurationManager.Instance.PlayerInputHandlers; 

        for (int i = 0; i < inputs.Count; i++)
        {
            _targets.Add(inputs[i].transform);
        }
    }

    private void LateUpdate()
    {
        if (_targets.Count == 0) return;
        
        _bounds = new Bounds(_targets[0].position, Vector3.zero);
        for (int i = 0; i < _targets.Count; i++)
        {
            _bounds.Encapsulate(_targets[i].position);
        }

        Move();
        Zoom();
    }

    private void Zoom()
    {
        float greatestDistance = _bounds.size.x;
        float newZoom = Mathf.Lerp(minZoom, maxZoom, greatestDistance / zoomLimiter);
        _mainCamera.orthographicSize = Mathf.Lerp(_mainCamera.orthographicSize, newZoom, Time.deltaTime * zoomSpeed);
    }

    private void Move()
    {
        Vector3 center = GetCenterPoint();
        Vector3 newPosition = center + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref _velocity, smoothTime);
    }

    private Vector3 GetCenterPoint()
    {
        if (_targets.Count == 1) return _targets[0].position;
        
        return _bounds.center;
    }

}
