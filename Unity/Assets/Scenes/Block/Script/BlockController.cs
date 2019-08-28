/*
 *  [UnityChan2Dから改変]
 *  ブロックの挙動について定義
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLibrary;
using UniRx;
using UniRx.Triggers;
[RequireComponent(typeof(BoxCollider2D))]


public class BlockController : MonoBehaviour
{
    /* パラメーター */
    [Tooltip("破壊後にスポーンされるブロック")]
    public  GameObject p_brokenBlock = default;
    [Tooltip("破壊後にスポーンされるコイン")]
    public  GameObject p_spawnCoin   = default;
    [Tooltip("破壊可能かどうか")]
    public  bool       p_breakable   = true;
    [Tooltip("コインの出現率"), Range(0f, 1f)]
    public  float      p_coinSpawn   = 0.5f;

    /* パラメーター(半固定) */
    [SerializeField, Tooltip("コリジョン2Dレイヤー")]
    private LayerMask cp_whatIsPlayer = default;
    
    /* 変数(半固定) */
    private Level.GameInstance c_gameinstance;
    private BoxCollider2D      c_collision;


    /* メインループ */
    ///<summary>起動時</summary>
    private void Awake()
    {
        c_collision    = GetComponent<BoxCollider2D>();
        c_gameinstance = GameObject.Find("Level").GetComponent<Level.GameInstance>();
    }

    ///<summary>ゲーム開始時</summary>
    private void Start()
    {
        //コリジョン接触検知
        this.OnCollisionEnter2DAsObservable()
            //Unitychanが当たって来たとき
            .Where(_ => _.gameObject.tag == "Player")
            //破壊可能かどうか確認
            .Where(_ => p_breakable)
            //当たり判定の確認
            .Where(_ => OverlapArea.Check(transform.position.x,
                                          transform.position.y - transform.lossyScale.y,
                                          c_collision.size.x * 0.49f,
                                          0.05f,
                                          cp_whatIsPlayer))
            .Subscribe(_ => OnCollision());
    }

    private void OnCollision()
    {
                c_gameinstance.BreakBlock(); //GameInstance.Block++;

                //抽選に成功すればコインをスポーン、失敗すればブロックだけ。
                if (UnityEngine.Random.Range(0f, 1f) <= p_coinSpawn) {
                    Spawn.Sameplace(p_spawnCoin,   this.transform);
                    Spawn.Sameplace(p_brokenBlock, this.transform);
                }     
                else {
                    Spawn.Sameplace(p_brokenBlock, this.transform);
                }

                Destroy(gameObject); //自身を破壊 
    }
    
}