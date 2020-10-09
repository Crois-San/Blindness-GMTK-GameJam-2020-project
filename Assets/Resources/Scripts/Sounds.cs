using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Звуки ходьбы
 * Звук прыжка
 * Звук при получении урона
 * Звук при смерти персонажа
 * Звук переключения входов
 * Звук включенных элементов
 * Звук движения поршня
 * Звук эмбиента
 * Звук победы
 */
public class Sounds
{
    public AudioClip Sound { get; set; }
    public Sounds(AudioClip sound)
    {
        Sound = sound;
    }
    public static Sounds FootstepStone
    {
        get { return new Sounds(Resources.Load<AudioClip>("Sounds/footstepStone")); }
    }
    public static Sounds FootstepGrass
    {
        get { return new Sounds(Resources.Load<AudioClip>("Sounds/footstepGrass")); }
    }
    public static Sounds FootstepSand
    {
        get { return new Sounds(Resources.Load<AudioClip>("Sounds/footstepSand")); }
    }
    public static Sounds FootstepWood
    {
        get { return new Sounds(Resources.Load<AudioClip>("Sounds/footstepWood")); }
    }
    public static Sounds DamageHit
    {
        get { return new Sounds(Resources.Load<AudioClip>("Sounds/damage")); }
    }
    public static Sounds DeathScore
    {
        get { return new Sounds(Resources.Load<AudioClip>("Sounds/DeathScore")); }
    }
    public static Sounds VictoryScore
    {
        get { return new Sounds(Resources.Load<AudioClip>("Sounds/VictoryScore")); }
    }
    public static Sounds JumpSound
    {
        get { return new Sounds(Resources.Load<AudioClip>("Sounds/jump")); }
    }
    public static Sounds GunShot
    {
        get { return new Sounds(Resources.Load<AudioClip>("Sounds/gunShot")); }
    }
    public static Sounds GunCollision
    {
        get { return new Sounds(Resources.Load<AudioClip>("Sounds/gunCollision")); }
    }
    public static Sounds MonsterRoar
    {
        get { return new Sounds(Resources.Load<AudioClip>("Sounds/monsterRoar")); }
    }
    public static Sounds MonsterHurt
    {
        get { return new Sounds(Resources.Load<AudioClip>("Sounds/monsterHurt")); }
    }
    public static Sounds MonsterChasing
    {
        get { return new Sounds(Resources.Load<AudioClip>("Sounds/monsterChasing")); }
    }
    public static Sounds WallCollision
    {
        get { return new Sounds(Resources.Load<AudioClip>("Sounds/wallCollision")); }
    }

}
