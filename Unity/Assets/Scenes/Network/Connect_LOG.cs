using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network.Json;

namespace Network.Implement
{
    ///<summary>LOGにブロック破壊数を追記する</summary>
    public sealed class AddBlockLOG : Base.Network_Create
    {
        /* 変数 */
        ///<summary>操作するGameInstance</summary>
        private readonly Level.GameInstance gameinstance;

        ///<summary>前回送信時のブロック破壊数</summary>
        private static int Block_lastsend = 0;

        /* コンストラクター */
        ///<param name="gameinstance">操作するGameInstance</param>
        ///<param name="url">例えば：127.0.0.1:8000/Unity/log</param>
        public AddBlockLOG(string url) : base(url) => this.gameinstance = GameObject.Find("Level").GetComponent<Level.GameInstance>();

        /* メソッド */
        ///<summary>データを作成してbyteに変換</summary>
        protected override byte[] MakeJson()
        {
            //jsonを設定
            Data_LOG data = new Data_LOG { id  = this.gameinstance.ID,
                                           add = this.gameinstance.Block - Block_lastsend };
            //byteに変換
            return System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
        }

        ///<summary>後処理（現在のブロック破壊数を記録）</summary> 
        protected override void PostProcess() => Block_lastsend = this.gameinstance.Block;
    }
}
