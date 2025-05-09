using UnityEngine;

public class Crate : Breakable
{
    private Animator _animator;
    private Rigidbody2D _rigibody;
    private BoxCollider2D _collider;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    protected override void Explode()
    {
        Destroy(_rigibody);
        Destroy(_collider);
        _animator.SetTrigger("Explode");
        
    }

    public void End()
    {
        Destroy(gameObject);

    }

  
}
