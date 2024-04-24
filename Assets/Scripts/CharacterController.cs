using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isJumping;
    public float RunSpeed;
    
    [SerializeField] private Transform feet;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpForce;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float _horizontalMove;
    private bool _wasMovingLeft;
    private float _jumpCooldown;
    private bool _inputLock;
    private float _inputLockTimer;
    private float _momentumTimer;
    private float _momentunMaximumTime = 1f;
    private float _momentumMultiplier = 1.5f;
    private float _streakMultiplier = 1f;
    private bool _playerIsDead;

    private void Start()
    {
        PlayerManager.Instance.PlayerDied.AddListener(OnPlayerDied);    
    }

    private void OnPlayerDied()
    {
        Debug.Log("Player died");
        _playerIsDead = true;
    }

    private void Update()
    {
        if (_playerIsDead) return;
        
        isGrounded = Physics2D.OverlapCircle(feet.position, groundDistance, groundLayer);
        if (isGrounded)
        {
            MomentumCalculation();
        }
        
        if (_inputLock)
        {
            _inputLockTimer += Time.deltaTime;
            if (_inputLockTimer > .3f)
            {
                _inputLock = false;
            }
            
            UpdateAnimator();
            return;
        }
        
        _horizontalMove = Input.GetAxisRaw("Horizontal") * RunSpeed;
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }
    
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        if(isJumping && isGrounded)
        {
            rb.velocity = Vector2.up * (jumpForce * _momentumMultiplier * _streakMultiplier);
            isJumping = false;

            if (_momentumMultiplier > 1f)
            {
                if (_streakMultiplier <= 1.5f)
                {
                    // Debug.Log("streakki nousi");
                    _streakMultiplier += 0.05f;
                }                
            }
            else
            {
                ResetStreakMultiplier();
            }
            
            ResetMomentumMultiplier();
        }

        MoveCharacter();
    }
    
    private void UpdateAnimator()
    {   
        animator.SetFloat("Speed", Mathf.Abs(_horizontalMove));
        animator.SetBool("Grounded", isGrounded);
        
        if (rb.velocity.x < 0f)
        {
            _wasMovingLeft = true;
        }
        else if (rb.velocity.x > 0f)
        {
            _wasMovingLeft = false;
        }

        if (rb.velocity.y > 0f)
        {
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsFreeFalling", false);
        }
        else if(rb.velocity.y < 0f)
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFreeFalling", true);
        }

        spriteRenderer.flipX = _wasMovingLeft;
    }

    private void MoveCharacter()
    {
        rb.velocity = new Vector2(_horizontalMove * RunSpeed, rb.velocity.y);
    }
    public void FlipHorizontalMove()
    {
        _horizontalMove *= -1;
        _inputLock = true;
        _inputLockTimer = 0f;
        
        rb.AddForce(new Vector2(_horizontalMove * _momentumMultiplier, _momentumMultiplier), ForceMode2D.Impulse);
        ResetMomentumMultiplier();
    }
    
    private void MomentumCalculation()
    {
        // Debug.Log("momentumi: " + _momentumMultiplier);

        if (_momentumMultiplier >= 1f )
        {
            _momentumMultiplier -= Time.deltaTime;
        }
    }

    private void ResetMomentumMultiplier()
    {
        _momentumMultiplier = 1.5f;
    }
    
    private void ResetStreakMultiplier()
    {
        // Debug.Log("streakki resetoitui");
        _streakMultiplier = 1f;
    }
}
