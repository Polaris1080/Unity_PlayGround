using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace UI
{
    ///<summary>GameInstanceとUIを繋ぐ</summary>
    public class UI_Parent : MonoBehaviour
    {
        /* 変数 */
        ///<summary>メールボックス</summary>
        public Queue<string> mailbox = new Queue<string>();

        ///<summary>管理しているUI_Child</summary>
        public List<Child> child = new List<Child>();


        /* メインループ */
        void Update()
        {
            //メールボックスの内容をchildに追加
            while(mailbox.Count != 0) { child.Add(new Child(mailbox.Dequeue())); }

            //状態の更新
            foreach (Child item in child){item.Refresh();}
        }


        /* クラス */
        ///<summary>ゲームインスタンス</summary>
        public class Child{
            /* 変数 */
            ///<summary>現在の値</summary>
            private string Now;

            ///<summary>gameinstance内のパラメーター</summary>
            private readonly FieldInfo          fieldInfo;

            ///<summary>管理しているUI_child</summary>
            private readonly UI_Child           UI_child;

            ///<summary>ゲームインスタンス</summary>
            private readonly Level.GameInstance gameinstance;


            /* コンストラクター */
            public Child(string name) {
                this.gameinstance = GameObject.Find("Level").GetComponent<Level.GameInstance>();
                this.fieldInfo    = gameinstance.GetType().GetField(name);
                this.UI_child     = GameObject.Find('$'+name).GetComponent<UI_Child>();
            }


            /* メソッド */
            ///<summary>状態の更新</summary>
            public void Refresh()
            {
                //gameinstanceから現在の値を取得
                string Block = fieldInfo.GetValue(gameinstance).ToString();

                //変化していたら、現在の値を更新して、子のメールボックスに送る
                if (Now != Block) { Now = Block; UI_child.mailbox.Enqueue(Now); }
            }
        }
    }
}