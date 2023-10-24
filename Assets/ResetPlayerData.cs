using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPlayerDataButton()
    {
        PlayerData pd = DontDestroy.Instance.GetComponent<PlayerData>();
        pd.Init();
        SaveSystem.SavePlayerData(pd);
        DontDestroy.Instance.GetComponent<GameData>().SwitchLevel(pd.MaxLvl);
    }
}
