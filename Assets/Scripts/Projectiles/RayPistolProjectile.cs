using System;
using UnityEngine;

public class RayPistolProjectile : MonoBehaviour
{
    public float BaseDmg { get; set; }
    
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Fire(Vector2 direction, float speed)
    {
        if (_rb == null) return;
        
        _rb.linearVelocity = direction * speed;
    }
}
