using System;
using System.Collections.Generic;
using UnityEngine;


//make translate values a variable 
public class PressurePlate : MonoBehaviour
{
    [SerializeField] private Vector3 originalPos;

    private Vector3 endPos;

    private bool moveBack = false;

    private Color originalColor;

    [SerializeField] private Transform[] targetSpikes;
    [SerializeField] private bool moveWhenPressed = true;

    private float moveDown;
    private float moveUp;

    private List<GameObject> colliders = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;

        originalColor = GetComponent<SpriteRenderer>().color;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")
            || other.gameObject.layer == LayerMask.NameToLayer("External-objects"))
        {
            colliders.Add(other.gameObject);
            other.transform.parent = transform;

            if (colliders.Count <= 1)
            {
                GetComponent<SpriteRenderer>().color=Color.red;
                if (moveWhenPressed)
                {
                    transform.Translate(0,-0.1f,0);
                }
                
                for (int i = 0; i < targetSpikes.Length; i++)
                {
                    targetSpikes[i].Translate(0,-0.6f,0);
                }
            }
            
            
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")
            || other.gameObject.layer == LayerMask.NameToLayer("External-objects"))
        {
            other.transform.parent = null;
            colliders.Remove(other.gameObject);
            
            if (colliders.Count == 0)
            {
                GetComponent<SpriteRenderer>().color=originalColor;
                if (moveWhenPressed)
                {
                    transform.Translate(0, +0.1f, 0);
                }
                
                for (int i = 0; i < targetSpikes.Length; i++)
                {
                    targetSpikes[i].Translate(0, +0.6f, 0);
                }
            }
        }

    }

    
}
