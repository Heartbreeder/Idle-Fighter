using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralContentManager : MonoBehaviour
{

    public Transform ContentBase;

    public enum ContentSelection
    {
        Tab1,Tab2,Tab3
    }

    public ContentSelection ContentToggle;

    #region Characters Panel
    public GameObject TapDamageFieldPrefab;
    public GameObject CharacterFieldPrefab;

    #endregion

    #region Upgrades Panel

    #endregion

    #region Store Panel

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        ContentToggle = ContentSelection.Tab1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OpenCharacterTab()
    {
        //instantiate tap dmg tab
        //for each character
        //instatntiate character tab
    }

}
