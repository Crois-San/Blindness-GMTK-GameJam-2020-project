using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : Character
{
    //коллайдеры, нужный для проверки при движении
    private Collider2D[] collidersNextToEntity, collidersAboveEntity, collidersUnderEntity, collidersUnderPlatform;
    //коллайдер, для поиска ввода 
    private Collider2D input;

    private Collision2D otherCollision;
    //основной таймер
    private float timer;
    //таймер, по истечению которого враг перестает преследовать игрока
    private float attentionTimerPlayer;
    //таймер, считающий время, за которое враг должен успеть изменить элемент
    private float attentionTimerInput;
    public int CollisionCount { get; set; }
    //интерфейсы
    private IDamageDealer dd;
    private IHealthSystem hs;
    private ISoundSystem ss, ssRoar, ssHurt, ssChase;

    private bool isPlaying = false;
    
    //скорость движения в спокойном состоянии
    [SerializeField]
    private float speedIdle = 0.05f;
    //скорость движения в состоянии преследования
    [SerializeField]
    private float speedChasing = 0.1f;
    private float groundHeight;
    RaycastHit2D raycastResult;
    private Vector3 rayOriginPoint;
    [SerializeField]
    //маски для поиска игрока и ввода соответственно
    private LayerMask targetPlayerMask;

    //объекты игрока и ввода соответственно
    private GameObject targetPlayer, targetInput;

    [SerializeField] 
    private int damagePoints = 1;
    [SerializeField] 
    private float damageDelay = 3f;

    [SerializeField] 
    private float knockbackStrength = 1f;

    //геттеры
    public GameObject getTargetPlayer => targetPlayer;
    public GameObject getTargetInput => targetInput;
    public Collider2D[] getCollidersNextToEntity => collidersNextToEntity;
    public int getDamagePoints => damagePoints;
    public int setDamagePoints
    {
        set => damagePoints = value;
    }


    protected override void Moving(float move)
    {
        //поиск препятствий перед врогом
        collidersNextToEntity =
            Physics2D.OverlapCircleAll(transform.position + move * 2.0f * Vector3.right, 0.2f, mask);
        //поиск пропастей перед врагом
        collidersUnderEntity =
            Physics2D.OverlapCircleAll(
                transform.position + (characterSize.y / 2 + 0.5f) * Vector3.down + move * (characterSize.x/2 + 0.2f) * Vector3.right, 0.2f,
                mask);
        //поиск препятствий над врогом
        collidersAboveEntity =
            Physics2D.OverlapCircleAll(transform.position + move * 2.0f * Vector3.right + (characterSize.y / 2 + 0.5f) * Vector3.up, 0.2f, mask);
        /*
         * поиск  платформы под найденной платформой,
         * таким образом враг может спрыгнуть при небольшой высоте
         */ 
        if (collidersUnderEntity.Length !=0)
        {
            groundHeight = collidersUnderEntity[0].bounds.size.y;
            collidersUnderPlatform = Physics2D.OverlapCircleAll(
                transform.position + (characterSize.y / 2 + 1f) * Vector3.down + move * (characterSize.x/2 + 0.2f) * Vector3.right, 0.2f,
                mask);
        }
        
        //если есть препятсвие, враг запрыгнет на него, если впереди стена, то вместо прыжка развернется
        if (collidersNextToEntity.Length > 0 && collidersAboveEntity.Length == 0)
        {
            jumpRequest = true;
        } else if (collidersAboveEntity.Length > 0)
        {
            moving *= -1;
        }

        Jumping(); 
        // если есть пропасть впереди, враг развернется
        if (collidersUnderPlatform != null) 
        {
            if (collidersUnderEntity.Length == 0 && collidersUnderPlatform.Length == 0 &&!jumpRequest)
            {
                moving *= -1;
            } 
        }
        

        base.Moving(move);
    }

    protected override void Jumping()
    {
        if (grounded && jumpRequest)
        {
            body.AddForce(SpeedMultiplier * Time.deltaTime * jumpspeed*Vector2.up, ForceMode2D.Impulse);
            jumpRequest = false;
            grounded = false;
        }
        else
        {
            //проверка касанния с платформой, подробнее в комментариях в Character.cs
            Vector2 boxCenter = (Vector2) body.transform.position + 0.5f * (characterSize.y + boxSize.y) * Vector2.down;
            grounded = (Physics2D.OverlapBox(boxCenter, boxSize, 0f, mask) != null);
        }

        if (body.velocity.y < 0f)
        {
            body.gravityScale = fallMultiplier;
        }
    }


    //спокойное состояние
    void StateIdle()
    {
        
        speed = speedIdle;
        //каждые десять секунд враг разворвчивается в обратную сторону
        if (timer >= 10)
        {
            moving *= -1;
            timer = 0.0f;
        }

        Moving(moving);
    }

    //состояние преследования
    void StatePlayerChasing()
    {
        if (!isPlaying)
        {
            ssChase.MakeSound();
            isPlaying = true;
        }
        
        
        //изменяет направление движения, если игрок прошел мимо врага
        moving =Mathf.Sign((targetPlayer.transform.position - transform.position).x) ;
        attentionTimerPlayer += Time.deltaTime % 60;
        speed = speedChasing;
        Moving(moving);
        //по истечении таймера враг теряет внимание и возращается в спокойное состояние
        if (attentionTimerPlayer >= 3)
        {
            targetPlayer = null;
            attentionTimerPlayer = 0;
        }

    }



    // Start is called before the first frame update
    protected override void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        characterSize = GetComponent<BoxCollider2D>().size;
        sr = GetComponent<SpriteRenderer>();
        body.gravityScale = 1f;
        /*
         * Размер области для определения соприкосновения с землей,
         * groundedSkin - высота области.
         */
        boxSize = new Vector2(characterSize.x, groundedSkin);
        moving = 1;
        speed = 0.05f;
        grounded = false;
        dd = new DamageSystem(gameObject,damagePoints,knockbackStrength,damageDelay);
        hs = new HealthSystem(healthPoints,gameObject);
        ssRoar = new SoundSystemDefaultLooping(gameObject,Sounds.MonsterRoar, 0.5f);
        ssChase = new SoundSystemDefaultOneshot(gameObject, Sounds.MonsterChasing, 0.5f);
        ssHurt = new SoundSystemDefault(gameObject, Sounds.MonsterHurt, 0.5f);
        ssRoar.MakeSound();
    }

    // Update is called once per frame
    protected override void Update()
    {
        dd.DamageDealing(otherCollision);
        hs.NpcDeath();
    }

    protected override void FixedUpdate()
    {
        timer += Time.deltaTime % 60;
        //начальная точка для поиска персонажа
        rayOriginPoint = transform.position + moving * characterSize.x * Vector3.right;
        /*
         * Поиск персонажа. Если персонаж попадает в поле зрения врага, он переходит в состояние преследования.
         * Луч для поиска разворачивается вместе с персонажем.
         */
        raycastResult = Physics2D.Raycast(rayOriginPoint, moving*Vector3.right,15f, targetPlayerMask);
        if (raycastResult)
        {
            //запоминает персонажа
            targetPlayer = raycastResult.collider.gameObject;
        }
        if (raycastResult || targetPlayer)
        {
            StatePlayerChasing();
        }
        else
        {
            StateIdle();
        }
            
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        CollisionCount ++;
        otherCollision = other;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        CollisionCount --;
        otherCollision = null;
    }
}