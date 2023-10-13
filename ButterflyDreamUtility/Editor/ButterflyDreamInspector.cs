using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ButterflyDreamUtility.Editor
{
    using Attribute;

    /// <summary>
    /// デフォルトのインスペクタービュー
    /// </summary>
    [CanEditMultipleObjects, CustomEditor(typeof(UnityEngine.Object), true)]
    public class ButterflyDreamInspector : UnityEditor.Editor
    {
        /// <summary>
        /// メソッド情報と属性情報のタプル配列
        /// </summary>
        private (MethodInfo methodInfo, ButtonAttribute attr)[] methodAttrInfos = Array.Empty<(MethodInfo, ButtonAttribute)>();

        private void OnEnable()
        {
            // target内のメソッドを全検索してButton属性を持つものを抽出保存
            foreach (MethodInfo methodInfo in target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var attr = methodInfo.GetCustomAttribute<ButtonAttribute>();
                if (attr == null) continue;
                attr.ButtonName = methodInfo.Name;
                ArrayUtility.Add(ref methodAttrInfos, (methodInfo, attr));
            }
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            foreach (var methodAttrInfo in methodAttrInfos)
            {
                if (GUILayout.Button(methodAttrInfo.attr.ButtonName))
                {
                    try
                    {
                        methodAttrInfo.methodInfo.Invoke(target, methodAttrInfo.attr.parameters);
                    }
                    catch (TargetParameterCountException)
                    {
                        UnityEngine.Debug.LogError("引数の数の相違によるエラーです");
                    }
                    catch (ArgumentException)
                    {
                        UnityEngine.Debug.LogError("引数の型の相違によるエラーです");
                    }
                }
            }
        }
    }
}