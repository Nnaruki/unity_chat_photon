using UnityEngine;
using UnityEngine.UI;

using System.Collections;

    public class PlayerUIScript : MonoBehaviour
    {

        #region Public Properties

        //キャラの頭上に乗るように調整するためのOffset
        public Vector3 ScreenOffset = new Vector3(0f, 50f, 0f);

        //プレイヤー名前設定用Text
        public Text PlayerNameText;

        //プレイヤーのHP用Slider
        public Slider PlayerHPSlider;

        //プレイヤーのチャット用Text
        //public Text PlayerChatText;

        #endregion

        #region Private Properties
        //追従するキャラのPlayerManager情報
        PlayerManager _target;
        float _characterControllerHeight;
        Transform _targetTransform;
        Vector3 _targetPosition;
        #endregion

        #region MonoBehaviour Messages

        void Awake()
        {
            //このオブジェクトはCanvasオブジェクトの子オブジェクトとして生成
            this.GetComponent<Transform>().SetParent(GameObject.Find("Canvas").GetComponent<Transform>());
        }

        void Update()
        {
            //もしPlayerがいなくなったらこのオブジェクトも削除
            if (_target == null)
            {
                Destroy(this.gameObject);
                return;
            }

            // 現在のHPをSliderに適用
            if (PlayerHPSlider != null)
            {
                PlayerHPSlider.value = _target.HP;
            }

            // 頭上チャットを表示
            //if (PlayerChatText != null)
            //{
            //    PlayerChatText.text = _target.ChatText;
            //}

        }

        #endregion

        void LateUpdate()
        {
            //targetのオブジェクトを追跡する
            if (_targetTransform != null)
            {
                _targetPosition = _targetTransform.position;    //三次元空間上のtargetの座標を得る
                _targetPosition.y += _characterControllerHeight;  //キャラクターの背の高さを考慮する
                //targetの座標から頭上UIの画面上の二次元座標を計算して移動させる
                this.transform.position = Camera.main.WorldToScreenPoint(_targetPosition) + ScreenOffset;   
            }
        }

        #region Public Methods
        public void SetTarget(PlayerManager target)
        {
            if (target == null)//targetがいなければエラーをConsoleに表示
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }
            //targetの情報をこのスクリプト内で使うのでコピー
            _target = target;
            _targetTransform = _target.GetComponent<Transform>();

            CharacterController _characterController = _target.GetComponent<CharacterController>();
            
            //PlayerManagerの頭上UIに表示したいデータをコピー
            if (_characterController != null)
            {
                _characterControllerHeight = _characterController.height;
            }

            if (PlayerNameText != null)
            {
                PlayerNameText.text = _target.photonView.owner.NickName;
            }
            if (PlayerHPSlider != null)
            {
                PlayerHPSlider.value = _target.HP;
            }
            //if (PlayerChatText != null)
            //{
            //    PlayerChatText.text = _target.ChatText;
            //}
        }
        #endregion
}