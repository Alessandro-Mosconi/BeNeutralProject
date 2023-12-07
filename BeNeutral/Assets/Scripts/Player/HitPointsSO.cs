using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Hitpoints")]
public class HitPointsSO : ScriptableObject
{
    [SerializeField] private float hitPointValue;

    public float HitPointValue
    {
        get { return hitPointValue; }
        set { hitPointValue = value; }
    }
}
