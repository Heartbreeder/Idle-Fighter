using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSet", menuName = "Heart/CharacterSet")]
public class CharacterArray : ScriptableObject
{
    [SerializeField]
    public CharacterTemplate[] Array;
}
