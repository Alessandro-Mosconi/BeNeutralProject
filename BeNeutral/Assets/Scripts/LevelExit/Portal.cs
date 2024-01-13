using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;



public class Portal : MonoBehaviour
{
    private Animator telportAnimator;

    private PlayerMovement playerMovementScript;
    
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    private Transform pos1;
    private Transform pos2;
    
    private PlayerManager pSc1;
    private PlayerManager pSc2;

    [SerializeField] private Transform secretLevelPosition1;

    [SerializeField] private Transform secretLevelPosition2;


    private Transform SecretCameraTarget;
    
    private FollowLastPlayer OriginalCameraTarget;

    
    private CinemachineVirtualCamera _vcam;

    private EMPwall empWall;


    private RespawnPlayers respawn;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        telportAnimator = GetComponent<Animator>();
        playerMovementScript = FindObjectOfType<PlayerMovement>();
        
        pSc1 = player1.GetComponent<PlayerManager>();
        pos1 = pSc1.transform;
        pSc2 = player2.GetComponent<PlayerManager>();
        pos2 = pSc2.transform;

        SecretCameraTarget = GameObject.Find("Secret Level Camera Target").GetComponent<Transform>();

        _vcam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();


        OriginalCameraTarget = GameObject.Find("Camera Target").GetComponent<FollowLastPlayer>();


        empWall = GameObject.Find("EMP").GetComponent<EMPwall>();

        respawn = GameObject.Find("Respawn").GetComponent<RespawnPlayers>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivatePortal()
    {
        telportAnimator.SetTrigger("OnPlayerEnter");
        
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetButton("MagneticFieldPlayer" + playerMovementScript.playerNumber))
            {
                ActivatePortal();
                StartCoroutine(EnterPortal(other.transform));
                
            }
            
        }
    }

    private IEnumerator EnterPortal(Transform player)
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        
        respawn.gameObject.SetActive(false);
        
        yield return EnterPortalEffect(player);
        OriginalCameraTarget.enabled = false;
        yield return new WaitForSeconds(1f);
        pos1.transform.position = secretLevelPosition1.position;
        pos2.transform.position = secretLevelPosition2.position;
        _vcam.PreviousStateIsValid = false;
        _vcam.Follow = SecretCameraTarget;

        empWall.isActive = true;
        
        
        pSc1.fallDetector.SetActive(false);
        pSc2.fallDetector.SetActive(false);
        player.GetComponent<PlayerMovement>().enabled = true;



    }

    private IEnumerator EnterPortalEffect(Transform player)
    {
        float elapsed = 0f;
        float duration = 1f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;

            yield return null;
        }

    }
}
