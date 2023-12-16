using UnityEngine;

namespace ButterflyDreamUtility.Extensions
{
    public static class VectorExtension
    {
        /// <summary>
        /// 「lhs ＜ rhs」の判定
        /// </summary>
        /// <param name="lhs">左側の値</param>
        /// <param name="rhs">右側の値</param>
        /// <returns></returns>
        public static bool IsLessThan(this ref Vector3 lhs, ref Vector3 rhs)
        {
            return lhs.x < rhs.x && lhs.y < rhs.y && lhs.z < rhs.z;
        }

        /// <summary>
        /// 「lhs ＜ rhs」の判定
        /// </summary>
        /// <param name="lhs">左側の値</param>
        /// <param name="rhs">右側の値</param>
        /// <returns></returns>
        public static bool IsLessThan(this ref Vector3 lhs, Vector3 rhs)
        {
            return lhs.x < rhs.x && lhs.y < rhs.y && lhs.z < rhs.z;
        }

        /// <summary>
        /// 「lhs ＜ rhs」の判定
        /// </summary>
        /// <param name="lhs">左側の値</param>
        /// <param name="rhs">右側の値</param>
        /// <returns></returns>
        public static bool IsLessThan(this ref Vector2 lhs, ref Vector2 rhs)
        {
            return lhs.x < rhs.x && lhs.y < rhs.y;
        }

        /// <summary>
        /// 「lhs ＜ rhs」の判定
        /// </summary>
        /// <param name="lhs">左側の値</param>
        /// <param name="rhs">右側の値</param>
        /// <returns></returns>
        public static bool IsLessThan(this ref Vector2 lhs, Vector2 rhs)
        {
            return lhs.x < rhs.x && lhs.y < rhs.y;
        }

        /// <summary>
        /// 「lhs ＜＝ rhs」の判定
        /// </summary>
        /// <param name="lhs">左側の値</param>
        /// <param name="rhs">右側の値</param>
        /// <returns></returns>
        public static bool IsLessThanOrEqual(this ref Vector3 lhs, ref Vector3 rhs)
        {
            return lhs == rhs || (lhs.x < rhs.x && lhs.y < rhs.y && lhs.z < rhs.z);
        }

        /// <summary>
        /// 「lhs ＜＝ rhs」の判定
        /// </summary>
        /// <param name="lhs">左側の値</param>
        /// <param name="rhs">右側の値</param>
        /// <returns></returns>
        public static bool IsLessThanOrEqual(this ref Vector3 lhs, Vector3 rhs)
        {
            return lhs == rhs || (lhs.x < rhs.x && lhs.y < rhs.y && lhs.z < rhs.z);
        }

        /// <summary>
        /// 「lhs ＜＝ rhs」の判定
        /// </summary>
        /// <param name="lhs">左側の値</param>
        /// <param name="rhs">右側の値</param>
        /// <returns></returns>
        public static bool IsLessThanOrEqual(this ref Vector2 lhs, ref Vector2 rhs)
        {
            return lhs == rhs || (lhs.x < rhs.x && lhs.y < rhs.y);
        }

        /// <summary>
        /// 「lhs ＜＝ rhs」の判定
        /// </summary>
        /// <param name="lhs">左側の値</param>
        /// <param name="rhs">右側の値</param>
        /// <returns></returns>
        public static bool IsLessThanOrEqual(this ref Vector2 lhs, Vector2 rhs)
        {
            return lhs == rhs || (lhs.x < rhs.x && lhs.y < rhs.y);
        }

        /// <summary>
        /// 「lhs ＞ rhs」の判定
        /// </summary>
        /// <param name="lhs">右側の値</param>
        /// <param name="rhs">左側の値</param>
        /// <returns></returns>
        public static bool IsGreaterThan(this ref Vector3 lhs, ref Vector3 rhs)
        {
            return lhs.x > rhs.x && lhs.y > rhs.y && lhs.z > rhs.z;
        }

