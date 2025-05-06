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
    private Vector3 _initialScale;

    private Vector2 _moveDir = Vector2.zero;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private static readonly int Walking = Animator.StringToHash("Walking");
    

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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = _moveDir * _speed;
        
    }

    public void OnBomb(InputAction.CallbackContext ctx)

    //bomb spwamn
    {
        if(ctx.canceled)
        {
            Vector3 pos = transform.position;
            Vector3 bombPos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y) + 0.5f, pos.z);
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

        Snap();

        
    }

    if (ctx.canceled)
    {
        _moveDir = Vector2.zero;
        Snap();

        // Set Walking animation to false
        _animator.SetBool(Walking, false);
        Debug.Log("show error");
    }
}

    private void Snap()
    {
        //better collision to the blocks
        float x = _rigidbody.position.x;
        float y = _rigidbody.position.y;

        bool roundX = false;
        bool roundY = false;

        float xSnapTo = Mathf.Round(Mathf.Abs(x));
        if (Mathf.Abs(x) < xSnapTo + _snapError &&
            Mathf.Abs(x) > xSnapTo - _snapError &&
            _moveDir.x == 0)
            roundX = true;
            float ySnapTo = Mathf.Round(Mathf.Abs(y));
        if (Mathf.Abs(y) < xSnapTo + _snapError &&
            Mathf.Abs(y) > xSnapTo - _snapError &&
            _moveDir.y == 0)
            roundX = true;

            _rigidbody.position = new Vector2(roundX ? Mathf.Round(x) : x, roundY ? Mathf.Round(y) : y );
    }
}
