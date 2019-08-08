/* 
 *  ゲーム内での値を保持する。
 *  Configとしても活用している。
 *  UnrealEngine4のGameInstanceからリスペクト。
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class GameInstance : MonoBehaviour
    {
        [Header("Default")]
        [Tooltip("自分のＩＤ（基本は１で固定）")]
        public int ID = 1;

        [Tooltip("自分の名前（未接続時に適応）")]
        public string Name = "FooBar";

        [Tooltip("破壊したブロック数（未接続時に適応）")]
        public int Block = 0;


        [Header("Spawn")]
        [Tooltip("スポーンさせるブロック")]
        public GameObject spawntarget;

        [Tooltip("スポーンさせる間隔")]
        public Vector3 span_spawn = new Vector3 (0.5f,1,2);

        [Tooltip("Blockのしきい値")]
        public Vector3Int threshold_spawn = new Vector3Int(5,10,15);

        [Tooltip("スポーンさせる範囲")]
        public Vector2Int range_spawn = new Vector2Int(7,1);

        [Tooltip("スポーン履歴（記憶されている場所にはスポーンしない）")]
        public int memory_spawn = 8;


        [Header("Network")]
        [Tooltip("DBにUpdateする間隔")]
        public int netInterval = 5;

        /* ゲッター・セッター */
        ///<summary>ブロック破壊</summary>
        public void BreakBlock() => Block++;
        ///<summary>ブロック取得</summary>
        public int  Get_Block()  => Block;



    }
}


