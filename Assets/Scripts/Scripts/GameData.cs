using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameData : MonoBehaviour
{

    #region Data
    public GameObject[] PlayerCharacters;

    public GameObject[] EnemyCharacter;

    public GameObject FadeText;
    #endregion

    #region Runtime Data

    //public GameObject[] CharacterPrefabs;

    public CharacterArray CharacterDatabase;

    public TerrainArray TerrainDatabase;

    public CharacterArray[] BasicEnemyDatabases;

    public CharacterArray[] BossEnemyDatabases;

    //public GameObject[] EnemyPrefabs;

    public int currentLevel;

    public bool FreezeTime;

    private float fadingTime = 1;
    private int fadeCounter;

    public int CharactersAlive;

    #endregion

    #region Unity Functions
    void Start()
    {

        FreezeTime = false;
        currentLevel = -10;
        SwitchLevel(GetComponent<PlayerData>().MaxLvl);


    }

    void Update()
    {
        
    }


    #endregion

    #region AI Targeting Functions

    public List<GameObject> GetTargets(int amount, TargetTypes targettingType)
    {
        List<GameObject> ret = new List<GameObject>();

        if (targettingType == TargetTypes.Enemy)
        {
            ret.Add(EnemyCharacter[0]);
        }
        else if (targettingType == TargetTypes.AllyFront)
        {
            for (int i = 0; i < amount; i++)
            {
                if (i > CharactersAlive)
                    break;

                for (int j = 0; j < PlayerCharacters.Length; j++)
                {
                    if (PlayerCharacters[j].transform.GetChild(0) != null)
                    {
                        if (!PlayerCharacters[j].transform.GetChild(0).GetComponent<FrankLogic>().IsDead)
                        {
                            ret.Add(PlayerCharacters[j]);
                            break;
                        }
                    }
                }


            }
        }
        else if (targettingType == TargetTypes.AllyBack)
        {
            for (int i = PlayerCharacters.Length; i > PlayerCharacters.Length - amount; i--)
            {
                ret.Add(PlayerCharacters[i - 1]);
            }
        }
        else if (targettingType == TargetTypes.AllyRandom)
        {
            List<int> randoms = GenerateRandomList(amount, 0, PlayerCharacters.Length - 1);
            foreach (int index in randoms)
            {
                ret.Add(PlayerCharacters[index]);
            }
        }
        else if (targettingType == TargetTypes.AllyAll)
        {
            foreach (GameObject obj in PlayerCharacters)
            {
                ret.Add(obj);
            }
        }
        else if (targettingType == TargetTypes.Everyone)
        {
            foreach (GameObject obj in PlayerCharacters)
            {
                ret.Add(obj);
            }
            foreach (GameObject obj in EnemyCharacter)
            {
                ret.Add(obj);
            }
        }
        return ret;
    }

    public List<int> GenerateRandomList(int amount, int min, int max)
    {
        List<int> randomList = new List<int>();
        for (int i = 0; i < amount && i< max-min; i++)
        {
            int numToAdd = Random.Range(min, max);
            while (!randomList.Contains(numToAdd))
            {
                numToAdd = Random.Range(min, max);
            }
            randomList.Add(numToAdd);

        }
        return randomList;
    }
    #endregion

    #region Switch Level Functions

    public void SwitchLevel(int nextLevel)
    {


        FreezeTime = true;
        /*
         * Fiver Index: Value between 1 and 5
         * Fiver Group Value starting from 0
         * */
        int fiverIndex = (nextLevel%5);
        if (fiverIndex == 0) fiverIndex = 5;
        int fiverGroup = ((nextLevel-1)/5);

        int currentfiverIndex = (currentLevel % 5);
        if (currentfiverIndex == 0) currentfiverIndex = 5;
        int currentfiverGroup = ((currentLevel-1) / 5);

        if (nextLevel > GetComponent<PlayerData>().MaxLvl)
        {
            GetComponent<SceneManager>().SwitchLevelToggles[currentfiverIndex-1].GetComponent<UnityEngine.UI.Toggle>().SetIsOnWithoutNotify(true);
            return;
        }

        //Fade in and out screen over time
        fadeCounter = 0;
        Debug.Log("Switching level");
        if (fadingTime <= 0)
            fadingTime = 1;
        GetComponent<SceneManager>().LoadingImage.GetComponent<CanvasGroup>().blocksRaycasts = true;
        Invoke("FadeIn", 0);
        
        //Clear enemies and allies
        foreach(GameObject character in PlayerCharacters)
        {
            if (character.transform.childCount > 0)
            {
                Destroy(character.transform.GetChild(0).gameObject);
            }
        }
        foreach (GameObject character in EnemyCharacter)
        {
            if (character.transform.childCount > 0)
            {
                Destroy(character.transform.GetChild(0).gameObject);
            }
        }

        //Set Background and BGM
        Debug.Log((int)currentLevel + " " + (int)nextLevel);
        int terrainIndex = (fiverGroup) % TerrainDatabase.Array.Length;
        Debug.Log("TerrainIndex" + terrainIndex);
        //Debug.Log((int) currentfiverGroup + " " + (int)fiverGroup);
        if (currentfiverGroup != fiverGroup)
        {
            

            GetComponent<SceneManager>().BackgroundMusic.GetComponent<AudioSource>().clip = TerrainDatabase.Array[terrainIndex].BGM;
            GetComponent<SceneManager>().BackgroundMusic.GetComponent<AudioSource>().Play();

            GetComponent<SceneManager>().BackgroundFront.GetComponent<UnityEngine.UI.Image>().sprite = TerrainDatabase.Array[terrainIndex].BackgroundFront;
            GetComponent<SceneManager>().BackgroundBack.GetComponent<UnityEngine.UI.Image>().sprite = TerrainDatabase.Array[terrainIndex].BackgroundBack;
        }

        //Spawn allies
        CharactersAlive = 0;
        PlayerData pd = gameObject.GetComponent<PlayerData>();
        GameObject instance;
        for (int i = 0; i < 5; i++)
        {
            if(pd.CharacterDataArray.Length > i)
            {
                if(pd.CharacterDataArray[i].Level > 0)
                {
                    instance = Instantiate(pd.CharacterDataArray[i].Template.CharacterPrefab, PlayerCharacters[i].transform);
                    instance.GetComponent<FrankLogic>().Init(pd.CharacterDataArray[i], false);
                    CharactersAlive++;
                }
            }
        }


        //Spawn enemy
        CharacterTemplate enemyTemplate;
        if (fiverIndex == 5)
        {
            enemyTemplate = BossEnemyDatabases[0].Array[0];
            ActiveCharacterData enemy = new ActiveCharacterData();
            enemy.Init(enemyTemplate, nextLevel, true);

            instance = Instantiate(enemyTemplate.CharacterPrefab, EnemyCharacter[0].transform);
            instance.GetComponent<FrankLogic>().Init(enemy, true);
            instance.GetComponent<FrankLogic>().HPBar = this.GetComponent<SceneManager>().EnemyHealthBar;
            GetComponent<SceneManager>().ResetTimer();
        }
        else
        {
            enemyTemplate = BasicEnemyDatabases[0].Array[0];
            ActiveCharacterData enemy = new ActiveCharacterData();
            enemy.Init(enemyTemplate, nextLevel, true);

            instance = Instantiate(enemyTemplate.CharacterPrefab, EnemyCharacter[0].transform);
            instance.GetComponent<FrankLogic>().Init(enemy, true);
            instance.GetComponent<FrankLogic>().HPBar = this.GetComponent<SceneManager>().EnemyHealthBar;

        }




        //Set GUIs
        GetComponent<SceneManager>().EnemyNameplate.GetComponent<TextMeshProUGUI>().text = enemyTemplate.CharacterName;
        GetComponent<SceneManager>().LevelIndicatorText.GetComponent<TextMeshProUGUI>().text = "Level: " + nextLevel;
        GetComponent<SceneManager>().LoadingText.GetComponent<TextMeshProUGUI>().text = "Level: " + nextLevel;

        if (fiverIndex == 5)
        {
            GetComponent<SceneManager>().EnemyTimerBar.SetActive(true);
            GetComponent<SceneManager>().EnemyTimerText.SetActive(true);
            GetComponent<SceneManager>().EnemyTimerBarBackground.SetActive(true);
        }
        else
        {
            GetComponent<SceneManager>().EnemyTimerBar.SetActive(false);
            GetComponent<SceneManager>().EnemyTimerText.SetActive(false);
            GetComponent<SceneManager>().EnemyTimerBarBackground.SetActive(false);
        }

        //Set the Level toggles
        GetComponent<SceneManager>().SwitchLevelToggles[0].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ((int)((fiverGroup) * 5 + 1)).ToString();
        GetComponent<SceneManager>().SwitchLevelToggles[1].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ((int)((fiverGroup) * 5 + 2)).ToString();
        GetComponent<SceneManager>().SwitchLevelToggles[2].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ((int)((fiverGroup) * 5 + 3)).ToString();
        GetComponent<SceneManager>().SwitchLevelToggles[3].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ((int)((fiverGroup) * 5 + 4)).ToString();
        GetComponent<SceneManager>().SwitchLevelToggles[4].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ((int)((fiverGroup) * 5 + 5)).ToString();

        GetComponent<SceneManager>().SwitchLevelToggles[fiverIndex-1].GetComponent<UnityEngine.UI.Toggle>().SetIsOnWithoutNotify(true);
        //GetComponent<SceneManager>().SwitchLevelToggles[nextLevel%5].GetComponent<UnityEngine.UI.Toggle>().isOn = true;

        currentLevel = nextLevel;
    }

    public void SwitchLevelPrevious()
    {
        if (currentLevel > 1)
            SwitchLevel(currentLevel-1);
        else
            SwitchLevel(1);
    }
    public void SwitchLevelLast()
    {
            SwitchLevel(GetComponent<PlayerData>().MaxLvl);
    }

    public void SwitchLevelNext()
    {
        if (GetComponent<PlayerData>().MaxLvl == currentLevel)
            GetComponent<PlayerData>().MaxLvl ++;
        SwitchLevel(currentLevel+1);

    }

    public void SwitchLevelRelative(int offset)
    {
        int fiverGroup = ((currentLevel - 1) / 5);

        SwitchLevel(fiverGroup * 5 + offset);
    }

    public void FadeIn()
    {
        GetComponent<SceneManager>().LoadingImage.GetComponent<CanvasGroup>().alpha += 0.01f;
        fadeCounter++;
        if (fadeCounter < 100)
            Invoke("FadeIn", fadingTime / 100);
        else
            Invoke("FadeOut", fadingTime/2);
    }

    public void FadeOut()
    {
        GetComponent<SceneManager>().LoadingImage.GetComponent<CanvasGroup>().alpha -= 0.01f;
        fadeCounter--;
        if (fadeCounter >= 0)
            Invoke("FadeOut", fadingTime / 100);
        else
        {
            FreezeTime = false;
            GetComponent<SceneManager>().LoadingImage.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    #endregion

}

public enum TargetTypes
{
    Enemy,
    AllyFront,
    AllyBack,
    AllyRandom,
    AllyAll,
    Everyone
}
