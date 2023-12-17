using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ButterflyDreamUtility.UI
{
    using Constants;
    using Extensions;

    [RequireComponent(typeof(RectTransform))]
    public class SimpleScrollView : UIBehaviour, IBeginDragHandler, IDragHandler
    {
        /// <summary>
        /// スクロール方向
        /// </summary>
        private enum ScrollDirection
        {
            /// <summary> 横スクロール </summary>
            Horizontal,
            /// <summary> 縦スクロール </summary>
            Vertical,
            /// <summary> 両方向スクロール </summary>
            Both,
            /// <summary> どちらもスクロールしない </summary>
            None
        }

        [SerializeField, Tooltip("このScrollViewのRectTransform")]
        private RectTransform m_rectTransform = null;

        /// <summary>
        /// このScrollViewのRectTransform
        /// </summary>
        public RectTransform rectTransform
        {
            get
            {
                m_rectTransform ??= transform as RectTransform;
                return m_rectTransform;
            }
        }

        [SerializeField, Tooltip("スクロール内容の表示範囲を表すRectTransform")]
        private RectTransform m_viewport = null;

        /// <summary>
        /// スクロール内容の表示範囲を表すRectTransform
        /// </summary>
        public RectTransform viewport => m_viewport;

        /// <summary>
        /// スクロールする方向
        /// </summary>
        private ScrollDirection scrollDirection = ScrollDirection.Vertical;

        /// <summary>
        /// 縦・横スクロールの際のスクロール時のイベント
        /// <remarks>
        /// 値は割合で、ScrollDirection.Bothの場合はX・Yの2つの割合値を返す
        /// </remarks>
        /// </summary>
        public event Action<float, float> onValueChanged = null;

        /// <summary>
        /// ドラッグを開始した時のタッチ位置
        /// </summary>
        private Vector2 pointerStartLocalPoint = Vector2.zero;

        /// <summary>
        /// ドラッグを開始した時のviewportの位置
        /// </summary>
        private Vector2 viewPortStartPos = Vector2.zero;

        /// <summary>
        /// RectTransformのトラッカー
        /// </summary>
        private DrivenRectTransformTracker tracker = new DrivenRectTransformTracker();

        /// <summary>
        /// スクロールビューの実際の表示範囲のRect
        /// </summary>
        private Rect realRect => rectTransform.rect;

        /// <summary>
        /// スクロールビューの実際の表示範囲サイズ
        /// </summary>
        public Vector2 realSizeDelta => rectTransform.rect.size;

        private Vector2 m_virtualSizeDelta = Vector2.zero;

        /// <summary>
        /// スクロールする仮想領域のサイズ
        /// </summary>
        public Vector2 virtualSizeDelta
        {
            get
            {
                if(m_virtualSizeDelta.IsLessThanOrEqual(Vector2.zero))
                {
                    m_virtualSizeDelta = viewport.rect.size;
                }
                return m_virtualSizeDelta;
            }
            set => SetVirtualSizeDelta(ref value);
        }

        /// <summary>
        /// スクロールする仮想領域のサイズの設定
        /// </summary>
        /// <param name="virtualSize">任意のサイズ</param>
        public void SetVirtualSizeDelta(ref Vector2 virtualSize)
        {
            if (virtualSize.IsLessThanOrEqual(realSizeDelta))
            {
                scrollDirection = ScrollDirection.None;
            }
            else if(Mathf.Approximately(virtualSize.x, realRect.width))
            {
                scrollDirection = ScrollDirection.Vertical;
            }
            else if (Mathf.Approximately(virtualSize.y, realRect.height))
            {
                scrollDirection = ScrollDirection.Horizontal;
            }
            else
            {
                scrollDirection = ScrollDirection.Both;
            }
            tracker.Clear();
            viewport.sizeDelta = virtualSize;
            tracker.Add(this, viewport, ConstantDrivenTransformProperties.ExceptXYPos);
        }

        // ドラッグが開始される前にBaseInputModuleによって呼び出される
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            // 左クリック以外は無視
            if (eventData.button != PointerEventData.InputButton.Left) return;
            if (!IsActive()) return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform, eventData.position, eventData.pressEventCamera, out pointerStartLocalPoint);
            viewPortStartPos = viewport.anchoredPosition;
        }

        // ドラッグが発生しているとき、カーソルが移動するたびに呼び出される
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            // 左クリック以外は無視
            if (eventData.button != PointerEventData.InputButton.Left) return;
            if (!IsActive()) return;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
            {
                return;
            }

            Vector2 pointerDelta = localPoint - pointerStartLocalPoint;
            // MEMO:Vector系の等価評価は浮動小数点の誤差が考慮されている
            if (pointerDelta == Vector2.zero) return;

            float posX = default;
            float posY = default;
            Vector2 anchoredPos;
            switch (scrollDirection)
            {
                case ScrollDirection.Horizontal:
                    float limitWidth = (virtualSizeDelta.x - realRect.width) / 2;
                    posX = Mathf.Clamp(viewPortStartPos.x + pointerDelta.x, -limitWidth, limitWidth);
                    anchoredPos = new Vector2(posX, viewPortStartPos.y);
                    break;
                case ScrollDirection.Vertical:
                    float limitHeight = (virtualSizeDelta.y - realRect.height) / 2;
                    posY = Mathf.Clamp(viewPortStartPos.y + pointerDelta.y, -limitHeight, limitHeight);
                    anchoredPos = new Vector2(viewPortStartPos.x, posY);
                    break;
                case ScrollDirection.Both:
                    Vector2 limitSize = (virtualSizeDelta - realSizeDelta) / 2;
                    Vector2 pos = viewPortStartPos + pointerDelta;
                    posX = pos.x;
                    posY = pos.y;
                    anchoredPos = VectorExtension.Clamp(ref pos, -limitSize, ref limitSize);
                    break;
                default:
                    const string message = "スクロール領域がありません";
                    UnityEngine.Debug.LogWarning(message);
                    return;
            }
            viewport.anchoredPosition = anchoredPos;
            onValueChanged?.Invoke(posX, posY);
        }
    }
}