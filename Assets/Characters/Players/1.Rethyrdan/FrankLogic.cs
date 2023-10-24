using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrankLogic : MonoBehaviour
{
    [Header("Dynamic Parameters")]
    public bool IsEnemy;

    public ActiveCharacterData CharacterData;



    public int CurrentHP;
    public GameObject HPBar;

    public GameObject TimerBar;
    public float timer;
    public float timerCap;

    public Animator CharacterAnimator;

    [Header("Animation Names")]
    public string IdleName;
    public string Attack1Name;
    public string Attack2Name;
    public string Attack3Name;
    public string HurtName;
    public string DeadName;

    private float IdleTime;
    private float Attack1Time;
    private float Attack2Time;
    private float Attack3Time;
    private float HurtTime;
    private float DeadTime;

    private List<GameObject> Targets;
    private int NextAction;
    private AutoAbility NextActionData;

    public bool IsDead;

    public void Init(ActiveCharacterData character, bool isEnemy)
    {

        CharacterData = character;

        IsEnemy = isEnemy;

        CurrentHP = CharacterData.MaxHP;

        timerCap = (float)(1/CharacterData.Speed);
        timer = 0;

    }

    public void UpdateLevels()
    {

    }


    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        UpdateAnimClipTimes();

        Targets = new List<GameObject>();

        IsDead = false;
        NextAction = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if(HPBar!=null)
            HPBar.GetComponent<Image>().fillAmount = ((float)CurrentHP / (float)CharacterData.MaxHP);

        if (IsDead)
            return;
        
        if(!DontDestroy.Instance.GetComponent<GameData>().FreezeTime)
            timer += Time.deltaTime;

        if(TimerBar!=null)
            TimerBar.GetComponent<Image>().fillAmount = (timer / timerCap);

        if (timer > timerCap)
        {
            timer = 0;
            DoAction();
            
        }
    }

    public void DoAction()
    {
        float delay = 0; ;
        //Select action
        SelectAction();
        if (NextAction == -2)
        {
            NextActionData = CharacterData.Template.Attack2;
            delay = Attack2Time;
        }
        else if (NextAction == -3)
        {
            NextActionData = CharacterData.Template.Attack3;
            delay = Attack3Time;
        }
        else
        {
            NextActionData = CharacterData.Template.BaseAtk;
            delay = Attack1Time;
        }

        //Find targets
        Targets = DontDestroy.Instance.GetComponent<GameData>().GetTargets(NextActionData.TargetAmount ,NextActionData.Targets);

        //Do pre-action: animation, sound
        CharacterAnimator.Play(NextActionData.AnimationSelf);

        //Wait for the animation duration.
        Invoke("DoPostAction", delay);
        
    }

    public void DoPostAction()
    {
        //Spawn Damage object on target (visuals+audio)

        //Deal Damage
        int TotalDmg = (int) (CharacterData.DMG * NextActionData.DamageMultiplier);

        foreach (GameObject enemy in Targets)
        {
            enemy.transform.GetChild(0).SendMessage("TakeDamage", TotalDmg);
        }

        //Spawn Effect object on target and set its Destroy timer (non-visual/auditory that does "something" for a duration and reverts it OnDestroy)


        Invoke("SetIdlePose", 0);
    }


    public void TakeDamage(int amount)
    {
        int actualDamage;

        if (CharacterData.Armor != 0 && amount>0)
            actualDamage = (amount + CharacterData.Armor) / CharacterData.Armor;
        else
            actualDamage = amount;

        TakeTrueDamage(actualDamage);

    }


    public void TakeTrueDamage(int amount)
    {
        CurrentHP -= amount;
        if (amount >= 0)
        {
            GameObject damageText = Instantiate(DontDestroy.Instance.GetComponent<GameData>().FadeText, this.transform.position + (Vector3.up * 200), this.transform.rotation, this.transform);
            damageText.GetComponent<animatedTextLogic>().Init("<color=red>-" + amount + "</color>", animatedTextLogic.AnimatedTextAnimation.RandomUp);
        }
        else
        {
            GameObject damageText = Instantiate(DontDestroy.Instance.GetComponent<GameData>().FadeText, this.transform.position + (Vector3.up * 200), this.transform.rotation, this.transform);
            damageText.GetComponent<animatedTextLogic>().Init("<color=green>+" + (-amount) + "</color>", animatedTextLogic.AnimatedTextAnimation.RandomUp);
        }


        if (CurrentHP > CharacterData.MaxHP)
            CurrentHP = CharacterData.MaxHP;

        if (CurrentHP <= 0)
        {
            CharacterAnimator.Play(DeadName);
            if (IsEnemy && !IsDead)
            {
                DontDestroy.Instance.GetComponent<PlayerData>().Gold += CharacterData.Gold;
            }
            if (!IsEnemy && !IsDead)
            {
                DontDestroy.Instance.GetComponent<GameData>().CharactersAlive--;
                if (DontDestroy.Instance.GetComponent<GameData>().CharactersAlive <= 0)
                {
                    DontDestroy.Instance.GetComponent<GameData>().SwitchLevelPrevious();
                }
            }
            IsDead = true;
            return;
        }

        CharacterAnimator.Play(HurtName);
        Invoke("SetIdlePose", HurtTime);
    }

    public void SetIdlePose()
    {
        if (!IsDead)
            CharacterAnimator.Play(IdleName);
    }

    public void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = CharacterAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (string.Equals(clip.name, Attack1Name))
            {
                Attack1Time = clip.length;
                clip.wrapMode = WrapMode.Once;
            }
            else if (string.Equals(clip.name, Attack2Name))
            {
                Attack2Time = clip.length;
                clip.wrapMode = WrapMode.Once;
            }
            else if (string.Equals(clip.name, Attack3Name))
            {
                Attack3Time = clip.length;
                clip.wrapMode = WrapMode.Once;
            }
            else if (string.Equals(clip.name, IdleName))
            {
                IdleTime = clip.length;
                clip.wrapMode = WrapMode.Loop;
            }
            else if (string.Equals(clip.name, HurtName))
            {
                HurtTime = clip.length;
                clip.wrapMode = WrapMode.PingPong;
            }
            else if (string.Equals(clip.name, DeadName))
            {
                DeadTime = clip.length;
                clip.wrapMode = WrapMode.ClampForever;
            }
        }
    }

    public void SelectAction()
    {
        float range = Random.Range(0, 1);
        if (range <= 0.25f)
        {
            NextAction = 2;
        }else if (range < 0.35f)
        {
            NextAction = 3;
        }
        else
        {
            NextAction = 1;
        }
    }
}