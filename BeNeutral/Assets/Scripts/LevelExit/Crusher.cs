using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour
{
    [SerializeField] private float upSpeed;

    [SerializeField] private float downSpeed;

    [SerializeField] private Transform upward;

    [SerializeField] private Transform downward;

    private bool crushed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= upward.position.y)
        {
            crushed = true;
        }

        if (transform.position.y <= downward.position.y)
        {
            crushed = false;
        }

        if (crushed)
        {
            transform.position = Vector2.MoveTowards(transform.position, downward.position
                , downSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, upward.position
                , upSpeed * Time.deltaTime);
        }
    }
}
