using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainSet", menuName = "Heart/TerrainSet")]
public class TerrainArray : ScriptableObject
{
    [SerializeField]
    public TerrainTemplate[] Array;
}
