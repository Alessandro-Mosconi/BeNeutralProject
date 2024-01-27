using System.Collections;
using System.Linq;
using UI;
using UnityEngine;

public class MagneticField : MonoBehaviour
{
    [SerializeField] private float dimensioneIniziale = 0.01f;
    [SerializeField] private float dimensioneFinale = 10.0f;
    [SerializeField] private float velocitaTransizione = 2.0f;
    [SerializeField] public int playerPolarity = 1; //1 = red, -1 = blue
    
    private GameObject magneticFieldInstance;
    private PlayerMovement playerMovementScript;
    public bool isActive = true;

    public float GetCurrentIntensity()
    {
        return magneticFieldInstance.transform.localScale.x * 0.5f;
    }

    private void Awake()
    {
        playerMovementScript = GetComponent<PlayerMovement>();
    }
    private void Start()
    {
        // Istanzia il campo magnetico come un oggetto disattivato
        magneticFieldInstance = GetChildGameObject("MagneticField");
        magneticFieldInstance.transform.localScale = new Vector3(dimensioneIniziale, dimensioneIniziale, 1.0f);
        magneticFieldInstance.SetActive(isActive);
        
    }

    private void Update()
    {
        if (Input.GetButtonDown("MagneticFieldPlayer" + playerMovementScript.playerNumber))
        {
            AttivaMagneticField();
        }
        else if (Input.GetButtonUp("MagneticFieldPlayer" + playerMovementScript.playerNumber))
        {
            DisattivaMagneticField();
        }
    }
    
    //made the method public so I can access it 

    public void AttivaMagneticField()
    
    {
        isActive = true;
        magneticFieldInstance.SetActive(true);
        
        // - start magnetic field sound
        AudioManager.instance.PlayForceFieldPlayer();
        
        StartCoroutine(TransizioneBolla(true));
    }
    //made the method public so I can access it
    public void DisattivaMagneticField()
    {
        StartCoroutine(TransizioneBolla(false));
    }

    private IEnumerator TransizioneBolla(bool ingrandisci)
    {
        Vector3 dimensioneIniziale =  !ingrandisci? magneticFieldInstance.transform.localScale : new Vector3(this.dimensioneIniziale, this.dimensioneIniziale, 1.0f);
        Vector3 dimensioneFinale = ingrandisci ? new Vector3(this.dimensioneFinale, this.dimensioneFinale, 1.0f) : new Vector3(this.dimensioneIniziale, this.dimensioneIniziale, 1.0f);;

        float tempoTrascorso = 0f;

        while (tempoTrascorso < 1.0f)
        {
            tempoTrascorso += Time.deltaTime * velocitaTransizione ;
            magneticFieldInstance.transform.localScale = Vector3.Lerp(dimensioneIniziale, dimensioneFinale, tempoTrascorso);
            yield return null;
        }

        if (!ingrandisci)
        {
            isActive = false;
            magneticFieldInstance.SetActive(false);
        }
    }
    private GameObject GetChildGameObject(string withName)
    {
        foreach (Transform childTransform in transform)
        {;
            if (childTransform.gameObject.name == withName)
            {
                return childTransform.gameObject;
            }
        }

        return null;
    }
    
    
}
