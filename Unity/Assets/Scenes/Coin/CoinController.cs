/*
 *  コインの挙動について定義
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    /* パラメーター(半固定) */
	[Tooltip("Coinsの最大しきい値")]
    public int   cp_coinsLimit = 4;
	[Tooltip("Coin一個毎に上昇するピッチ"), Range(0f, 0.5f)]
    public float cp_pitchUP    = 0.125f;
	[Tooltip("上昇するピッチのぶれ（％）"), Range(0f, 0.5f)]
    public float cp_pitchShake = 0.25f;
	[Tooltip("寿命"), Range(0f, 5f)]
    public float cp_lifetime   = 2.5f;


    /* メインループ */
    void Start () {
		//Level内に存在するCoinを数える
		int coins_amount = GameObject.FindGameObjectsWithTag("Coin").Length;
		//最大しきい値におさまるよう調整
		coins_amount = ((coins_amount >= cp_coinsLimit) ? cp_coinsLimit:coins_amount);

		//Coin_getのピッチ変更
		AudioSource coin_get = gameObject.GetComponent<AudioSource>();
		float shake    = 1 + Random.Range(cp_pitchShake * -1f, cp_pitchShake); //ブレ
		float tone     = (coins_amount - 1) * cp_pitchUP;                      //階調
		coin_get.pitch = 1 + (tone * shake);

		//寿命が来たら破壊
		Destroy(gameObject, cp_lifetime);
	}
}
