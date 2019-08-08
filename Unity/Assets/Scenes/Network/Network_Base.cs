/*
 *  基底クラス
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace Network.Base
{
    ///<summary>Network_CRUDの基底クラス</summary>
    public abstract class Network_Base
    {
        /* 変数 */
        ///<summary>通信先のURL</summary>  
        protected readonly string url;

        /* コンストラクター */
        protected Network_Base(string url)            => this.url = $"http://{url}";
        protected Network_Base(string url, string id) => this.url = $"http://{url}/{id}";
    }

    ///<summary>[C]RUD</summary>
    public abstract class Network_Create : Network_Base
    {
        /* コンストラクター */
        protected Network_Create(string url) : base(url) { }

        /* メソッド */
        public IEnumerator Create()
        {
            //POSTリクエストを生成
            UnityWebRequest request = new UnityWebRequest(this.url, "POST"){ uploadHandler   = new UploadHandlerRaw(MakeJson()),
                                                                             downloadHandler = new DownloadHandlerBuffer()       };
            //ヘッダーにタイプを設定
            request.SetRequestHeader("Content-Type", "application/json");

            //送信
            yield return request.SendWebRequest();

            //エラー判定・確認
            if (request.isHttpError || request.isNetworkError) { Debug.Log(request.error); }

            //後処理
            PostProcess();
        }

        /* 抽象メソッド */
        ///<summary>後処理</summary> 
        protected abstract void PostProcess();

        ///<summary>データを作成してbyteに変換</summary> 
        protected abstract byte[] MakeJson();
    }

    ///<summary>C[R]UD</summary>
    public abstract class Network_Read : Network_Base
    {
        /* コンストラクター */
        protected Network_Read(string url, string id) : base(url, id) { }

        /* メソッド */
        public IEnumerator Read()
        {
            //GETリクエストを生成
            UnityWebRequest request = new UnityWebRequest(this.url,"GET");

            //サーバーに送信
            yield return request.SendWebRequest();

            //エラー確認
            if (request.isHttpError || request.isNetworkError) { Debug.Log(request.error); }
            //後処理
            else                                               { PostProcess(request); }
        }

        /* 抽象メソッド */
        ///<summary>後処理（受信したデータを処理する）</summary> 
        public abstract void PostProcess(UnityWebRequest request);
    }

    ///<summary>CR[U]D</summary>
    public abstract class Network_Update : Network_Base
    {
        /* コンストラクター */
        protected Network_Update(string url, string id) : base(url, id) { }

        /* メソッド */
        public IEnumerator Update() //DBからユーザー情報を読み込む
        {
            //POSTリクエストを生成
            UnityWebRequest request = new UnityWebRequest(this.url, "PUT") { uploadHandler   = new UploadHandlerRaw(MakeJson()),
                                                                             downloadHandler = new DownloadHandlerBuffer()       };
            //ヘッダーにタイプを設定
            request.SetRequestHeader("Content-Type", "application/json");

            //送信
            yield return request.SendWebRequest();

            //エラー判定・確認
            if (request.isHttpError || request.isNetworkError) { Debug.Log(request.error); }

            //後処理
            PostProcess();
        }

        /* 抽象メソッド */
        ///<summary>データを作成してbyteに変換</summary> 
        public abstract byte[] MakeJson();

        ///<summary>後処理</summary> 
        public abstract void PostProcess();
    }
}
