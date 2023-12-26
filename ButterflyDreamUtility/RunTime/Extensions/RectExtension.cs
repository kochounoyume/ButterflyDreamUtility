using UnityEngine;

namespace ButterflyDreamUtility.Extensions
{
    public static class RectExtension
    {
        /// <summary>
        /// 重なっている部分の割合を返す
        /// </summary>
        /// <param name="target">基準となるRect</param>
        /// <param name="other">重なっているRect</param>
        /// <returns>重なっている部分の割合</returns>
        public static float OverlapsAreaRatio(this Rect target, Rect other)
        {
            if (!target.Overlaps(other)) return default;
            float xMin = Mathf.Max(target.xMin, other.xMin);
            float xMax = Mathf.Min(target.xMax, other.xMax);
            float yMin = Mathf.Max(target.yMin, other.yMin);
            float yMax = Mathf.Min(target.yMax, other.yMax);
            float result = (xMax - xMin) * (yMax - yMin) / target.width * target.height;
            return Mathf.Approximately(result, 1.0f) ? 1.0f : result;
        }
    }
}