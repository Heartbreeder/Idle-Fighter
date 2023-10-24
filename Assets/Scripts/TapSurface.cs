using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapSurface : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        //Debug.Log("CLick Successful");
        if(!DontDestroy.Instance.GetComponent<GameData>().FreezeTime)
            DontDestroy.Instance.GetComponent<PlayerData>().Tap();
    }
}
