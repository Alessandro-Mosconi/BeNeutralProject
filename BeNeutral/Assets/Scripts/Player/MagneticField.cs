using System.Collections;
using System.Linq;
using UI;
using UnityEngine;

public class MagneticField : MonoBehaviour
{
    [SerializeField] private GameObject magneticFieldPrefab;
    private GameObject magneticFieldInstance;

    public float dimensioneIniziale = 0.01f;
    public float dimensioneFinale = 0.5f;
    public float velocitaTransizione = 2.0f;
    public bool isActive = false;
    
    private PlayerMovement playerMovementScript;

    private void Awake()
    {
        playerMovementScript = GetComponent<PlayerMovement>();
    }
    private void Start()
    {
        // Istanzia il campo magnetico come un oggetto disattivato
        magneticFieldInstance = GetChildGameObject(gameObject, "MagneticField");
        magneticFieldInstance.transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
        magneticFieldInstance.SetActive(isActive);

        // Imposta la dimensione iniziale
        magneticFieldInstance.transform.localScale = new Vector3(dimensioneIniziale, dimensioneIniziale, 1.0f);
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

    private void AttivaMagneticField()
    {
        isActive = true;
        magneticFieldInstance.SetActive(true);
        
        // - start magnetic field sound
        AudioManager.instance.PlayForceFieldPlayer();
        
        StartCoroutine(TransizioneBolla(true));
    }

    private void DisattivaMagneticField()
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
    private GameObject GetChildGameObject(GameObject fromGameObject, string withName)
    {
        foreach (Transform childTransform in transform)
        {
            if (childTransform.gameObject.name == withName)
            {
                return childTransform.gameObject;
            }
        }

        return null;
    }
    
}
