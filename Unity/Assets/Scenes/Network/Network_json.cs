/*
 * ネットワーク越しに送受信するjsonを定義
 */
using System.Collections;
using System.Collections.Generic;

namespace Network.Json
{
    [System.Serializable] ///<summary>DBへの送受信用</summary>
    public struct Data_DB  { public string name;
                             public int    block; }

    [System.Serializable] ///<summary>Logへの送信用</summary>
    public struct Data_LOG { public int id;
                             public int add; }
}