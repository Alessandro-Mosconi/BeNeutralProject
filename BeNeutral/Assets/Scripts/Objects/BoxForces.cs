using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class BoxForces : MonoBehaviour
{
    [SerializeField] public int positivty = 1;
    [SerializeField] public int gravityDirection = 1;
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
        

        var fieldRender = gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>();
        if (positivty > 0)
        {
            fieldRender.material.SetColor("_Color", Color.red);
        }
        else
        {
            fieldRender.material.SetColor("_Color", Color.blue);
        }

        GetComponents();
    }

    // Update is called once per frame
    void Update()
    {
        HandleGravity();
            
        if (magneticField1 != null && magneticField1.activeSelf)
        {
            ApplyForce(magneticField1, positivty);
        }
        
        if (magneticField2 != null && magneticField2.activeSelf)
        {
            ApplyForce(magneticField2, -positivty);
        }
        
    }

    private void HandleGravity()
    {
        if (gravityDirection > 0)
        {
            boxRb.gravityScale = Math.Abs(boxRb.gravityScale);
            Quaternion rotation = transform.rotation;
            rotation.x = 0;
            transform.rotation = rotation;
        }
        else
        {
            boxRb.gravityScale = Math.Abs(boxRb.gravityScale) * -1;
            Quaternion rotation = transform.rotation;
            rotation.x = -180;
            transform.rotation = rotation;
        }
    }
private void ApplyForce(GameObject magneticField, int playerPositivity)
    {
        //deltaX positive if box right, field left

        float distanceX = GetDistanceXFrom(magneticField);

        float directionX = GetDirectionXFrom(magneticField);
        float forceX;
        if (distanceX > 1f)
        {
            forceX = GetForceFromDistance(distanceX, 1.3f);
        }
        else
        {
            forceX = boxRb.velocity.x;
        }
        
        float distanceY = GetDistanceYFrom(magneticField);

        float directionY = GetDirectionYFrom(magneticField);
        
        float forceY;
        
        if (distanceY > 1.2f)
        {
            forceY = GetForceFromDistance(distanceY, 1.3f);
        }
        else
        {
            forceY = boxRb.velocity.y;
        }

        if (distanceX * distanceX +  distanceY * distanceY < maxForceDistance * maxForceDistance)
        {
            if (playerPositivity * magneticAttraction > 0)
            {
                attractBox(directionX, forceX, directionY, forceY);
            }
            else
            {
                repelBox(directionX, forceX, directionY, forceY);
            }
        }
    }

    private void repelBox(float directionX, float forceX, float directionY, float forceY)
    {
        float oldXPosition = boxRb.position.x;
        //repelli
        boxRb.velocity = new Vector2(directionX * forceX, directionY * forceY);

        print("repello");
        if (forceX > 0 && Math.Round(oldXPosition, 2) == Math.Round(boxRb.position.x, 2))
        {
            print("autorepello");
        }
    }

    private void attractBox(float directionX, float forceX, float directionY, float forceY)
    {
        //attrai fino a essere vicini 
        boxRb.velocity = new Vector2(-directionX * forceX, -directionY * forceY);
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
    private float GetForceFromDistance(float distance, float increaser)
    {
        //aumenta avvicinandosi
        return Math.Abs(maxForceDistance * increaser - distance * increaser);
    }

    private float GetDistanceYFrom(GameObject otherObject)
    {
        return Math.Abs(transform.position.y - otherObject.transform.position.y);
    }
    private float GetDirectionYFrom(GameObject otherObject)
    {
        //1 a destra, -1 a sinistra
        if (otherObject.transform.position.y > transform.position.y)
        {
            return 1f;
        }
        else
        {
            return -1f;
        }
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