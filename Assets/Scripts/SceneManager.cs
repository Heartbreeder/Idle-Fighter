using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    [Header("Top GUI")]
    public GameObject LevelIndicatorText;
    public GameObject EnemyNameplate;
    public GameObject EnemyHealthBar;
    public GameObject EnemyTimerBar;
    public GameObject EnemyTimerText;
    public GameObject EnemyTimerBarBackground;

    [Header("Switch Level GUI")]
    public GameObject[] SwitchLevelToggles;

    [Header("Ability Activation GUI")]
    public GameObject[] AbilityButtons;

    [Header("Gold and Gems")]
    public GameObject GoldText;
    public GameObject GemsText;

    [Header("Loading Screen")]
    public GameObject LoadingImage;
    public GameObject LoadingText;

    [Header("Background")]
    public GameObject BackgroundBack;
    public GameObject BackgroundFront;
    public GameObject BackgroundMusic;

    private float EnemyHeathFillingPrevious, EnemyTimerFillingPrevious;

    public float MaxTime = 30;


    private float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GoldText.GetComponent<TextMeshProUGUI>().text = GetComponent<PlayerData>().Gold.ToString();

        if (EnemyHealthBar.GetComponent<Image>().fillAmount <= 0 && EnemyHeathFillingPrevious > 0)
        {
            GetComponent<GameData>().SwitchLevelNext();
        }
        EnemyHeathFillingPrevious = EnemyHealthBar.GetComponent<Image>().fillAmount;

        //timerLogic
        if (EnemyTimerBar.activeSelf)
        {
            if (!DontDestroy.Instance.GetComponent<GameData>().FreezeTime && currentTime > 0)
                currentTime -= Time.deltaTime;

            if (currentTime <0)
            {
                GetComponent<GameData>().SwitchLevelPrevious();
            }
            else
            {
                EnemyTimerBar.GetComponent<Image>().fillAmount = currentTime / MaxTime;

                float minutes = Mathf.Floor(currentTime / 60.0f);
                float seconds = currentTime - minutes * 60.0f;
                EnemyTimerText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:00}:{1:00}", minutes, seconds);


             }


            EnemyTimerFillingPrevious = EnemyTimerBar.GetComponent<Image>().fillAmount;
        }

        


    }

    public void ResetTimer()
    {
        currentTime = MaxTime;
    }
}