        /// <summary>
        /// 「lhs ＞ rhs」の判定
        /// </summary>
        /// <param name="lhs">右側の値</param>
        /// <param name="rhs">左側の値</param>
        /// <returns></returns>
        public static bool IsGreaterThan(this ref Vector3 lhs, Vector3 rhs)
        {
            return lhs.x > rhs.x && lhs.y > rhs.y && lhs.z > rhs.z;
        }

        /// <summary>
        /// 「lhs ＞ rhs」の判定
        /// </summary>
        /// <param name="lhs">右側の値</param>
        /// <param name="rhs">左側の値</param>
        /// <returns></returns>
        public static bool IsGreaterThan(this ref Vector2 lhs, ref Vector2 rhs)
        {
            return lhs.x > rhs.x && lhs.y > rhs.y;
        }

        /// <summary>
        /// 「lhs ＞ rhs」の判定
        /// </summary>
        /// <param name="lhs">右側の値</param>
        /// <param name="rhs">左側の値</param>
        /// <returns></returns>
        public static bool IsGreaterThan(this ref Vector2 lhs, Vector2 rhs)
        {
            return lhs.x > rhs.x && lhs.y > rhs.y;
        }

        /// <summary>
        /// 「lhs ＞＝ rhs」の判定
        /// </summary>
        /// <param name="lhs">右側の値</param>
        /// <param name="rhs">左側の値</param>
        /// <returns></returns>
        public static bool IsGreaterThanOrEqual(this ref Vector3 lhs, ref Vector3 rhs)
        {
            return lhs == rhs || (lhs.x > rhs.x && lhs.y > rhs.y && lhs.z > rhs.z);
        }

        /// <summary>
        /// 「lhs ＞＝ rhs」の判定
        /// </summary>
        /// <param name="lhs">右側の値</param>
        /// <param name="rhs">左側の値</param>
        /// <returns></returns>
        public static bool IsGreaterThanOrEqual(this ref Vector3 lhs, Vector3 rhs)
        {
            return lhs == rhs || (lhs.x > rhs.x && lhs.y > rhs.y && lhs.z > rhs.z);
        }

        /// <summary>
        /// 「lhs ＞＝ rhs」の判定
        /// </summary>
        /// <param name="lhs">右側の値</param>
        /// <param name="rhs">左側の値</param>
        /// <returns></returns>
        public static bool IsGreaterThanOrEqual(this ref Vector2 lhs, ref Vector2 rhs)
        {
            return lhs == rhs || (lhs.x > rhs.x && lhs.y > rhs.y);
        }

        /// <summary>
        /// 「lhs ＞＝ rhs」の判定
        /// </summary>
        /// <param name="lhs">右側の値</param>
        /// <param name="rhs">左側の値</param>
        /// <returns></returns>
        public static bool IsGreaterThanOrEqual(this ref Vector2 lhs, Vector2 rhs)
        {
            return lhs == rhs || (lhs.x > rhs.x && lhs.y > rhs.y);
        }

        /// <summary>
        /// 指定した範囲内に収めた値のコピーを返す
        /// </summary>
        /// <param name="value">対象の値</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <returns></returns>
        public static Vector3 Clamp(ref Vector3 value, ref Vector3 min, ref Vector3 max)
        {
            return new Vector3(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y), Mathf.Clamp(value.z, min.z, max.z));
        }

        /// <summary>
        /// 指定した範囲内に収めた値のコピーを返す
        /// </summary>
        /// <param name="value">対象の値</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <returns></returns>
        public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
        {
            return new Vector3(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y), Mathf.Clamp(value.z, min.z, max.z));
        }

        /// <summary>
        /// 指定した範囲内に収めた値のコピーを返す
        /// </summary>
        /// <param name="value">対象の値</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <returns></returns>
        public static Vector2 Clamp(ref Vector2 value, ref Vector2 min, ref Vector2 max)
        {
            return new Vector2(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y));
        }

        /// <summary>
        /// 指定した範囲内に収めた値のコピーを返す
        /// </summary>
        /// <param name="value">対象の値</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <returns></returns>
        public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max)
        {
            return new Vector2(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y));
        }
    }
}