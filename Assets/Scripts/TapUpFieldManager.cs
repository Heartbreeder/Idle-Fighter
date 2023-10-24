using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TapUpFieldManager : MonoBehaviour
{
    public TextMeshProUGUI DamageText;
    public TextMeshProUGUI ButtonText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Init");
    }

    IEnumerator Init()
    {
        yield return new WaitForEndOfFrame();
        DamageText.text = "DMG:<color=red> " + DontDestroy.Instance.GetComponent<PlayerData>().TapDMG;
        ButtonText.text = "LvL UP: \n " + DontDestroy.Instance.GetComponent<PlayerData>().TapUpTotalCost + " G";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelupButton()
    {
        DontDestroy.Instance.GetComponent<PlayerData>().LevelupTap();
        DamageText.text = "DMG:<color=red> " + DontDestroy.Instance.GetComponent<PlayerData>().TapDMG;
        ButtonText.text = "LvL UP: \n " + DontDestroy.Instance.GetComponent<PlayerData>().TapUpTotalCost + " G";
    }

}
