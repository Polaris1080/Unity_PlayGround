using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network.Json;

namespace Network.Implement
{
    ///<summary>DBのブロック破壊数を更新する</summary>
    public sealed class UpdateDB : Base.Network_Update
    {
        /* 変数 */
        ///<summary>操作するGameInstance</summary>
        private readonly Level.GameInstance gameinstance;

        /* コンストラクター */
        ///<param name="gameinstance">操作するGameInstance</param>
        ///<param name="url">例えば：127.0.0.1:8000/Unity/DB</param>
        ///<param name="id">例えば：1911</param>
        public UpdateDB(string url, string id) : base(url, id) => this.gameinstance = GameObject.Find("Level").GetComponent<Level.GameInstance>();

        /* メソッド */
        ///<summary>データを作成してbyteに変換</summary>
        public override byte[] MakeJson()
        {
            //jsonを設定
            Data_DB data = new Data_DB { name  = this.gameinstance.Name,
                                         block = this.gameinstance.Block };
            //byteに変換
            return System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
        }

        /* 抽象メソッド */
        ///<summary>後処理</summary> 
        public override void PostProcess(){}
    }

    ///<summary>DBからデータを読み込む</summary>
    public sealed class LoadDB : Base.Network_Read
    {
        /* 変数 */
        ///<summary>操作するGameInstance</summary>
        private readonly Level.GameInstance gameinstance;

        /* コンストラクター */
        ///<param name="gameinstance">操作するGameInstance</param>
        ///<param name="url">例えば：127.0.0.1:8000/Unity/DB</param>
        ///<param name="id">例えば：1918</param>
        public LoadDB(string url, string id) : base(url, id) => this.gameinstance = GameObject.Find("Level").GetComponent<Level.GameInstance>();

        /* 抽象メソッド */
        ///<summary>後処理（受信したデータを処理する）</summary> 
        ///<param name="request">サーバーからの返信</param>
        public override void PostProcess(UnityEngine.Networking.UnityWebRequest request)
        {
            //受信したjsonを変換
            Data_DB jsonClass = JsonUtility.FromJson<Data_DB>(request.downloadHandler.text);

            //jsonを使用して更新
            { this.gameinstance.Name  = jsonClass.name;
              this.gameinstance.Block = jsonClass.block; }
        }
    }
}
