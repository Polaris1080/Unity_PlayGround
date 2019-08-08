using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    ///<summary>Parentから情報を受け取り、UIを変更する。（基底クラス）</summary>
    public class UI_Child : MonoBehaviour
    {
        ///<summary>メールボックス</summary>
        public Queue mailbox = new Queue();


        ///<summary>親のメールボックスに送信</summary>
        void Start() => GameObject.Find("UI").GetComponent<UI.UI_Parent>().mailbox.Enqueue(this.name.Replace("$", ""));
    }
}
