using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class ExitPortal : MonoBehaviour
{
     private Animator telportAnimator;

    private PlayerMovement playerMovementScript;
    
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    private Transform pos1;
    private Transform pos2;
    
    private PlayerManager pSc1;
    private PlayerManager pSc2;

    [SerializeField] private Transform exitLevelPosition1;

    [SerializeField] private Transform exitLevelPosition2;


    private Transform OriginalCameraTarget;
    
    private SecretLevelCamera secretCameraScript;
    private FollowLastPlayer originalCameraScript;
    

    
    [SerializeField] private CinemachineVirtualCamera _vcam;

    private EMPwall empWall;


    private GameObject barrier;

    
    private RespawnPlayers respawn;


    private SecretLevelRespawn secretRespawn;

    
    
    
    // Start is called before the first frame update
    void Start()
    {
        telportAnimator = GetComponent<Animator>();
        playerMovementScript = FindObjectOfType<PlayerMovement>();
        
        pSc1 = player1.GetComponent<PlayerManager>();
        pos1 = pSc1.transform;
        pSc2 = player2.GetComponent<PlayerManager>();
        pos2 = pSc2.transform;

        OriginalCameraTarget = GameObject.Find("Camera Target").GetComponent<Transform>();

        _vcam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();


        secretCameraScript = GameObject.Find("Secret Level Camera Target").GetComponent<SecretLevelCamera>();

        originalCameraScript = GameObject.Find("Camera Target").GetComponent<FollowLastPlayer>();


        empWall = GameObject.Find("EMP").GetComponent<EMPwall>();

        barrier = GameObject.Find("Barrier");
        
        
        respawn = GameObject.Find("Respawn").GetComponent<RespawnPlayers>();


        secretRespawn = GameObject.Find("SecretLevelRespawn").GetComponent<SecretLevelRespawn>();


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
        
        secretRespawn.gameObject.SetActive(false);
        respawn.gameObject.SetActive(true);
        
        yield return EnterPortalEffect(player);
        secretCameraScript.enabled = false;
        yield return null;
        originalCameraScript.enabled = true;
        pos1.transform.position = exitLevelPosition1.position;
        pos2.transform.position = exitLevelPosition2.position;
        _vcam.PreviousStateIsValid = false;
        _vcam.Follow = OriginalCameraTarget;

        empWall.isActive = false;
        barrier.SetActive(false);
        pSc1.fallDetector.SetActive(true);
        pSc2.fallDetector.SetActive(true);
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
