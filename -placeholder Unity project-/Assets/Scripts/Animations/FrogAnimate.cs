using System;
using System.Collections.Generic;
using UnityEngine;

public class FrogAnimate : MonoBehaviour
{
    private static readonly int IsSwim2Hash = Animator.StringToHash("isSwim2");
    private static readonly int IsSwimHash = Animator.StringToHash("isSwim");
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

    private Vector3 ogR;
    private Vector3 ogL;

    void Awake()
    {
        _sm = gameObject.GetComponent<SwimMovement>();
        _ps = gameObject.GetComponent<PlayerState>();

        _rb2D = gameObject.GetComponent<Rigidbody2D>();

        Rleg = RLegAnim.animator.gameObject.transform;
        Lleg = LLegAnim.animator.gameObject.transform;

        ogR = Rleg.localPosition;
        ogL = Lleg.localPosition;
    }

    void FixedUpdate()
    {
        bool isMoving = _sm.IsMoving;
        bool isOut = _ps.CurrentState == PlayerState.PlayerStates.ABOVEWATER;
        bool isWalk = isMoving && isOut;

        bool isX = _rb2D.linearVelocityX > 0;

        HeadAnim.animator.SetBool(IsSurfaceHash, isOut);
        HeadAnim.animator.SetBool(IsSwimHash, isMoving);

        ArmsAnim.animator.SetBool(IsSwimHash, isMoving && !isWalk);

        RLegAnim.animator.SetBool(IsSwimHash, isMoving);
        LLegAnim.animator.SetBool(IsSwim2Hash, isMoving);
        
        HeadAnim.sprite.flipX = isX;
        ArmsAnim.sprite.flipX = isX;

        if (isWalk)
        {
            if (isX)
            {
                LLegAnim.sprite.flipY = false;
                RLegAnim.sprite.flipY = false;

                Lleg.localPosition = ogR;
                Rleg.localPosition = ogR;
            }
            else
            {
                LLegAnim.sprite.flipY = true;
                RLegAnim.sprite.flipY = true;

                Lleg.localPosition = ogL;
                Rleg.localPosition = ogL;
            }
        }
        else
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
