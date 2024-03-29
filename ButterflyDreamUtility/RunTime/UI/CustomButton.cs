﻿using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ButterflyDreamUtility.UI
{
    /// <summary>
    /// 同時押しと連打の対策がされているボタン
    /// </summary>
    [UnityEngine.RequireComponent(typeof(Graphic))]
    public sealed class CustomButton : Selectable, IPointerClickHandler
    {
        /// <summary>
        /// ボタン押下時のクリック間隔（ミリ秒）
        /// </summary>
        private const int CLICK_INTERVAL_MS = 500;

        /// <summary>
        /// クリックされたかどうか（連打対策）
        /// </summary>
        private bool isClicked = false;

        /// <summary>
        /// destroyCancellationTokenのキャッシュ
        /// </summary>
        private CancellationToken cancellationToken;

        /// <summary>
        /// ボタン押下時に発火するイベント
        /// </summary>
        public event Action onClick = null;

        private readonly List<AsyncLazy> onClickAsyncList = new ();

        /// <summary>
        /// ボタン押下時に発火するイベント（非同期）
        /// <remarks>同時押し禁止・連打禁止に加えて登録イベントの処理終了も押下判定の再開に引っ掛ける</remarks>
        /// </summary>
        public event Func<CancellationToken, UniTask> onClickAsync
        {
            add => onClickAsyncList.Add(UniTask.Lazy(async () => await value(cancellationToken)));
            remove => onClickAsyncList.Remove(UniTask.Lazy(async () => await value(cancellationToken)));
        }

        /// <summary>
        /// 有効なCustomButtonを全て取得する
        /// </summary>
        public IEnumerable<CustomButton> allCustomButtons
        {
            get
            {
                List<CustomButton> result = new List<CustomButton>(s_SelectableCount);
                for (int i = 0; i < s_SelectableCount; i++)
                {
                    if (s_Selectables[i] is CustomButton
                        {
                            currentSelectionState: not SelectionState.Disabled
                        })
                    {
                        result.Add((CustomButton) s_Selectables[i]);
                    }
                }
                result.TrimExcess();
                return result;
            }
        }

        /// <summary>
        /// 既に押されているCustomButtonを全て取得する
        /// </summary>
        private IReadOnlyCollection<CustomButton> allOnClickCustomButtons
        {
            get
            {
                List<CustomButton> result = new List<CustomButton>(s_SelectableCount);
                for (int i = 0; i < s_SelectableCount; i++)
                {
                    if (s_Selectables[i] is CustomButton
                        {
                            currentSelectionState: not SelectionState.Disabled and not SelectionState.Normal
                        })
                    {
                        result.Add((CustomButton) s_Selectables[i]);
                    }
                }
                result.TrimExcess();
                return result;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            cancellationToken = destroyCancellationToken;
        }

        /// <inheritdoc/>
        public void OnPointerClick(PointerEventData eventData)
        {
            // 左クリック以外無視
            if (eventData.button != PointerEventData.InputButton.Left) return;

            // ボタンが無効なら無視
            if (!IsActive() || !IsInteractable()) return;

            // 連打・同時押し対策
            if (isClicked) return;

            // 登録されていた同期イベントの発火
            onClick?.Invoke();

            // 同時押し対策
            var customButtons = allOnClickCustomButtons;
            List<UniTask> tasks = new List<UniTask>(customButtons.Count + onClickAsyncList.Count);
            foreach (CustomButton btn in customButtons)
            {
                tasks.Add(UniTask.Defer(async () =>
                {
                    await UniTask.WaitUntil(() => btn.currentSelectionState == SelectionState.Normal, cancellationToken: cancellationToken);
                }));
            }

            // 登録されていた非同期イベントの登録
            foreach (AsyncLazy asyncLazy in onClickAsyncList)
            {
                tasks.Add(asyncLazy.Task);
            }

            UniTask.Void(async uniTasks =>
            {
                foreach (CustomButton customButton in allCustomButtons)
                {
                    customButton.isClicked = true;
                }
                // すべてのボタンの状態がNormalになるまで待機
                await UniTask.WhenAll(uniTasks);
                foreach (CustomButton customButton in allCustomButtons)
                {
                    if(customButton == this) continue;
                    customButton.isClicked = false;
                }
            }, tasks);

            // 連打対策
            UniTask.Void(async () =>
            {
                await UniTask.Delay(CLICK_INTERVAL_MS, cancellationToken: cancellationToken);
                isClicked = false;
            });
        }
    }
}