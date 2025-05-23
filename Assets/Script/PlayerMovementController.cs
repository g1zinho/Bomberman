using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    public static PlayerMovementController Instance;


    public float _speed = 5.0f;
    public float _snapError = 0.5f;

    public GameObject _BombPrefab;
    public PlayerInput _playerInput;
    private Vector3 _initialScale;

    private Vector2 _moveDir = Vector2.zero;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int Death = Animator.StringToHash("Death");
    

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _initialScale = transform.localScale;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = _moveDir * _speed;
        
    }

public void OnBomb(InputAction.CallbackContext ctx)
{
    if (ctx.canceled)
    {
        Vector3 pos = transform.position;

        // Snap to nearest tile center
        float snappedX = Mathf.Round(pos.x - 0.5f) + 0.6f;
        float snappedY = Mathf.Round(pos.y - 0.5f) + 0.5f;

        Vector3 bombPos = new Vector3(snappedX, snappedY, 0f);
        Instantiate(_BombPrefab, bombPos, Quaternion.identity);
    }
}

    public void OnMove(InputAction.CallbackContext ctx)
{
    if (ctx.performed)
    {
        // Set Walking animation to true
        _animator.SetBool(Walking, true);
        Vector2 input = ctx.ReadValue<Vector2>();

        // Normalize to -1, 0, or 1
        _moveDir.x = input.x != 0 ? Mathf.Sign(input.x) : 0;
        _moveDir.y = input.y != 0 ? Mathf.Sign(input.y) : 0;

        // Flip character when moving horizontally
        if (_moveDir.x != 0)
        {
            transform.localScale = new Vector3(_initialScale.x * Mathf.Sign(_moveDir.x), _initialScale.y, _initialScale.z);
        }
    

        
    }

    if (ctx.canceled)
    {
        _moveDir = Vector2.zero;

        // Set Walking animation to false
        _animator.SetBool(Walking, false);
    }
}  


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Explosion"))
        {
            Die();

        }
   }

   private void Die()
   {
       _animator.SetTrigger(Death);
       _playerInput.currentActionMap.Disable();
   }





}
