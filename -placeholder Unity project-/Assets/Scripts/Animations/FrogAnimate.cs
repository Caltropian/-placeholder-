using System;
using System.Collections.Generic;
using UnityEngine;

public class FrogAnimate : MonoBehaviour
{
    private static readonly int GoingUpHash = Animator.StringToHash("goingUp");
    private static readonly int IsSwim2Hash = Animator.StringToHash("isSwim2");
    private static readonly int IsSwimHash = Animator.StringToHash("isSwim");
    private static readonly int IsWalk2Hash = Animator.StringToHash("isWalk2");
    private static readonly int IsWalkHash = Animator.StringToHash("isWalk");
    private static readonly int IsSurfaceHash = Animator.StringToHash("isSurface");

    [SerializeField] AnimatedPart HeadAnim;
    [SerializeField] AnimatedPart ArmsAnim;
    [SerializeField] AnimatedPart RLegAnim;
    [SerializeField] AnimatedPart LLegAnim;
    private Transform Rleg;
    private Transform Lleg;

    private Rigidbody2D _rb2D;

    private SwimMovement _sm;
    private PlayerState _ps;
    private WalkMovement _wm;

    private Vector3 ogR;
    private Vector3 ogL;

    // [Space(20)]
    // [SerializeField]
    // private CapsuleCollider2D walkCollider;

    void Awake()
    {
        _sm = gameObject.GetComponent<SwimMovement>();
        _ps = gameObject.GetComponent<PlayerState>();
        _wm = gameObject.GetComponent<WalkMovement>();

        _rb2D = gameObject.GetComponent<Rigidbody2D>();

        Rleg = RLegAnim.animator.gameObject.transform;
        Lleg = LLegAnim.animator.gameObject.transform;

        ogR = Rleg.localPosition;
        ogL = Lleg.localPosition;
    }

    void FixedUpdate()
    {
        float animSpeed = Math.Abs(_rb2D.linearVelocityX) < 1f || _sm.IsBoosting ? 1 : Math.Clamp(Math.Abs(_rb2D.linearVelocityX), 1, 2);

        LLegAnim.animator.speed = animSpeed;
        RLegAnim.animator.speed = animSpeed;
        HeadAnim.animator.speed = animSpeed;
        ArmsAnim.animator.speed = animSpeed;

        bool isSwimming = _sm.IsMoving;
        bool fishOuttaWater = _ps.CurrentState == PlayerState.PlayerStates.ABOVEWATER;
        bool isWalk = _wm.IsMoving;

        bool isX = _rb2D.linearVelocityX > 0;

        HeadAnim.animator.SetBool(IsSurfaceHash, fishOuttaWater /*&& !isWalk*/);
        HeadAnim.animator.SetBool(IsSwimHash, isSwimming);

        ArmsAnim.animator.SetBool(IsSwimHash, isSwimming && !fishOuttaWater);
        ArmsAnim.animator.SetBool(IsWalkHash, !isSwimming && isWalk && fishOuttaWater);

        RLegAnim.animator.SetBool(IsSwimHash, isSwimming && !fishOuttaWater);
        LLegAnim.animator.SetBool(IsSwim2Hash, isSwimming && !fishOuttaWater);

        RLegAnim.animator.SetBool(IsWalkHash, isWalk && fishOuttaWater);
        LLegAnim.animator.SetBool(IsWalk2Hash, isWalk && fishOuttaWater);
        
        HeadAnim.sprite.flipX = isX;
        ArmsAnim.sprite.flipX = isX;

        if (isWalk && fishOuttaWater)
        {
            // HeadAnim.sprite.sortingOrder = 5;

            // HeadAnim.animator.SetBool(GoingUpHash, false);

            RLegAnim.sprite.sortingOrder = 0;

            LLegAnim.sprite.flipY = !isX;
            RLegAnim.sprite.flipY = !isX;

            float allOffset = isX ? -1 : -1.5f;
            float offset = isX ? -.75f : +.75f;

            Lleg.localPosition = new(ogR.x + allOffset + offset, ogR.y + .5f);
            Rleg.localPosition = new(ogR.x + allOffset, ogR.y + .5f);
        }
        // else if (fishOuttaWater)
        //     HeadAnim.sprite.sortingOrder = 1;
        
        if (!fishOuttaWater)
        {
            LLegAnim.sprite.flipY = true;
            RLegAnim.sprite.flipY = false;

            Lleg.localPosition = ogL;
            Rleg.localPosition = ogR;
        }
    }
}

[Serializable]
class AnimatedPart
{
    public Animator animator;
    public SpriteRenderer sprite;
}
