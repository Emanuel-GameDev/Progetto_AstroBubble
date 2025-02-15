using System;
using UnityEngine;

public class MovingBackground : MonoBehaviour
{
    public float speed;
    
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        _meshRenderer.material.mainTextureOffset = new Vector2(Time.time * speed, Time.time * speed);
    }
}
