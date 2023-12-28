using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Hitpoints")]
public class HitPointsSO : ScriptableObject
{
    [SerializeField] private float hitPointValue;
    [SerializeField] private float staminaValue;

    public float HitPointValue
    {
        get { return hitPointValue; }
        set { hitPointValue = value; }
    }
    public float StaminaValue
    {
        get { return staminaValue; }
        set { staminaValue = value; }
    }
}
