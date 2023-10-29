using System.Collections.Generic;
using UnityEngine;

namespace ButterflyDreamUtility.Extensions
{
    public static class ComponentExtension
    {
        /// <summary>
        /// 子オブジェクト以下のComponentを取得する
        /// </summary>
        /// <param name="target">対象のゲームオブジェクト</param>
        /// <param name="includeInactive">非アクティブのオブジェクトのコンポーネントも含めるかどうか</param>
        /// <param name="includeRoot">対象のルートゲームオブジェクトのコンポーネントも含めるかどうか</param>
        /// <typeparam name="T">取得するコンポーネントの型</typeparam>
        /// <returns>子オブジェクト以下の任意のComponent群</returns>
        public static T[] GetComponentsInChildren<T>(this Component target, bool includeInactive, bool includeRoot) where T : Component
        {
            if (includeRoot) return target.GetComponentsInChildren<T>(includeInactive);
            List<T> components = new List<T>();
            target.GetComponentsInChildren(includeInactive, components);
            foreach (T targetComponent in target.GetComponents<T>())
            {
                if (components.Contains(targetComponent))
                {
                    components.Remove(targetComponent);
                }
            }
            return components.ToArray();
        }

        /// <summary>
        /// 子オブジェクトのみのComponentを取得する（親も孫も含めない）
        /// </summary>
        /// <param name="target">対象のゲームオブジェクト</param>
        /// <param name="includeInactive">非アクティブのオブジェクトのコンポーネントも含めるかどうか</param>
        /// <typeparam name="T">取得するコンポーネントの型</typeparam>
        /// <returns>子オブジェクトのみの任意のComponent群</returns>
        public static T[] GetComponentsInOnlyChildren<T>(this Component target, bool includeInactive = false) where T : Component
        {
            List<T> components = new List<T>();
            Transform parent = target.transform;
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if(includeInactive && !child.gameObject.activeSelf) continue;
                if(child.TryGetComponent(out T component))
                {
                    components.Add(component);
                }
            }
            return components.ToArray();
        }
    }
}