/*
 *  [UnityChan2Dから改変]
 *  Unitychanの挙動について定義
 */
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(BoxCollider2D))]


public class UnityChan2DController : MonoBehaviour
{
    /* パラメーター */
    [Tooltip("移動速度（最速）"), Range(7.5f, 12.5f)]
    public  float         maxSpeed      = 10f;
    [Tooltip("ジャンプ力"), Range(900f, 1200f)]
    public  float         jumpPower     = 1000f;
    [Tooltip("地面が存在するレイヤー")]
    public  LayerMask     whatIsGround;

    /* 変数(半固定) */
    private Animator      c_animator;
    private BoxCollider2D c_boxcollier2D;
    private Rigidbody2D   c_rigidbody2D;

    /* 変数 */
    private bool          isGround; //地面に接地しているか？

    /* 定数 */
    private const float   co_offsetOverlapAreaY = 1.5f; //当たり判定のオフセット（本体の中心から下）


    /* メインループ */
    ///<summary>リセット</summary>
    void Reset()
    {
        Awake();

        // UnityChan2DController
        maxSpeed      = 10f;
        jumpPower     = 1000;
        whatIsGround  = 1 << LayerMask.NameToLayer("Ground");

        // Transform
        transform.localScale = new Vector3(1, 1, 1);

        // Rigidbody2D
        c_rigidbody2D.gravityScale   = 3.5f;
        c_rigidbody2D.freezeRotation = true;

        // BoxCollider2D
        c_boxcollier2D.size   = new Vector2(1, 2.5f);
        c_boxcollier2D.offset = new Vector2(0, -0.25f);

        // Animator
        c_animator.applyRootMotion = false;
    }

    ///<summary>起動時</summary>
    void Awake()
    {
        c_animator     = GetComponent<Animator>();
        c_boxcollier2D = GetComponent<BoxCollider2D>();
        c_rigidbody2D  = GetComponent<Rigidbody2D>();
    }

    void Update() { Move(Input.GetAxis("Horizontal"), Input.GetButtonDown("Jump")); }

    ///<summary>移動</summary>
    void Move(float move, bool jump)
    {
        float[] T = new float[3];//Temp

        if (Mathf.Abs(move) > 0) //方向転換
        {
            T[0] = transform.rotation.x;            //Ｘ軸
            T[1] = Mathf.Sign(move) == 1 ? 0 : 180; //Ｙ軸
            T[2] = transform.rotation.z;            //Ｚ軸
            transform.rotation = Quaternion.Euler(T[0], T[1], T[2]);
        }
        
        //加速
        T[0] = move * maxSpeed;          //Ｘ軸
        T[1] = c_rigidbody2D.velocity.y; //Ｙ軸
        c_rigidbody2D.velocity = new Vector2(T[0], T[1]);

        //アニメーションに情報を送る
        c_animator.SetFloat("Horizontal", move);
        c_animator.SetFloat("Vertical",   c_rigidbody2D.velocity.y);
        c_animator.SetBool ("isGround",   isGround);

        if (jump && isGround)
        {
            //アニメーションに情報を送る
            c_animator.SetTrigger("Jump");
            SendMessage("Jump", SendMessageOptions.DontRequireReceiver);

            //上向きに力を加えてジャンプ
            c_rigidbody2D.AddForce(Vector2.up * jumpPower);
        }
    }

    ///<summary>物理の更新</summary>
    void FixedUpdate()
    {
        isGround = MyLibrary.OverlapArea.Check(transform.position.x,          transform.position.y - (co_offsetOverlapAreaY * transform.localScale.y),
                                               c_boxcollier2D.size.x * 0.49f, 0.05f,
                                               whatIsGround);
        //アニメーションに結果を送る
        c_animator.SetBool("isGround", isGround);
    }
}