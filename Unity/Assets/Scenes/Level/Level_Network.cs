/*
 *  http通信について定義。
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;

namespace Network
{
    public partial class Level_Network : MonoBehaviour
    {
        /* 設定 */
        [SerializeField, Tooltip("インターネット接続機能")]
        private readonly bool ConnectNetwork = true;

        /* 変数 */
        ///<summary>ゲームインスタンス</summary>
        private Level.GameInstance         gameinstance;

        ///<summary>接続するアドレスのリスト</summary>
        private Dictionary<string, string> URLlist;


        /* メインループ */
        ///<summary>起動時</summary>
        private void Awake() => enabled = ConnectNetwork; //インターネット接続機能が不要な場合は無効化

        ///<summary>ゲーム開始時</summary>
        private void Start()
        {
            gameinstance = GameObject.Find("Level").GetComponent<Level.GameInstance>();
            URLlist = new Dictionary<string, string>(Initialize_URLlist());
            StartCoroutine(LoadDB());         //DBからユーザー情報を読み込む
            StartCoroutine(UpdateRepeater()); //定期的にUpdateDBを実行
        }

        ///<summary>終了時はDBに退避</summary>
        private void OnApplicationQuit() { if(ConnectNetwork){StartCoroutine(UpdateDB());} }

        ///<summary>定期的にUpdateDBを実行</summary>
        private IEnumerator UpdateRepeater()
        {
            while (true) //intervalが経過したら
            {
                StartCoroutine(UpdateDB());    //DBを更新
                StartCoroutine(AddBlockLOG()); //LOGに追記
                yield return new WaitForSeconds(this.gameinstance.netInterval);
            }
        }


        /* メソッド */
        ///<summary>アドレスリストの初期化</summary>
        private static Dictionary<string, string> Initialize_URLlist() {
            Dictionary<string, string> URLlist = new Dictionary<string, string>();

            //Resourceからファイルを読み込む
            TextAsset xmlTextAsset = Resources.Load("Config") as TextAsset;

            //テーブルからデータを抽出し、URLlistに追加
            foreach (XElement row in XDocument.Parse(xmlTextAsset.text).Element("Network").Elements("List"))
            {
                URLlist.Add(row.Element("NAME").Value, $"{row.Element("IP").Value}:{row.Element("PORT").Value}/{row.Element("URL").Value}");
            }

            return URLlist;
        }
    }
}
