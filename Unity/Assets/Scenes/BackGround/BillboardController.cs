using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;


public class BillboardController : MonoBehaviour
{

    /* ReactiveProperty */
    ///<summary>Logoの透明度</summary>
    private FloatReactiveProperty rp_Logo_opacity = new FloatReactiveProperty(1f);

    /* パラメーター */
    [Tooltip("Logoの透明度 x:閾値 y:倍率")]
    ///<summary>『Logoの透明度を変更する』テーブル</summary>
    public Vector2[] table_Logo_opacity = new Vector2[4]
    {
        new Vector2(0.66f,  0.99f),
        new Vector2(0.33f,  0.90f),
        new Vector2(0.17f,  0.95f),
        new Vector2(0.01f,  0.99f)
    };
    [Tooltip("床面補正")]
    ///<summary>床面に対する補正値</summary>
    public Vector3 correction_Floor   = new Vector3(     0f,    3.5f, 0f);
    [Tooltip("BackGround_Mountainの移動倍率")]
    ///<summary>プレイヤーの位置に対する、Mountainが移動する倍率</summary>
    public Vector3 moveRatio_Mountain = new Vector3(0.0625f, 0.0625f, 0f);
    [Tooltip("BackGround_Nightの移動倍率")]
    ///<summary>プレイヤーの位置に対する、Nightが移動する倍率</summary>
    public Vector3 moveRatio_Night    = new Vector3(-0.125f, -0.125f, 0f);

    public bool flag_Mountain_Update, flag_Night_Update, flag_Logo_Update;



    /* 変数(半固定) */
    private GameObject c_UnityChan;
    private GameObject c_Mountain, c_Night, c_Logo;
    private Vector3 c_FirstPosition_Mountain, c_FirstPosition_Night;
    private SpriteRenderer c_Logo_SpriteRenderer;

    private void Awake()
    {
        c_UnityChan = GameObject.Find("UnityChan");
        c_Mountain = transform.Find("BackGround_Mountain").gameObject;
        c_Night    = transform.Find("BackGround_Night")   .gameObject;
        c_Logo     = transform.Find("Logo_UnityChan")     .gameObject;

        c_FirstPosition_Mountain = c_Mountain.transform.position;
        c_FirstPosition_Night    = c_Night   .transform.position;

        c_Logo_SpriteRenderer = c_Logo.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Logo_opacityが変化したときに、透明度を変更する
        rp_Logo_opacity.Subscribe(_ => c_Logo_SpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, _));

        /* BackGround_Mountain */
        Observable.EveryUpdate()
            .Where    (_ => flag_Mountain_Update)
            .Subscribe(_ => MovePosition(c_Mountain, c_FirstPosition_Mountain, moveRatio_Mountain))
            .AddTo(c_Mountain);

        /* BackGround_Night */
        Observable.EveryUpdate()
            .Where    (_ => flag_Night_Update)
            .Subscribe(_ => MovePosition(c_Night,    c_FirstPosition_Night,    moveRatio_Night))
            .AddTo(c_Night);

        /* Logo_UnityChan */
        Observable.EveryUpdate()
            .Where(_ => flag_Logo_Update)
            .Subscribe(_ => Logo_Update())
            .AddTo(c_Logo);
    }
    private void Logo_Update() {
        //閾値判定を行い、引っかかるならば、透明度を変更して抜ける
        foreach (var item in table_Logo_opacity){
            if (rp_Logo_opacity.Value >= item.x){ rp_Logo_opacity.Value *= item.y; return; }
        }
        rp_Logo_opacity.Dispose(); //すべての閾値に引っかからなければ、
        Destroy(c_Logo);        //Streamを停止して、Logoを破壊
    }

    ///<summary>位置変更</summary>
    private void MovePosition(GameObject target, Vector3 firstposition,Vector3 ratio)
    {
        Vector3 addPosition = Vector3.Scale(correction_Floor + c_UnityChan.transform.position, ratio);
        target.transform.position = firstposition + addPosition;
    }
}
