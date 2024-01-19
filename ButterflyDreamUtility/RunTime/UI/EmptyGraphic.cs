using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ButterflyDreamUtility.UI
{
    /// <summary>
    /// 空処理のグラフィック
    /// <remarks>
    /// 想定用途：マスク時のベース, 画面全体を透明なuGUIで覆いたいとき、Buttonの判定領域を透明にしたいとき
    /// </remarks>
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class EmptyGraphic : Graphic
    {
        /// <summary>
        /// 子オブジェクト以下のGraphic（自分は含まない）のCanvasRenderer
        /// </summary>
        private Graphic[] childGraphics = System.Array.Empty<Graphic>();

        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();
            List<Graphic> childGraphicList = new List<Graphic>(transform.childCount);
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out Graphic graphic))
                {
                    childGraphicList.Add(graphic);
                }
            }
            childGraphicList.TrimExcess();
            if(childGraphicList.Count > 0)
            {
                childGraphics = childGraphicList.ToArray();
            }
            childGraphicList.Clear();
        }

        // Selectableの押下時の色変化はここを呼び出しているみたいなので、ちょっと処理を上書き
        public override void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
        {
            foreach (Graphic childGraphic in childGraphics)
            {
                childGraphic.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
            }
        }

        /// <inheritdoc/>
        public override void SetMaterialDirty()
        {
            // 空処理を上書き
        }

        /// <inheritdoc/>
        public override void SetVerticesDirty()
        {
            // 空処理を上書き
        }

        /// <inheritdoc/>
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
}