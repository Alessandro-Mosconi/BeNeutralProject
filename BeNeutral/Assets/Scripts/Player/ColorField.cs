using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorField : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        var playerMovement = GetComponentInParent<MagneticField>();

        var fieldRender = gameObject.GetComponent<Renderer>();
        if (playerMovement.playerPolarity > 0)
        {
            fieldRender.material.SetColor("_Color", Color.red);
        }
        else
        {
            fieldRender.material.SetColor("_Color", Color.blue);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
