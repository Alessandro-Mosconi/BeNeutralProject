using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoxForces : MonoBehaviour
{
    [SerializeField] public float maxForceDistance = 3;
    [SerializeField] public int magneticAttraction = 1;
    private Rigidbody2D boxRb;
    private GameObject player1;
    private GameObject player2;
    private GameObject magneticField1;
    private GameObject magneticField2;

    // Start is called before the first frame update
    void Start()
    {
        GetComponents();
    }

    // Update is called once per frame
    void Update()
    {
        if (magneticField1 != null && magneticField1.activeSelf)
        {
            ApplyForceX(magneticField1, 1);
        }
        
        if (magneticField2 != null && magneticField2.activeSelf)
        {
            ApplyForceX(magneticField2, -1);
        }
        
    }

    private void ApplyForceX(GameObject magneticField, int playerPositivity)
    {
        //deltaX positive if box right, field left

        float distanceX = GetDistanceXFrom(magneticField);

        float direction = GetDirectionXFrom(magneticField);

        float forceX = GetForceXFromDistance(distanceX, 1.3f);

        if (distanceX < maxForceDistance)
        {
            if (playerPositivity * magneticAttraction > 0)
            {
                if (distanceX > 0.9)
                {
                    attractBox(direction, forceX);
                }
            }
            else
            {
                repelBox(direction, forceX);
            }
        }
    }

    private void repelBox(float direction, float forceX)
    {
        //repelli
        boxRb.velocity = new Vector2(direction * forceX, boxRb.velocity.y);
    }

    private void attractBox(float direction, float forceX)
    {
        //attrai fino a essere vicini 
        boxRb.velocity = new Vector2(-direction * forceX, boxRb.velocity.y);
    }

    private float GetDistanceXFrom(GameObject otherObject)
    {
        return Math.Abs(transform.position.x - otherObject.transform.position.x);
    }
    private float GetDirectionXFrom(GameObject otherObject)
    {
        //1 a destra, -1 a sinistra
        if (otherObject.transform.position.x > transform.position.x)
        {
            return 1f;
        }
        else
        {
            return -1f;
        }
    }
    private float GetForceXFromDistance(float distance, float increaser)
    {
        //aumenta avvicinandosi
        return Math.Abs(maxForceDistance * increaser - distance * increaser);
    }

    private void GetComponents()
    {
        boxRb = gameObject.GetComponent<Rigidbody2D>();
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        magneticField1 = GetChildGameObject(player1, "MagneticField");
        magneticField2 = GetChildGameObject(player2, "MagneticField");
    }


    public GameObject GetChildGameObject(GameObject fromGameObject, string withName)
    {
        var allKids = fromGameObject.GetComponentsInChildren<Transform>();
        var kid = allKids.FirstOrDefault(k => k.gameObject.name == withName);
        if (kid == null) return null;
        return kid.gameObject;
    }
}