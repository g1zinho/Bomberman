using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBeam : MonoBehaviour
{

    [SerializeField] private float _expansiontime = 0.2f;
    [SerializeField] private float _delay = 0.1f;
    [SerializeField] private float _dispersionTime = 0.2f;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        var collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;

        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(scale.y, 0, scale.z);

        float t = 0;
        
        while ( t < _expansiontime)
        {
            t += Time.deltaTime * 2;
            if ( t >= _expansiontime)
            {
                t += _expansiontime;
            }

            transform.localScale = new Vector3(scale.x, Mathf.Lerp(0, scale.y, t / _expansiontime), scale.z);
            yield return new WaitForEndOfFrame();
        }
        
        collider.enabled = true;
        yield return new WaitForSeconds(_delay);

        t = 0;
         while ( t < _dispersionTime)
        {
            t += Time.deltaTime * 2;
            if ( t >= _dispersionTime)
            {
                t += _dispersionTime;
            }

            transform.localScale = new Vector3(scale.x, Mathf.Lerp( scale.y, 0 , t / _dispersionTime), scale.z);
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);

    }

 
}
