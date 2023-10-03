using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ButterflyDreamUtility.UI
{
    /// <summary>
    /// 同時押しと連打の対策がされているボタン
    /// </summary>
    [RequireComponent(typeof(Graphic))]
    public sealed class CustomButton : Selectable, IPointerClickHandler
    {
        /// <summary>
        /// ボタン押下時に発火するイベント
        /// </summary>
        public event Action onClick = null;

        /// <summary>
        /// ボタン押下時に発火するイベント（非同期で待機させることで排他制御する）
        /// <remarks>
        /// 引数にキャンセルトークンを渡して呼び出す
        /// </remarks>
        /// </summary>
        public event Func<CancellationToken, UniTask> onClickLockAsync = null;

        /// <summary>
        /// クリックされたかどうか（連打対策のためにクリックされてからしばらくtrueのまま）
        /// </summary>
        private bool isClicked = false;

        /// <summary>
        /// 同一ボタンを二本以上の指で押している際の該当接触数（そんなに押さないで...）
        /// </summary>
        private uint touchCount = default;

        /// <summary>
        /// 何かしら状態が変わっているボタン群
        /// </summary>
        private static readonly List<CustomButton> activeStateButtons = new List<CustomButton>(10);

        /// <inheritdoc/>
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            touchCount++;
            activeStateButtons.Add(this);
        }

        /// <inheritdoc/>
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            touchCount--;
            activeStateButtons.Remove(this);
        }

        /// <inheritdoc/>
        public void OnPointerClick(PointerEventData eventData)
        {
            // 左クリック以外無視
            if (eventData.button != PointerEventData.InputButton.Left) return;

            // ボタンが無効なら無視
            if (!IsActive() || !IsInteractable()) return;

            // アクティブなボタンに登録がないなら無視（同時押し対策）& 接触数が二点以上なら無視（同一ボタン重複押し対策）
            if(!activeStateButtons.Contains(this) || touchCount > 1) return;

            // 同時に押していた他のボタン群の状態をクリア
            foreach (CustomButton activeStateButton in activeStateButtons)
            {
                if (activeStateButton == this) continue;
                // 演出状態のリセット
                activeStateButton.InstantClearState();
            }
            activeStateButtons.RemoveAll(btn => btn != this);

            // 連打対策
            if (isClicked) return;
            isClicked = true;
            UniTask.Void(async token =>
            {
                // 厳密に排他制御する場合
                if (onClickLockAsync != null)
                {
                    // 登録されていた非同期イベントの発火・待機
                    await onClickLockAsync(token);
                }
                else  // 適当に連打対策したい場合
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(colors.fadeDuration), cancellationToken: token);
                }
                isClicked = false;
            }, destroyCancellationToken);

            // 登録されていた同期イベントの発火
            onClick?.Invoke();
        }
    }
}