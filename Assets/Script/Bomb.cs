using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class Bomb : Breakable
{
    public GameObject _beam;
    public GameObject _beamEnd;

    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _BreakableLayer;

    private BoxCollider2D _boxCollider;
    private float _countdown = 0.0f; 
    public float _timer = 2.0f;

    [field: SerializeField] public float Range {get; set;} = 1;

    void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.enabled = false;
        _countdown = 0.0f;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!_boxCollider.enabled)
        {
            if((PlayerMovementController.Instance.transform.position - transform.position) .sqrMagnitude > 1.0f)
            _boxCollider.enabled = true;
            
        }

        _countdown+= Time.deltaTime;
        if(_countdown > _timer)
        {
            Explode();
        }
    }

    protected override void Explode()
    {
        Detonate();
        Destroy(gameObject);
    }

    private void Detonate()
    {
        Spread(Vector2.up);
        Spread(Vector2.down);
        Spread(Vector2.left);
        Spread(Vector2.right);
    }

    private void Spread(Vector2 dir)
    {

        Vector2 origin = transform.position;

        RaycastHit2D wallHit = Physics2D.Raycast(origin, dir, Range, _wallLayer);
        RaycastHit2D breakableHit = Physics2D.Raycast(origin, dir, Range, _BreakableLayer);
        bool hitWall = wallHit.collider;
        bool hitBreakable = wallHit.collider;

        if(hitBreakable && hitWall)
        {
            if(wallHit.distance < breakableHit.distance)
            {
                hitBreakable = false;
            }

            else
            {
                hitWall = true;
            }
        }
        
        if (hitWall)
        {
            float dist = (wallHit.point - origin).magnitude;
            if (dist <= 1.0f)
            { 
                return;
            }

            Vector2 pos = (wallHit.point - dir * 0.5f);
            InstantiateBeam(pos, dir, true);

            for(int i = 1; i < dist - 1; ++i)
            {
                InstantiateBeam(pos -i *dir, dir, false);
            }

        }
        else if(hitBreakable)
        {
            float dist = (breakableHit.point - origin).magnitude;
            
            Vector2 pos =  breakableHit.point + dir * 0.5f;
            InstantiateBeam(pos, dir, true);

        
              for(int i = 1; i < dist - 1; ++i)
            {
                InstantiateBeam(pos -i *dir, dir, false);
            }
        }
        else {
            Vector2 pos = (Vector2)transform.position + dir * Range;
            InstantiateBeam(pos, dir, true);

            int i = 1;
            while((pos - (Vector2)transform.position).magnitude > 1.0f)
            {
                pos -= i * dir;
                InstantiateBeam(pos, dir, false);
            }
        }
    
    }

    private void InstantiateBeam(Vector2 pos, Vector2 dir, bool end)
    {
        Quaternion quat = Quaternion.identity;
        if(dir == Vector2.up)
        {
            quat = Quaternion.Euler( 0, 0, -90.0f);
        }
           
        if(dir == Vector2.down)
        {
            quat = Quaternion.Euler( 0, 0, 90.0f);
        }
           
        if(dir == Vector2.right)
        {
            quat = Quaternion.Euler( 0, 0, 180.0f);
        }
           
        Instantiate(end? _beamEnd : _beam, pos, quat);
    
        
    }
}
