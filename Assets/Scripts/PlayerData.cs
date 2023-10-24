using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerData : MonoBehaviour
{
    #region Set From Inspector
    [Header("--- Set From Inspector ---")]
    public GameObject GoldTextObject;
    public GameObject GemsTextObject;
    [Header("--- ---")]
    #endregion

    #region Static Data
    public double TapUpCostBase = 2;
    public double TapUpCostLinear = 1;
    public double TapUpCostProgressive = 0.05;
    #endregion

    #region Saved Data
    [SerializeField]
    private double _gold;
    public double Gold
    {
        get { return _gold; }
        set
        {
            string index;
            if ((value - _gold) > 0) index = "+";
            else index = "";
            string printVal = (value - _gold).ToString();

            GameObject damageText = Instantiate(gameObject.GetComponent<GameData>().FadeText, GoldTextObject.transform.position + (Vector3.up * 50), GoldTextObject.transform.rotation, GoldTextObject.transform);
            damageText.GetComponent<animatedTextLogic>().Init("<color=yellow>" + index + printVal + "</color>", animatedTextLogic.AnimatedTextAnimation.StraightUp);
            _gold = value;
        }
    }
    public double Gems;
    public int MaxLvl;
    public ActiveCharacterData[] CharacterDataArray;
    public int TapDMG;

    //REMOVE
    public int FrankLevel = 1;
    #endregion

    #region Temporary Data
    public int TapUpTotalCost;

    public GameData gamedata;
    #endregion

    #region Deserialiser
    public void Init(PlayerDataContainer data)
    {
        Gold = data.Gold;
        Gems = data.Gems;
        MaxLvl = data.MaxLvl;

        gamedata = gameObject.GetComponent<GameData>();
        CharacterDataArray = new ActiveCharacterData[gamedata.CharacterDatabase.Array.Length];
        for (int i=0; i< gamedata.CharacterDatabase.Array.Length; i++)
        {
            if(data.CharacterLevel.Length > i)
            {
                CharacterDataArray[i] = new ActiveCharacterData();
                CharacterDataArray[i].Init(gamedata.CharacterDatabase.Array[i], data.CharacterLevel[i], false);
            }
            else
            {
                CharacterDataArray[i] = new ActiveCharacterData();
                CharacterDataArray[i].Init(gamedata.CharacterDatabase.Array[i], 0, false);
            }
        }

        TapDMG = data.TapDMG;
    }

    #endregion

    #region Constructor
    public void Init()
    {
        Gold = 0;
        Gems = 0;
        MaxLvl = 1;
        gamedata = gameObject.GetComponent<GameData>();
        CharacterDataArray = new ActiveCharacterData[gamedata.CharacterDatabase.Array.Length];
        CharacterDataArray[0] = new ActiveCharacterData();
        CharacterDataArray[0].Init(gamedata.CharacterDatabase.Array[0], 1, false);
        for (int i=1; i< CharacterDataArray.Length; i++)
        {
            CharacterDataArray[i] = new ActiveCharacterData();
            CharacterDataArray[i].Init(gamedata.CharacterDatabase.Array[i], 0, false);
        }
        TapDMG = 1;
    }
    #endregion

    #region Unity functions
    void Start()
    {

        TapUpTotalCost = (int)TapUpCostBase + (int)(TapUpCostLinear * TapDMG) + (int)(TapUpCostBase * System.Math.Pow(TapUpCostProgressive, TapDMG));


        //Debug.Log("Tapuptotalcost" + TapUpTotalCost);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Awake()
    {
        Init(SaveSystem.LoadPlayerData());
    }

    private void OnApplicationQuit()
    {
        SaveSystem.SavePlayerData(this);
    }

    #endregion

    #region Tap Functions
    public void Tap()
    {
        GetComponent<GameData>().EnemyCharacter[0].transform.GetChild(0).SendMessage("TakeTrueDamage", TapDMG);
    }

    public void LevelupTap()
    {
        if (Gold < TapUpTotalCost)
            return;

        Gold -= TapUpTotalCost;
        TapDMG++;

        TapUpTotalCost = (int)TapUpCostBase + (int)(TapUpCostLinear * TapDMG) + (int)(TapUpCostBase * System.Math.Pow(TapUpCostProgressive, TapDMG));
    }

    #endregion

    #region Character Functions
    public void CharacterLvlup(int index)
    {
        if(Gold >= CharacterDataArray[index].Gold)
        {
            Gold -= CharacterDataArray[index].Gold;
            CharacterDataArray[index].IncreaseLevel();
        }
    }

    #endregion
}

[System.Serializable]
public class PlayerDataContainer
{
    #region Data
    public double Gold;
    public double Gems;
    public int MaxLvl;
    //Character Data
    public int[] CharacterLevel;
    public int TapDMG;
    #endregion

    #region Constructors
    public PlayerDataContainer() { }

    public PlayerDataContainer(PlayerData data)
    {
        Init(data);
    }
    #endregion

    #region Serialiser
    public void Init(PlayerData data)
    {
        Gold = data.Gold;
        Gems = data.Gems;
        MaxLvl = data.MaxLvl;

        CharacterLevel = new int[data.CharacterDataArray.Length];
        for (int i=0; i < CharacterLevel.Length; i++)
        {
            CharacterLevel[i] = data.CharacterDataArray[i].Level;
        }

        TapDMG = data.TapDMG;
    }
    #endregion

}

[System.Serializable]
public class ActiveCharacterData
{
    //Inputted
    public int Level;
    public bool IsEnemy;
    public CharacterTemplate Template;

    //Calculated
    public int MaxHP;
    public int DMG;
    public int Armor;
    public double Speed;
    public double Gold;

    public void Init(CharacterTemplate template, int level, bool isEnemy)
    {
        Template = template;
        Level = level;
        IsEnemy = isEnemy;

        CalculateStats();
    }

    public void IncreaseLevel()
    {
        Level++;
        CalculateStats();

        //if (Level == 1)
          //  DontDestroy.Instance.GetComponent<GameData>().SwitchLevel(DontDestroy.Instance.GetComponent<GameData>().currentLevel);


    }

    public void CalculateStats()
    {
        CharacterBaseStats bst = Template.BaseStats;

        int calcLevel = Level;
        if (calcLevel < 1) calcLevel = 1;
        MaxHP = (int)bst.BaseHP + (int)(bst.IncreaseHPLinear * calcLevel) + (int)(System.Math.Pow(bst.IncreaseHPProgressive, calcLevel));
        DMG = (int)bst.BaseDMG + (int)(bst.IncreaseDMGLinear * calcLevel) + (int)(System.Math.Pow(bst.IncreaseDMGProgressive, calcLevel));
        Armor = (int)bst.BaseArmor + (int)(bst.IncreaseArmorLinear * calcLevel) + (int)(System.Math.Pow(bst.IncreaseArmorProgressive, calcLevel));

        Gold = bst.BaseCost + (bst.IncreaseCostLinear * Level) + (System.Math.Pow(bst.IncreaseCostProgressive, Level));
        Gold = System.Math.Round(Gold);
        if(bst.IncreaseSpeedLevelStep != 0)
        {
            int speedSteps = (int)(Level / bst.IncreaseSpeedLevelStep);
            Speed = bst.BaseSpeed + (bst.IncreaseSpeedLinear * speedSteps);
            if (Speed > 2.5f * bst.BaseSpeed || Speed > 2.5f)
                Speed = 2.5f * bst.BaseSpeed;
        }
        else
            Speed = bst.BaseSpeed;

    }

}

