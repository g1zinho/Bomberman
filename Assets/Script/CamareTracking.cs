using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamareTracking : MonoBehaviour
{

    public Transform _Player;
    public Vector2 _xClamp;
    public Vector2 _yClamp;


    //set framerat
    void Awake()
    {
        Application.targetFrameRate = 60;
    }


    void LateUpdate()
    {
       
        Vector2 pos = (Vector2)_Player.position;
         //limite da camera
        float newX = Mathf.Clamp(pos.x, _xClamp.x, _xClamp.y );
        float newY = Mathf.Clamp(pos.y, _yClamp.y, _yClamp.x );

        transform.position = new Vector3(newX, newY, transform.position.z);
        
    }
}
