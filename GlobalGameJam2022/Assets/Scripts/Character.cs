using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float SpeedModifier = 20.0f;
    public float JumpModifier = 50.0f;
    public float GravityDirection = 1.0f;
    public float JumpCooldownMax = 0.1f;
    public LayerMask JumpLayerMask;
    public WorldEnum world;
    public GameObject wolfRoot;
    public AudioSource howlAudio;
    public AudioSource collectAudio;
    
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;
    private bool _canJump = false;
    private bool _warpable = false;
    private bool _exitable = false;
    private float _jumpCooldown = 0.0f;
    private Animator _animator;

    private float _lastHorizontalSpeed = 0.0f;
    private bool _facingRight = true;

    public int collectables = 0;

    private const string kWarpTag = "WarpZone";
    private const string kEndTag = "EndZone";

    public bool Warpable
    {
        get => _warpable;
    }

    public bool Exitable
    {
        get => _exitable;
    }

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        
        _animator.Play("Idle", -1, 0.0f);
    }
    
    void Update()
    {
        _rigidbody2D.gravityScale = GravityDirection;

        if (_jumpCooldown > 0.0f)
        {
            _jumpCooldown -= Time.deltaTime;
        }

        if (IsGrounded())
        {
            _animator.SetBool("Grounded", true);
            if (_jumpCooldown <= 0.0f && !_canJump)
            {
                _canJump = true;
            }
        }
        else
        {
            _animator.SetBool("Grounded", false);
        }
    }

    public void ApplyMovement(float force)
    {
        if (force < 0.0f && _facingRight)
        {
            _facingRight = false;
            wolfRoot.transform.Rotate(0.0f,180.0f,0.0f); 
        }
        else if (force > 0.0f && !_facingRight)
        {
            _facingRight = true;
            wolfRoot.transform.Rotate(0.0f,180.0f,0.0f); 
        }
        
        _animator.SetBool("Running", !Mathf.Approximately(force,0.0f));
        
        _rigidbody2D.velocity = new Vector2(force*SpeedModifier, _rigidbody2D.velocity.y);
    }

    public void TryJump()
    {
        if (!_canJump)
        {
            return;
        }

        _canJump = false;

        _jumpCooldown = JumpCooldownMax;

        var JumpForce = (GravityDirection < 0.0f)? (-JumpModifier): JumpModifier;
        
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.y, JumpForce);
        
        _animator.SetTrigger("Jump");
    }

    public void TryHowl()
    {
        _animator.SetTrigger("Howl");
    }

    public void OnHowl()
    {
        if (howlAudio)
        {
            howlAudio.Play();
        }
    }
    
    private bool IsGrounded()
    {
        float tolerance = 0.1f;
        float margin = 0.1f;
        var gravVector = new Vector2(0, -GravityDirection);
        var colliderSize = new Vector3(_collider2D.bounds.size.x - margin, _collider2D.bounds.size.y, _collider2D.bounds.size.z);
        var raycast = Physics2D.BoxCast(_collider2D.bounds.center, colliderSize, 0.0f, gravVector, tolerance, JumpLayerMask);

        Color rayColor = (raycast.collider) ? Color.green : Color.red;
        
        Debug.DrawRay(_collider2D.bounds.center + new Vector3(_collider2D.bounds.extents.x,0), gravVector*(_collider2D.bounds.extents.y + tolerance), rayColor);
        Debug.DrawRay(_collider2D.bounds.center - new Vector3(_collider2D.bounds.extents.x,0), gravVector*(_collider2D.bounds.extents.y + tolerance), rayColor);
        Debug.DrawRay(_collider2D.bounds.center + new Vector3(_collider2D.bounds.extents.x,((_collider2D.bounds.extents.y + tolerance)* -GravityDirection)), new Vector2(-Math.Abs(GravityDirection),0.0f)*(_collider2D.bounds.extents.y), rayColor);
        
        //Debug.Log(raycast.collider);
        
        return raycast.collider != null;
    }
    
    public void getCollectable()
    {
        this.collectables++;
        if (collectAudio)
        {
            collectAudio.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(kWarpTag))
        {
            _warpable = true;
        }
        else if (other.tag.Equals(kEndTag))
        {
            _exitable = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals(kWarpTag))
        {
            _warpable = false;
        }
        else if (other.tag.Equals(kEndTag))
        {
            _exitable = false;
        }
    }
}
