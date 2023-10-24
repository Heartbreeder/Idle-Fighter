using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoPanelHandler : MonoBehaviour
{
    [Header("Persistent content")]
    public TextMeshProUGUI CharacterNameField;
    public TextMeshProUGUI CharacterLevelField;

    [Header("Toggles")]
    public GameObject TabToggleObject;
    public Toggle BasicInfoToggle;
    public Image Ability1Image;
    public Image Ability2Image;
    public Image Ability3Image;
    public Image ActiveAbilityImage;

    [Header("Toggle Highlights")]
    public Image Ability2Highlight;
    public Image Ability3Highlight;
    public Image ActiveAbilityHighlight;

    [Header("Info Tab")]
    public GameObject InfoTabObject;
    public TextMeshProUGUI InfoNameField;
    public TextMeshProUGUI InfoLevelField;
    public TextMeshProUGUI InfoDescriptionField;

    [Header("Inventory")]
    public GameObject InventoryTab;


    private ActiveCharacterData SelectedCharacter;

    public void Start()
    {
        if(SelectedCharacter == null)
        {
            Init(DontDestroy.Instance.GetComponent<PlayerData>().CharacterDataArray[0]);
        }
    }

    public void Init(ActiveCharacterData selectedCharacter)
    {
        SelectedCharacter = selectedCharacter;
        CharacterNameField.text = SelectedCharacter.Template.CharacterName;
        CharacterLevelField.text = "Level: " + SelectedCharacter.Level;
        //tab images
        Ability1Image.sprite = SelectedCharacter.Template.BaseAtk.AbilityIcon;
        Ability2Image.sprite = SelectedCharacter.Template.Attack2.AbilityIcon;
        Ability3Image.sprite = SelectedCharacter.Template.Attack3.AbilityIcon;
        ActiveAbilityImage.sprite = SelectedCharacter.Template.ActiveSkill.AbilityIcon;

        if (SelectedCharacter.Level > SelectedCharacter.Template.Attack2.ActivationLvl) Ability2Highlight.color = Color.green; else Ability2Highlight.color = Color.grey;
        if (SelectedCharacter.Level > SelectedCharacter.Template.Attack3.ActivationLvl) Ability3Highlight.color = Color.green; else Ability3Highlight.color = Color.grey;
        if (SelectedCharacter.Level > SelectedCharacter.Template.ActiveSkill.ActivationLvl) ActiveAbilityHighlight.color = Color.green; else ActiveAbilityHighlight.color = Color.grey;
        if (BasicInfoToggle.isOn) ChangeTab(BasicInfoToggle);
        else BasicInfoToggle.isOn = true;
    }


    public void CloseButtonLogic()
    {
        this.gameObject.SetActive(false);
    }

    public void ChangeTab(Toggle caller)
    {
        Debug.Log("Change Tab Called");
        if (!caller.isOn) return;

        int index = caller.GetComponent<ToggleIndexData>().Index;

        if(index == 0)
        {
            //Character Info
            InfoTabObject.SetActive(true);
            InventoryTab.SetActive(false);

            InfoNameField.text = "Role: " + SelectedCharacter.Template.CharacterRole.ToString();
            InfoLevelField.text = "";
            InfoDescriptionField.text = SelectedCharacter.Template.CharacterDescription;

        }
        else if (index == 1)
        {
            //Inventory
        }
        else if (index == 2)
        {
            //Ability 1
            InfoNameField.text = SelectedCharacter.Template.BaseAtk.AbilityName;
            InfoLevelField.text = "Basic Attack";
            InfoDescriptionField.text = SelectedCharacter.Template.BaseAtk.Description;
        }
        else if (index == 3)
        {
            //Ability 2
            InfoNameField.text = SelectedCharacter.Template.Attack2.AbilityName;
            string lvltext = "Activation Level: " + SelectedCharacter.Template.Attack2.ActivationLvl;
            if (SelectedCharacter.Level < SelectedCharacter.Template.Attack2.ActivationLvl) InfoLevelField.text = "<color=red>"+lvltext+"</color>"; else InfoLevelField.text = lvltext;
            InfoDescriptionField.text = "15% chance to use\n" + SelectedCharacter.Template.Attack2.Description;
        }
        else if (index == 4)
        {
            //Ability 3
            InfoNameField.text = SelectedCharacter.Template.Attack3.AbilityName;
            string lvltext = "Activation Level: " + SelectedCharacter.Template.Attack3.ActivationLvl;
            if (SelectedCharacter.Level < SelectedCharacter.Template.Attack3.ActivationLvl) InfoLevelField.text = "<color=red>" + lvltext + "</color>"; else InfoLevelField.text = lvltext;
            InfoDescriptionField.text = "5% chance to use\n" + SelectedCharacter.Template.Attack3.Description;
        }
        else if (index == 5)
        {
            //Active Ability
            InfoNameField.text = SelectedCharacter.Template.ActiveSkill.AbilityName;
            string lvltext = "Activation Level: " + SelectedCharacter.Template.ActiveSkill.ActivationLvl;
            if (SelectedCharacter.Level < SelectedCharacter.Template.ActiveSkill.ActivationLvl) InfoLevelField.text = "<color=red>" + lvltext + "</color>"; else InfoLevelField.text = lvltext;
            InfoDescriptionField.text = "Cooldown :" + SelectedCharacter.Template.ActiveSkill.Cooldown + "\n" + SelectedCharacter.Template.ActiveSkill.Description;
        }

    }
 
}
