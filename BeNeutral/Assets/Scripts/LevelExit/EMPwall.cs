using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPwall : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float speed;
    public Vector3 empPos;
    [SerializeField] private float damageValue;

    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

    private PlayerManager p1;
    private PlayerManager p2;


    public bool isActive = false;


    private SecretLevelRespawn checkpoints;

    void Start()
    {
        empPos = transform.position;
        p1 = player1.GetComponent<PlayerManager>();
        p2 = player2.GetComponent<PlayerManager>();
        checkpoints = GameObject.Find("SecretLevelRespawn").GetComponent<SecretLevelRespawn>();
        

    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            if (p1.Fell || p2.Fell)
            {
                transform.position = empPos;
            }else if ((p1.Fell || p2.Fell) && (p1.IsSecretCheckpoint2 || p2.IsSecretCheckpoint2))
            {
                transform.position = checkpoints.checkpoint2;
            }
        }

    }

}
