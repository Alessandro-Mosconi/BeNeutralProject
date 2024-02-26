using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class BulletPlayer : BasePlayerWeapon
{
    public float speed = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.parent.position.x - gameObject.transform.position.x > 20)
        {
            gameObject.SetActive(false);
        }
        transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //print(other.gameObject.layer);

        //gameObject.SetActive(false);
        if (other.gameObject.layer == LayerMask.GetMask("Terrain"))
        {
            //collision with terrain will despawn the object
            gameObject.SetActive(false);
        }
    }

    public override void DidCollideWithEnemy()
    {
        //Collided with enemy -> hide (not destroy) since we are in a pool!
        gameObject.SetActive(false);
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}