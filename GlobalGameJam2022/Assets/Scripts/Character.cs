using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour, ISaveable
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
    private int count;

    private float _lastHorizontalSpeed = 0.0f;
    private bool _facingRight = true;

    public int collectables = 0;

    private const string kWarpTag = "WarpZone";
    private const string kEndTag = "EndZone";
    private SaveManager saveManager;

    public void SaveData(PlayerData data)
    {
        
        GameObject wolf1 = GameObject.FindGameObjectWithTag("Wolf1");
        GameObject wolf2 = GameObject.FindGameObjectWithTag("Wolf2");

        data.wolf1X = wolf1.transform.position.x;
        data.wolf1Y = wolf1.transform.position.y;
        data.wolf1Z = wolf1.transform.position.z;
        data.wolf2X = wolf2.transform.position.x;
        data.wolf2Y = wolf2.transform.position.y;
        data.wolf2Z = wolf2.transform.position.z;
        data.currSceneName = SceneManager.GetActiveScene().name;
    }

    //implements LoadData from ISaveable interface
    public void LoadData(PlayerData data)
    {
        Debug.Log("load called from character controller");
        GameObject wolf1 = GameObject.FindGameObjectWithTag("Wolf1");
        GameObject wolf2 = GameObject.FindGameObjectWithTag("Wolf2");
        Debug.Log(wolf1.transform.position.x + "x pos before load");
        Vector3 wolf1Pos = new Vector3(data.wolf1X, data.wolf1Y, data.wolf1Z);
        wolf1.transform.position = wolf1Pos;
        Debug.Log(wolf1.transform.position.x + " x after load");

        Vector3 wolf2Pos = new Vector3(data.wolf2X, data.wolf2Y, data.wolf2Z);
        wolf2.transform.position = wolf2Pos;
    }

    private void Awake()
    {
        saveManager = FindObjectOfType<SaveManager>();
    }
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
        if (saveManager.playerData != null)
        {
            LoadData(saveManager.playerData);
        }
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
