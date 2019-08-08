using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyLibrary
{
    ///<summary>当たり判定</summary>
	public class OverlapArea{
        ///<summary>当たり判定を確認する</summary>
		public static bool Check(float pointCenter_x,float pointCenter_y,float overlapArea_x,float overlapArea_y,LayerMask layermask)
		{
			    float[] T = new float[4];  T[0] = pointCenter_x;  T[1] = overlapArea_x;  T[2] = pointCenter_y;  T[3] = overlapArea_y;

                //左上の座標  upperleft = pointCenter + overlapArea;    //右下の座標  bottomright = pointCenter - overlapArea;
                Vector2 upperleft = new Vector2(T[0]+T[1], T[2]+T[3]);  Vector2 bottomright = new Vector2(T[0]-T[1], T[2]-T[3]);
                //当たり判定
                return Physics2D.OverlapArea(upperleft, bottomright, layermask);
		}
	}

    ///<summary>スポーン</summary>
	public class Spawn : MonoBehaviour {
        ///<summary>同じ場所にスポーンする</summary>
		public static GameObject Sameplace (GameObject Spawn,Transform transform){
			//同位置にスポーンする
            GameObject T = Instantiate(Spawn, transform.position, transform.rotation);
			//自身と同位置に移動                            //スポーンしたGameObjectを返す                           
            T.transform.localScale = transform.lossyScale;  return T;
		}
	}
}