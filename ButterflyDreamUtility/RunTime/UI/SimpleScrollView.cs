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
        public enum ScrollAxis
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
        public virtual ScrollAxis scrollAxis { get; protected set; } = ScrollAxis.Vertical;

        /// <summary>
        /// ゲーム開始時、縦・横スクロールの際のスクロール時のイベントを発火するかどうか
        /// </summary>
        public bool isInitializeEvent { get; set; } = true;

        protected event Action<Rect> onValueChanged = null;

        /// <summary>
        /// 縦・横スクロールの際のスクロール時のイベント
        /// <remarks>
        /// 値はスクロール仮想領域の座標基準における、実際の表示範囲サイズの領域
        /// </remarks>
        /// </summary>
        public event Action<Rect> OnValueChanged
        {
            add
            {
                onValueChanged = null;
                // 登録時に初回発火
                if (isInitializeEvent)
                {
                    value?.Invoke(new Rect(Vector2.zero, realSizeDelta){ center = viewport.anchoredPosition });
                }
                onValueChanged += value;
            }
            remove => onValueChanged -= value;
        }

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
        protected DrivenRectTransformTracker tracker = new DrivenRectTransformTracker();

        /// <summary>
        /// スクロールビューの実際の表示範囲のRect
        /// </summary>
        protected Rect realRect => rectTransform.rect;

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
            protected set => m_virtualSizeDelta = value;
        }

        /// <summary>
        /// スクロールする仮想領域のサイズの設定
        /// </summary>
        /// <param name="virtualSizeX">任意のxサイズ</param>
        /// <param name="virtualSizeY">任意のyサイズ</param>
        public void SetVirtualSizeDelta(in float virtualSizeX, in float virtualSizeY)
        {
            SetVirtualSizeDelta(new Vector2(virtualSizeX, virtualSizeY));
        }

        /// <summary>
        /// スクロールする仮想領域のサイズの設定
        /// </summary>
        /// <param name="virtualSize">任意のサイズ</param>
        public void SetVirtualSizeDelta(Vector2 virtualSize)
        {
            virtualSizeDelta = virtualSize;
            if (virtualSize.IsLessThanOrEqual(realSizeDelta))
            {
                scrollAxis = ScrollAxis.None;
                viewport.anchoredPosition = Vector2.zero;
            }
            else if(Mathf.Approximately(virtualSize.x, realRect.width))
            {
                scrollAxis = ScrollAxis.Vertical;
                viewport.anchoredPosition = new Vector2(0, (- virtualSize.y + realRect.height) / 2);
            }
            else if (Mathf.Approximately(virtualSize.y, realRect.height))
            {
                scrollAxis = ScrollAxis.Horizontal;
                viewport.anchoredPosition = new Vector2((-virtualSize.x + realRect.width) / 2, 0);
            }
            else
            {
                scrollAxis = ScrollAxis.Both;
            }
            tracker.Clear();
            viewport.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, virtualSize.x);
            viewport.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, virtualSize.y);
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

            Vector2 anchoredPos;
            switch (scrollAxis)
            {
                case ScrollAxis.Horizontal:
                    float limitWidth = (virtualSizeDelta.x - realRect.width) / 2;
                    float posX = Mathf.Clamp(viewPortStartPos.x + pointerDelta.x, -limitWidth, limitWidth);
                    anchoredPos = new Vector2(posX, viewPortStartPos.y);
                    break;
                case ScrollAxis.Vertical:
                    float limitHeight = (virtualSizeDelta.y - realRect.height) / 2;
                    float posY = Mathf.Clamp(viewPortStartPos.y + pointerDelta.y, -limitHeight, limitHeight);
                    anchoredPos = new Vector2(viewPortStartPos.x, posY);
                    break;
                case ScrollAxis.Both:
                    Vector2 limitSize = (virtualSizeDelta - realSizeDelta) / 2;
                    anchoredPos = VectorExtension.Clamp(viewPortStartPos + pointerDelta, -limitSize, limitSize);
                    break;
                default:
                    const string message = "スクロール領域がありません";
                    UnityEngine.Debug.LogWarning(message);
                    return;
            }
            viewport.anchoredPosition = anchoredPos;
            onValueChanged?.Invoke(new Rect(Vector2.zero, realSizeDelta){ center = anchoredPos });
        }

        /// <summary>
        /// スクロールする関係上、親子の座標系が反転するので、親のRectを子の座標系のRectに変換する
        /// </summary>
        /// <param name="rect">対象の親のRect</param>
        public void ConvertRectInChildRange(ref Rect rect) => rect.center = -rect.center;
    }
}