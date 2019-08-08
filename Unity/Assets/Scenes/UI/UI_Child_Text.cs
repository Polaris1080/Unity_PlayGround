using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    ///<summary>Parentから情報を受け取り、UIを変更する。（テキスト）</summary>
    public class UI_Child_Text : UI_Child
    {
        ///<summary>メールボックスに入っていれば更新</summary>
        void Update() { if (this.mailbox.Count != 0) { this.gameObject.GetComponent<Text>().text = this.mailbox.Dequeue().ToString(); } }
    }
}
