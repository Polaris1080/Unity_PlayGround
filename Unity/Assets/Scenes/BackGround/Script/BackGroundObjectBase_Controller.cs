using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Billboard
{
    public class BackGroundObjectBase_Controller : MonoBehaviour
    {
        /* ReactiveProperty */
        ///<summary>現在のUnitychanの位置</summary>
        private Vector3ReactiveProperty rp_UnityChan_position = new Vector3ReactiveProperty();

        /* パラメーター */
        [Tooltip("床の位置")]
        ///<summary>床の位置の分だけ補正</summary>
        public Vector3 p_correction_floor = new Vector3(0f, 3.5f, 0f);
        [Tooltip("移動の倍率")]
        ///<summary>Unitychanの位置に対する移動の倍率</summary>
        public Vector3 p_move_ratio       = new Vector3(0f,   0f, 0f);
        [Tooltip("移動するか")]
        ///<summary>移動するか</summary>
        public bool    p_flag_move        = true;

        /* 変数(半固定) */
        ///<summary>UnityChanへのリンク</summary>
        private GameObject c_UnityChan;
        ///<summary>初期位置</summary>
        private Vector3    c_FirstPosition;


        /* メインループ */
        ///<summary>起動時</summary>
        private void Awake()
        {
            c_UnityChan     = GameObject.Find("UnityChan");
            c_FirstPosition = this.transform.position;
        }

        ///<summary>ゲーム開始時</summary>
        void Start()
        {
            //UnityChanの位置が変化したら移動
            rp_UnityChan_position
                .Distinct ()
                .Subscribe(_ => this.transform.position = c_FirstPosition + Vector3.Scale(p_correction_floor + _, p_move_ratio));

            //UnityChanの位置を監視
            Observable.EveryUpdate()
                .Subscribe(_ => rp_UnityChan_position.Value = p_flag_move ? c_UnityChan.transform.position : rp_UnityChan_position.Value)
                .AddTo    (this.gameObject);
        }
    }
}
