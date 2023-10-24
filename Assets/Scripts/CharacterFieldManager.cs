using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterFieldManager : MonoBehaviour
{
    public Image CharacterSprite;
    public GameObject IsActiveBorder, UnusedCharPanel, PortraitBackground;
    public TextMeshProUGUI nameField, LevelField, HealthField, DamageField, ArmorField, SpeedField, LevelUpCostField;
    public GameObject InfoPanelObject;

    [Header("Initialised after spawning")]
    public int CharacterIndex;

    private CharacterArray CA;
    private CharacterTemplate character;

    public void Init()
    {
        //CharacterIndex = characterindex;
        character = CA.Array[CharacterIndex];

        if(character.CharacterPortrait!=null)
            CharacterSprite.sprite = character.CharacterPortrait;
        nameField.text = character.CharacterName;
        if(character.CharacterRole == Roles.Fighter)
            PortraitBackground.GetComponent<Image>().color = Color.blue;
        else if (character.CharacterRole == Roles.Warrior)
            PortraitBackground.GetComponent<Image>().color = Color.red;
        else if (character.CharacterRole == Roles.Healer)
            PortraitBackground.GetComponent<Image>().color = Color.green;
        else if (character.CharacterRole == Roles.Archer)
            PortraitBackground.GetComponent<Image>().color = Color.yellow;
        else if (character.CharacterRole == Roles.Mage)
            PortraitBackground.GetComponent<Image>().color = Color.magenta;

        UpdateStats();
    }

    void Start()
    {
        CA = DontDestroy.Instance.gameObject.GetComponent<GameData>().CharacterDatabase;
        Init();
    }



    void Update()
    {
        
    }

    private void UpdateStats()
    {


        PlayerData pd = DontDestroy.Instance.GetComponent<PlayerData>();
        if (pd.CharacterDataArray[CharacterIndex].Level <= 0)
        {
            IsActiveBorder.GetComponent<Image>().color = Color.red;
            UnusedCharPanel.SetActive(true);
            LevelUpCostField.text = "Unlock: \n" + pd.CharacterDataArray[CharacterIndex].Gold.ToString() + "G";
        }
        else
        {
            IsActiveBorder.GetComponent<Image>().color = Color.green;
            UnusedCharPanel.SetActive(false);
            LevelUpCostField.text = "LVL up: \n" + pd.CharacterDataArray[CharacterIndex].Gold.ToString() + "G";
        }

        LevelField.text = "Level: " + pd.CharacterDataArray[CharacterIndex].Level.ToString();
        HealthField.text = "HP: <color=green>" + pd.CharacterDataArray[CharacterIndex].MaxHP.ToString() + "</color>";
        if (pd.CharacterDataArray[CharacterIndex].DMG >0)
            DamageField.text = "DMG:<color=red>" + pd.CharacterDataArray[CharacterIndex].DMG.ToString() + "</color>";
        else
            DamageField.text = "Heal:<color=green>" + (-pd.CharacterDataArray[CharacterIndex].DMG).ToString() + "</color>";
        ArmorField.text = "Armor: <color=blue>" + pd.CharacterDataArray[CharacterIndex].Armor.ToString() + "</color>";
        SpeedField.text = "Speed:<color=yellow>" + pd.CharacterDataArray[CharacterIndex].Speed.ToString() + "</color>";


    }

    public void LevelUpButtonLogic()
    {
        PlayerData pd = DontDestroy.Instance.GetComponent<PlayerData>();
        pd.CharacterLvlup(CharacterIndex);
        UpdateStats();
    }

    public void InfoButtonLogic()
    {
        InfoPanelObject.SetActive(true);
        PlayerData pd = DontDestroy.Instance.GetComponent<PlayerData>();
        InfoPanelObject.GetComponent<InfoPanelHandler>().Init(pd.CharacterDataArray[CharacterIndex]);
    }
}
