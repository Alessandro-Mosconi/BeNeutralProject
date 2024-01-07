using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//make translate values a variable 
public class PressurePlate : MonoBehaviour
{
    [SerializeField] private Vector3 originalPos;

    private Vector3 endPosUp;
    private Vector3 endPosDown;


    private bool moveBack = false;

    private Color originalColor;

    [SerializeField] private Transform[] targetSpikes;

    private float moveDown;
    private float moveUp;
    
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        endPosUp = transform.position - new Vector3(0, 0.1f, 0);
        endPosDown = transform.position + new Vector3(0, 0.1f, 0);

        originalColor = GetComponent<SpriteRenderer>().color;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.transform.name == "Player1")
        {
            if (transform.position.y > endPosUp.y)
            {
                transform.Translate(0, -0.01f, 0 *(1.5f *Time.deltaTime));
            }
        }else if (other.transform.name == "Player2")
        {
            if (transform.position.y < endPosDown.y)
            {
                transform.Translate(0, 0.01f, 0 *(1.5f *Time.deltaTime));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.transform.parent = transform;
            GetComponent<SpriteRenderer>().color=Color.red;

            for (int i = 0; i < targetSpikes.Length; i++)
            {
                //targetSpikes[i].Translate(0,0.6f,0);
                targetSpikes[i].Translate(0,-0.6f,0);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        moveBack = true;
        other.transform.parent = null;
        GetComponent<SpriteRenderer>().color=originalColor;
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            for (int i = 0; i < targetSpikes.Length; i++)
            {
                targetSpikes[i].Translate(0, +0.6f, 0);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (moveBack)
        {
            if (gameObject.name == "Pressure plateUp")
            {
                if (transform.position.y < originalPos.y)
                {
                    transform.Translate(0, 0.01f, 0);

                }
            }else if (gameObject.name == "Pressure plateDown")
                {
                    if (transform.position.y > originalPos.y)
                    {
                        transform.Translate(0, -0.01f, 0);

                    }
                }
            
        
            else
            {
                moveBack = false;
            }
        
        }
    }

    
}
