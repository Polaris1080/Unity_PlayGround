/*
 *  ラッパークラス
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Network
{
    public partial class Level_Network : MonoBehaviour
    {
        ///<summary>DBからデータを読み込む</summary>
        private IEnumerator LoadDB()      => new Implement.LoadDB     (URLlist["DataBase"], gameinstance.ID.ToString()).Read();

        ///<summary>DBのブロック破壊数を更新する</summary>
        private IEnumerator UpdateDB()    => new Implement.UpdateDB   (URLlist["DataBase"], gameinstance.ID.ToString()).Update();

        ///<summary>LOGにブロック破壊数を追記する</summary>
        private IEnumerator AddBlockLOG() => new Implement.AddBlockLOG(URLlist["Log"])                                 .Create();
    }
}
