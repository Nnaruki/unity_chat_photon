using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;


public class PlayerManager : Photon.PunBehaviour, IPunObservable
{
    //頭上のUIのPrefab
    public GameObject PlayerUiPrefab;

    //現在のHP
    public int HP = 100;
    
    //Localのプレイヤーを設定
    public static GameObject LocalPlayerInstance;

    //頭上UIオブジェクト
    GameObject _uiGo;

    #region プレイヤー初期設定
    void Awake()
    {
        if (photonView.isMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }
    }
    #endregion

    #region 頭上UIの生成
    void Start()
    {
            if (PlayerUiPrefab != null) 
            {
                //Playerの頭上UIの生成とPlayerUIScriptでのSetTarget関数呼出
                _uiGo = Instantiate(PlayerUiPrefab) as GameObject;
                _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            }
            else
            {
                Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
            }
    }
    #endregion

    void Update()
    {
        if (!photonView.isMine) //このオブジェクトがLocalでなければ実行しない
        {
            return;
        }
        //LocalVariablesを参照し、現在のHPを更新
        HP = LocalVariables.currentHP;
    }

    #region OnPhotonSerializeView同期
    //プレイヤーのHP,チャットを同期
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) 
    {
        if (stream.isWriting)
        {
            stream.SendNext(this.HP);
            //stream.SendNext(this.ChatText);
        }
        else
        {
            this.HP = (int)stream.ReceiveNext();
            //this.ChatText = (string)stream.ReceiveNext();
        }
    }
    #endregion
}