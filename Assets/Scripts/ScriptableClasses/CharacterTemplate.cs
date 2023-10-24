using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CharacterTemplate
{
    public string CharacterName;
    public Sprite CharacterPortrait;
    public GameObject CharacterPrefab;
    [TextArea(1,15)]
    public string CharacterDescription;

    public Roles CharacterRole;

    [Header("Basic Stats")]
    public CharacterBaseStats BaseStats;

    [Header("Automatic Abilities")]
    public AutoAbility BaseAtk;
    public AutoAbility Attack2;
    public AutoAbility Attack3;

    [Header("Active Ability")]
    public ActiveAbility ActiveSkill;

}

[System.Serializable]
public class CharacterBaseStats
{
    [Header("Cost(Ally) or Gold Reward(enemy)")]
    public double BaseCost;
    public double IncreaseCostLinear, IncreaseCostProgressive;

    [Header("HP")]
    public double BaseHP;
    public double IncreaseHPLinear, IncreaseHPProgressive;

    [Header("DMG")]
    public double BaseDMG;
    public double IncreaseDMGLinear, IncreaseDMGProgressive;

    [Header("Armor")]
    public double BaseArmor;
    public double IncreaseArmorLinear, IncreaseArmorProgressive;

    [Header("Speed")]
    public double BaseSpeed;
    public double IncreaseSpeedLinear, IncreaseSpeedLevelStep;
}

[System.Serializable]
public class AutoAbility
{
    public string AbilityName;
    public string Description;
    public Sprite AbilityIcon;
    public int ActivationLvl;
    [Tooltip("The animation called on the character when this ability is used")]
    public string AnimationSelf;
    [Tooltip("The SFX played when using the ability")]
    public AudioClip AbilitySFX;
    public TargetTypes Targets;
    public int TargetAmount;
    [Tooltip("This is multipleid by the base DMG value- Multiply by -1 to heal.")]
    public float DamageMultiplier;
    [Tooltip("The Visual effects of the Hit shown on the target")]
    public GameObject TargetVFXPrefab;
    [Tooltip("Additional Effect instantiated as child of the target. Manipulates its parent On Start and On Destroy OR loops a behavior over time.")]
    public GameObject TargetEffectPrefab;
    [Tooltip("Target Effect is destroyed after this much time.")]
    public float TargetEffectDuration;
    
}

[System.Serializable]
public class ActiveAbility
{
    public string AbilityName;
    public string Description;
    public Sprite AbilityIcon;
    public double Cooldown;
    public int ActivationLvl;
}

public enum Roles
{
    Fighter, Healer, Archer, Warrior, Mage
}

public enum AnimationTypes
{
    Idle, Attack1, Attack2, Attack3, TakeDmg, Die
}