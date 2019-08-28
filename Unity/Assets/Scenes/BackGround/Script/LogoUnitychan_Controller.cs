using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class LogoUnitychan_Controller : MonoBehaviour
{
    /* ReactiveProperty */
    ///<summary>Logoの透明度</summary>
    private FloatReactiveProperty rp_Logo_opacity = new FloatReactiveProperty(1f);

    /* テーブル */
    [Tooltip("Logoの透明度 x:閾値 y:倍率")]
    ///<summary>『Logoの透明度を変更する』テーブル</summary>
    public Vector2[] t_Logo_opacity = new Vector2[4]
    {
        new Vector2(0.66f,  0.250f),
        new Vector2(0.33f,  0.667f),
        new Vector2(0.17f,  0.500f),
        new Vector2(0.01f,  0.334f)
    };

    /* パラメーター */
    [Tooltip("更新するか")]
    ///<summary>更新するか</summary>
    public bool p_flag_Update;

    /* 変数(半固定) */
    ///<summary>SpriteRendererへのリンク</summary>
    private SpriteRenderer c_SpriteRenderer;


    /* メインループ */
    ///<summary>起動時</summary>
    private void Awake()
    {
        c_SpriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    ///<summary>ゲーム開始時</summary>
    void Start()
    {
        //Logo_opacityが変化したときに、透明度を変更する
        rp_Logo_opacity
            .Distinct ()
            .Subscribe(_ => c_SpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, _));

        //透明度の変化
        Observable.EveryUpdate()
            .Subscribe(_ => Logo_Update())
            .AddTo    (this.gameObject);
    }

    private void Logo_Update()
    {
        if (p_flag_Update)
        {
            //閾値判定を行い、引っかかるならば、透明度を変更して抜ける
            foreach (var item in t_Logo_opacity)
            {
                if (rp_Logo_opacity.Value >= item.x) { rp_Logo_opacity.Value -= item.y * Time.deltaTime; return; }
            }
            //すべての閾値に引っかからなければ、Streamを停止して、Logoを破壊
            rp_Logo_opacity.Dispose();
            Destroy(this.gameObject);
        }
    }
}
