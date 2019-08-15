﻿//ブロックの生成について定義。
using UnityEngine;
using System.Collections.Generic;
using Level;
using UniRx;
using UniRx.Triggers;


public class Level_SPAWN : MonoBehaviour{
    [SerializeField] private GameInstance Header = default;
    private float delda_spawn = 0; //積算デルタ秒
    Queue<Vector2Int> memory = new Queue<Vector2Int>();
    bool[] T = new bool[6];  //先に条件式を計算しておく
    float span = 0; //スポーンさせる間隔


    /*
    public void Update()
    {
        float span = 0; //スポーンさせる間隔
        this.delda_spawn += Time.deltaTime; //デルタ秒を更新
        //int blocks = GameObject.FindGameObjectsWithTag("Block").Length; //Level内に存在するBlockを数える
        
        /*
        bool[] T =new bool[6];  //先に条件式を計算しておく
        T[0] =                             0 <= blocks;    T[1] =  blocks < this.Header.threshold_spawn.x;
        T[2] = this.Header.threshold_spawn.x <= blocks;    T[3] =  blocks < this.Header.threshold_spawn.y;
        T[4] = this.Header.threshold_spawn.y <= blocks;    T[5] =  blocks < this.Header.threshold_spawn.z;
            

        //Blockの数に合わせて、スポーン間隔を調整
        if      ( T[0] && T[1] ) { span = this.Header.span_spawn.x; }
        else if ( T[2] && T[3] ) { span = this.Header.span_spawn.y; }
        else if ( T[4] && T[5] ) { span = this.Header.span_spawn.z; }
        else                     { span = Mathf.Infinity; }
        
        if (this.delda_spawn > span) //スポーン出来るだけの間隔が空いたら
        {
            this.delda_spawn = 0; //積算デルタ秒を初期化

            int x = UnityEngine.Random.Range(this.Header.range_spawn.x/(-2), this.Header.range_spawn.x/(2))*2;
            int y = UnityEngine.Random.Range(0, this.Header.range_spawn.y+1)*2;
            Vector2Int z = new Vector2Int(x,y);

            if (memory.Contains(z)) //スポーン履歴を確認
            {
                while (memory.Count > this.Header.memory_spawn) { memory.Dequeue(); } //上限を超えているなら削除
            }
            else
            {
                memory.Enqueue(z); //履歴に記録
                GameObject item = Instantiate(this.Header.spawntarget) as GameObject; //スポーン
                item.transform.position = new Vector3(x, y, 0); //位置調整
            }
        }
    }
    */

    public void Start()
    {
        int blocks = GameObject.FindGameObjectsWithTag("Block").Length;
        Subject<bool> boolsubject = new Subject<bool>();
        var disposable1 = boolsubject.Subscribe(x => {
            T[0] = 0 <= blocks; T[1] = blocks < this.Header.threshold_spawn.x;
            T[2] = this.Header.threshold_spawn.x <= blocks; T[3] = blocks < this.Header.threshold_spawn.y;
            T[4] = this.Header.threshold_spawn.y <= blocks; T[5] = blocks < this.Header.threshold_spawn.z;
        });
        var disposable2 = boolsubject.Subscribe(x => {
            if (T[0] && T[1]) { span = this.Header.span_spawn.x; }
            else if (T[2] && T[3]) { span = this.Header.span_spawn.y; }
            else if (T[4] && T[5]) { span = this.Header.span_spawn.z; }
            else { span = Mathf.Infinity; }
        });
        var disposable3 = boolsubject.Subscribe(x => {
            Debug.Log("D"); 
        });

        this.FixedUpdateAsObservable()
            .Subscribe(_ => Debug.Log("Update!"));
        Observable.EveryUpdate()
                  .Subscribe(_ => { Debug.Log("D"); })
                  .AddTo(this.gameObject);
    }
}