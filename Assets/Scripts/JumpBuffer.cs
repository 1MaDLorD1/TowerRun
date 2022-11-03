using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBuffer : MonoBehaviour
{
    [SerializeField] private float _jumpBuffForce;

    public float JumpBuffForce => _jumpBuffForce;
}
