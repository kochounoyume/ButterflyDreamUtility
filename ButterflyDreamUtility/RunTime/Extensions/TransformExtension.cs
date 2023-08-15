using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace ButterflyDreamUtility.Extensions
{
    using UniTaskTween;
    
    public static class TransformExtension
    {
        private static Dictionary<int, TweenRunner<VectorTween>> moveTweenRunnerTable = null;
        
        /// <summary>
        /// Transformの座標移動をトゥイーンする
        /// </summary>
        /// <param name="target">Transform</param>
        /// <param name="endValue">トゥイーン後の値</param>
        /// <param name="duration">トゥイーン時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="callback">トゥイーン完了後のコールバック</param>
        public static void Move2DTween(this Transform target, Vector3 endValue, float duration, bool isIgnoreTimeScale = false, UnityAction<Transform> callback = null)
        {
            TweenDataSet<VectorTween> dataSet = SetMoveTween(target, endValue, duration, VectorTweenMode.XY, isIgnoreTimeScale, callback);
            if(dataSet.Equals(default)) return;
            moveTweenRunnerTable[dataSet.instanceId].StartTween(dataSet.tweenValue);
        }
        
        /// <summary>
        /// Transformの座標移動を非同期でトゥイーンするUniTaskを返す
        /// </summary>
        /// <param name="target">Transform</param>
        /// <param name="endValue">トゥイーン後の値</param>
        /// <param name="duration">トゥイーン時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="token">キャンセルトークン（敢えて指定しなくても、AudioSource破壊時にキャンセルはされる）</param>
        /// <returns>Transformのy座標移動をトゥイーンする非同期処理</returns>
        public static UniTask Move2DTweenAsync(this Transform target, Vector3 endValue, float duration, bool isIgnoreTimeScale = false, CancellationToken token = default)
        {
            TweenDataSet<VectorTween> dataSet = SetMoveTween(target, endValue, duration, VectorTweenMode.XY, isIgnoreTimeScale);
            return dataSet.Equals(default) ? default : moveTweenRunnerTable[dataSet.instanceId].StartTweenAsync(dataSet.tweenValue, token);
        }
        
        /// <summary>
        /// Transformの座標移動を加算トゥイーンする
        /// </summary>
        /// <param name="target">Transform</param>
        /// <param name="addValue">トゥイーンで加算する分の値</param>
        /// <param name="duration">トゥイーン時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="callback">トゥイーン完了後のコールバック</param>
        public static void AddTween(this Transform target, Vector3 addValue, float duration, bool isIgnoreTimeScale = false, UnityAction<Transform> callback = null)
        {
            Vector3 endPosition = target.position + addValue;
            TweenDataSet<VectorTween> dataSet = SetMoveTween(target, endPosition, duration, VectorTweenMode.XY, isIgnoreTimeScale, callback);
            if(dataSet.Equals(default)) return;
            moveTweenRunnerTable[dataSet.instanceId].StartTween(dataSet.tweenValue);
        }
        
        /// <summary>
        /// Transformの座標移動を非同期で加算トゥイーンするUniTaskを返す
        /// </summary>
        /// <param name="target">Transform</param>
        /// <param name="addValue">トゥイーンで加算する分の値</param>
        /// <param name="duration">トゥイーン時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="token">キャンセルトークン（敢えて指定しなくても、AudioSource破壊時にキャンセルはされる）</param>
        /// <returns>Transformのy座標移動をトゥイーンする非同期処理</returns>
        public static UniTask AddTweenAsync(this Transform target, Vector3 addValue, float duration, bool isIgnoreTimeScale = false, CancellationToken token = default)
        {
            Vector3 endPosition = target.position + addValue;
            TweenDataSet<VectorTween> dataSet = SetMoveTween(target, endPosition, duration, VectorTweenMode.XY, isIgnoreTimeScale);
            return dataSet.Equals(default) ? default : moveTweenRunnerTable[dataSet.instanceId].StartTweenAsync(dataSet.tweenValue, token);
        }
        
        /// <summary>
        /// Transformの2D座標移動をトゥイーンする
        /// </summary>
        /// <param name="target">Transform</param>
        /// <param name="endValue">トゥイーン後の値</param>
        /// <param name="duration">トゥイーン時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="callback">トゥイーン完了後のコールバック</param>
        public static void Move2DTween(this Transform target, Vector2 endValue, float duration, bool isIgnoreTimeScale = false, UnityAction<Transform> callback = null)
        {
            TweenDataSet<VectorTween> dataSet = SetMoveTween(target, endValue, duration, VectorTweenMode.XY, isIgnoreTimeScale, callback);
            if(dataSet.Equals(default)) return;
            moveTweenRunnerTable[dataSet.instanceId].StartTween(dataSet.tweenValue);
        }
        
        /// <summary>
        /// Transformの2D座標移動を非同期でトゥイーンするUniTaskを返す
        /// </summary>
        /// <param name="target">Transform</param>
        /// <param name="endValue">トゥイーン後の値</param>
        /// <param name="duration">トゥイーン時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="token">キャンセルトークン（敢えて指定しなくても、AudioSource破壊時にキャンセルはされる）</param>
        /// <returns>Transformのy座標移動をトゥイーンする非同期処理</returns>
        public static UniTask Move2DTweenAsync(this Transform target, Vector2 endValue, float duration, bool isIgnoreTimeScale = false, CancellationToken token = default)
        {
            TweenDataSet<VectorTween> dataSet = SetMoveTween(target, endValue, duration, VectorTweenMode.XY, isIgnoreTimeScale);
            return dataSet.Equals(default) ? default : moveTweenRunnerTable[dataSet.instanceId].StartTweenAsync(dataSet.tweenValue, token);
        }
        
        /// <summary>
        /// Transformの2D座標移動を加算トゥイーンする
        /// </summary>
        /// <param name="target">Transform</param>
        /// <param name="addValue">トゥイーンで加算する分の値</param>
        /// <param name="duration">トゥイーン時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="callback">トゥイーン完了後のコールバック</param>
        public static void Add2DTween(this Transform target, Vector2 addValue, float duration, bool isIgnoreTimeScale = false, UnityAction<Transform> callback = null)
        {
            Vector3 endPosition = target.position + (Vector3)addValue;
            TweenDataSet<VectorTween> dataSet = SetMoveTween(target, endPosition, duration, VectorTweenMode.XY, isIgnoreTimeScale, callback);
            if(dataSet.Equals(default)) return;
            moveTweenRunnerTable[dataSet.instanceId].StartTween(dataSet.tweenValue);
        }
        
        /// <summary>
        /// Transformの2D座標移動を非同期で加算トゥイーンするUniTaskを返す
        /// </summary>
        /// <param name="target">Transform</param>
        /// <param name="addValue">トゥイーンで加算する分の値</param>
        /// <param name="duration">トゥイーン時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="token">キャンセルトークン（敢えて指定しなくても、AudioSource破壊時にキャンセルはされる）</param>
        /// <returns>Transformのy座標移動をトゥイーンする非同期処理</returns>
        public static UniTask Add2DTweenAsync(this Transform target, Vector2 addValue, float duration, bool isIgnoreTimeScale = false, CancellationToken token = default)
        {
            Vector3 endPosition = target.position + (Vector3)addValue;
            TweenDataSet<VectorTween> dataSet = SetMoveTween(target, endPosition, duration, VectorTweenMode.XY, isIgnoreTimeScale);
            return dataSet.Equals(default) ? default : moveTweenRunnerTable[dataSet.instanceId].StartTweenAsync(dataSet.tweenValue, token);
        }
        
        /// <summary>
        /// Transformのx座標移動をトゥイーンする
        /// </summary>
        /// <param name="target">Transform</param>
        /// <param name="endValue">トゥイーン後の値</param>
        /// <param name="duration">トゥイーン時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="callback">トゥイーン完了後のコールバック</param>
        public static void MoveXTween(this Transform target, float endValue, float duration, bool isIgnoreTimeScale = false, UnityAction<Transform> callback = null)
        {
            Vector3 endPosition = target.position;
            endPosition.x = endValue;
            TweenDataSet<VectorTween> dataSet = SetMoveTween(target, endPosition, duration, VectorTweenMode.X, isIgnoreTimeScale, callback);
            if(dataSet.Equals(default)) return;
            moveTweenRunnerTable[dataSet.instanceId].StartTween(dataSet.tweenValue);
        }
        
        /// <summary>
        /// Transformのx座標移動を非同期でトゥイーンするUniTaskを返す
        /// </summary>
        /// <param name="target">Transform</param>
        /// <param name="endValue">トゥイーン後の値</param>
        /// <param name="duration">トゥイーン時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="token">キャンセルトークン（敢えて指定しなくても、AudioSource破壊時にキャンセルはされる）</param>
        /// <returns>Transformのy座標移動をトゥイーンする非同期処理</returns>
        public static UniTask MoveXTweenAsync(this Transform target, float endValue, float duration, bool isIgnoreTimeScale = false, CancellationToken token = default)
        {
            Vector3 endPosition = target.position;
            endPosition.x = endValue;
            TweenDataSet<VectorTween> dataSet = SetMoveTween(target, endPosition, duration, VectorTweenMode.X, isIgnoreTimeScale);
            return dataSet.Equals(default) ? default : moveTweenRunnerTable[dataSet.instanceId].StartTweenAsync(dataSet.tweenValue, token);
        }
        
        /// <summary>
        /// Transformのx座標移動を加算トゥイーンする
        /// </summary>
        /// <param name="target">Transform</param>
        /// <param name="addValue">トゥイーンで加算する分の値</param>
        /// <param name="duration">トゥイーン時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="callback">トゥイーン完了後のコールバック</param>
        public static void AddXTween(this Transform target, float addValue, float duration, bool isIgnoreTimeScale = false, UnityAction<Transform> callback = null)
        {
            Vector3 endPosition = target.position;
            endPosition.x += addValue;
            TweenDataSet<VectorTween> dataSet = SetMoveTween(target, endPosition, duration, VectorTweenMode.X, isIgnoreTimeScale, callback);
            if(dataSet.Equals(default)) return;
            moveTweenRunnerTable[dataSet.instanceId].StartTween(dataSet.tweenValue);
        }
        
        /// <summary>
        /// Transformのx座標移動を非同期で加算トゥイーンするUniTaskを返す
        /// </summary>
        /// <param name="target">Transform</param>
        /// <param name="addValue">トゥイーンで加算する分の値</param>
        /// <param name="duration">トゥイーン時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="token">キャンセルトークン（敢えて指定しなくても、AudioSource破壊時にキャンセルはされる）</param>
        /// <returns>Transformのy座標移動をトゥイーンする非同期処理</returns>
        public static UniTask AddXTweenAsync(this Transform target, float addValue, float duration, bool isIgnoreTimeScale = false, CancellationToken token = default)
        {
            Vector3 endPosition = target.position;
            endPosition.x += addValue;
            TweenDataSet<VectorTween> dataSet = SetMoveTween(target, endPosition, duration, VectorTweenMode.X, isIgnoreTimeScale);
            return dataSet.Equals(default) ? default : moveTweenRunnerTable[dataSet.instanceId].StartTweenAsync(dataSet.tweenValue, token);
        }
        
        /// <summary>
        /// Transformのy座標移動をトゥイーンする
        /// </summary>
        /// <param name="target">Transform</param>
        /// <param name="endValue">トゥイーン後の値</param>
        /// <param name="duration">トゥイーン時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="callback">トゥイーン完了後のコールバック</param>
        public static void MoveYTween(this Transform target, float endValue, float duration, bool isIgnoreTimeScale = false, UnityAction<Transform> callback = null)
        {
            Vector3 endPosition = target.position;
            endPosition.y = endValue;
            TweenDataSet<VectorTween> dataSet = SetMoveTween(target, endPosition, duration, VectorTweenMode.Y, isIgnoreTimeScale, callback);
            if(dataSet.Equals(default)) return;
            moveTweenRunnerTable[dataSet.instanceId].StartTween(dataSet.tweenValue);
        }
        
        /// <summary>
        /// Transformのy座標移動を非同期でトゥイーンするUniTaskを返す
        /// </summary>
        /// <param name="target">Transform</param>
        /// <param name="endValue">トゥイーン後の値</param>
        /// <param name="duration">トゥイーン時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="token">キャンセルトークン（敢えて指定しなくても、AudioSource破壊時にキャンセルはされる）</param>
        /// <returns>Transformのy座標移動をトゥイーンする非同期処理</returns>
        public static UniTask MoveYTweenAsync(this Transform target, float endValue, float duration, bool isIgnoreTimeScale = false, CancellationToken token = default)
        {
            Vector3 endPosition = target.position;
            endPosition.y = endValue;
            TweenDataSet<VectorTween> dataSet = SetMoveTween(target, endPosition, duration, VectorTweenMode.Y, isIgnoreTimeScale);
            return dataSet.Equals(default) ? default : moveTweenRunnerTable[dataSet.instanceId].StartTweenAsync(dataSet.tweenValue, token);
        }
        
        /// <summary>
        /// Transformのy座標移動を加算トゥイーンする
        /// </summary>
        /// <param name="target">Transform</param>
        /// <param name="addValue">トゥイーンで加算する分の値</param>
        /// <param name="duration">トゥイーン時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="callback">トゥイーン完了後のコールバック</param>
        public static void AddYTween(this Transform target, float addValue, float duration, bool isIgnoreTimeScale = false, UnityAction<Transform> callback = null)
        {
            Vector3 endPosition = target.position;
            endPosition.y += addValue;
            TweenDataSet<VectorTween> dataSet = SetMoveTween(target, endPosition, duration, VectorTweenMode.Y, isIgnoreTimeScale, callback);
            if(dataSet.Equals(default)) return;
            moveTweenRunnerTable[dataSet.instanceId].StartTween(dataSet.tweenValue);
        }
        
        /// <summary>
        /// Transformのy座標移動を非同期で加算トゥイーンするUniTaskを返す
        /// </summary>
        /// <param name="target">Transform</param>
        /// <param name="addValue">トゥイーンで加算する分の値</param>
        /// <param name="duration">トゥイーン時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="token">キャンセルトークン（敢えて指定しなくても、AudioSource破壊時にキャンセルはされる）</param>
        /// <returns>Transformのy座標移動をトゥイーンする非同期処理</returns>
        public static UniTask AddYTweenAsync(this Transform target, float addValue, float duration, bool isIgnoreTimeScale = false, CancellationToken token = default)
        {
            Vector3 endPosition = target.position;
            endPosition.y += addValue;
            TweenDataSet<VectorTween> dataSet = SetMoveTween(target, endPosition, duration, VectorTweenMode.Y, isIgnoreTimeScale);
            return dataSet.Equals(default) ? default : moveTweenRunnerTable[dataSet.instanceId].StartTweenAsync(dataSet.tweenValue, token);
        }
        
        /// <summary>
        /// Transformのz座標移動をトゥイーンする
        /// </summary>
        /// <param name="target">Transform</param>
        /// <param name="endValue">トゥイーン後の値</param>
        /// <param name="duration">トゥイーン時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="callback">トゥイーン完了後のコールバック</param>
        public static void MoveZTween(this Transform target, float endValue, float duration, bool isIgnoreTimeScale = false, UnityAction<Transform> callback = null)
        {
            Vector3 endPosition = target.position;
            endPosition.z = endValue;
            TweenDataSet<VectorTween> dataSet = SetMoveTween(target, endPosition, duration, VectorTweenMode.Z, isIgnoreTimeScale, callback);
            if(dataSet.Equals(default)) return;
            moveTweenRunnerTable[dataSet.instanceId].StartTween(dataSet.tweenValue);
        }
        
        /// <summary>
        /// Transformのz座標移動を非同期でトゥイーンするUniTaskを返す
        /// </summary>
        /// <param name="target">Transform</param>
        /// <param name="endValue">トゥイーン後の値</param>
        /// <param name="duration">トゥイーン時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="token">キャンセルトークン（敢えて指定しなくても、AudioSource破壊時にキャンセルはされる）</param>
        /// <returns>Transformのy座標移動をトゥイーンする非同期処理</returns>
        public static UniTask MoveZTweenAsync(this Transform target, float endValue, float duration, bool isIgnoreTimeScale = false, CancellationToken token = default)
        {
            Vector3 endPosition = target.position;
            endPosition.z = endValue;
            TweenDataSet<VectorTween> dataSet = SetMoveTween(target, endPosition, duration, VectorTweenMode.Z, isIgnoreTimeScale);
            return dataSet.Equals(default) ? default : moveTweenRunnerTable[dataSet.instanceId].StartTweenAsync(dataSet.tweenValue, token);
        }
        
        /// <summary>
        /// Transformのz座標移動を加算トゥイーンする
        /// </summary>
        /// <param name="target">Transform</param>
        /// <param name="addValue">トゥイーンで加算する分の値</param>
        /// <param name="duration">トゥイーン時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="callback">トゥイーン完了後のコールバック</param>
        public static void AddZTween(this Transform target, float addValue, float duration, bool isIgnoreTimeScale = false, UnityAction<Transform> callback = null)
        {
            Vector3 endPosition = target.position;
            endPosition.z += addValue;
            TweenDataSet<VectorTween> dataSet = SetMoveTween(target, endPosition, duration, VectorTweenMode.Z, isIgnoreTimeScale, callback);
            if(dataSet.Equals(default)) return;
            moveTweenRunnerTable[dataSet.instanceId].StartTween(dataSet.tweenValue);
        }
        
        /// <summary>
        /// Transformのz座標移動を非同期で加算トゥイーンするUniTaskを返す
        /// </summary>
        /// <param name="target">Transform</param>
        /// <param name="addValue">トゥイーンで加算する分の値</param>
        /// <param name="duration">トゥイーン時間</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="token">キャンセルトークン（敢えて指定しなくても、AudioSource破壊時にキャンセルはされる）</param>
        /// <returns>Transformのy座標移動をトゥイーンする非同期処理</returns>
        public static UniTask AddZTweenAsync(this Transform target, float addValue, float duration, bool isIgnoreTimeScale = false, CancellationToken token = default)
        {
            Vector3 endPosition = target.position;
            endPosition.z += addValue;
            TweenDataSet<VectorTween> dataSet = SetMoveTween(target, endPosition, duration, VectorTweenMode.Z, isIgnoreTimeScale);
            return dataSet.Equals(default) ? default : moveTweenRunnerTable[dataSet.instanceId].StartTweenAsync(dataSet.tweenValue, token);
        }

        /// <summary>
        /// 移動トゥイーンを設定する
        /// </summary>
        /// <param name="target">Transform</param>
        /// <param name="endValue">トゥイーン後の値</param>
        /// <param name="duration">トゥイーン時間</param>
        /// <param name="mode">トゥイーンの対象プロパティの指定</param>
        /// <param name="isIgnoreTimeScale">Time.timeScaleを無視するかどうか</param>
        /// <param name="callback">トゥイーン完了後のコールバック</param>
        /// <returns>移動トゥイーンデータセット</returns>
        private static TweenDataSet<VectorTween> SetMoveTween(Transform target, Vector3 endValue, float duration, VectorTweenMode mode, bool isIgnoreTimeScale = false, UnityAction<Transform> callback = null)
        {
            if (target == null) return default;
            
            int id = target.GetInstanceID();
            bool isBeforeTableContain = moveTweenRunnerTable != null && moveTweenRunnerTable.ContainsKey(id);
        
            // 前回のトゥイーンをキャンセルする
            if (isBeforeTableContain)
            {
                // キャンセルトークンやイベントをリセットしてランナークラスを使いまわす
                moveTweenRunnerTable[id].StopTween();
            }
        
            Vector3 currentValue = target.position;
        
            if (currentValue == endValue)
            {
                target.position = endValue;
                return default;
            }
        
            var vectorTween = new VectorTween(mode, currentValue, endValue, duration, isIgnoreTimeScale);
            vectorTween.onTweenChanged += value => target.position = value;
        
            // TweenRunnerがなければ登録する
            if (!isBeforeTableContain)
            {
                moveTweenRunnerTable ??= new Dictionary<int, TweenRunner<VectorTween>>(1);
                moveTweenRunnerTable.Add(id, new TweenRunner<VectorTween>(target , id));
            }
            
            // コールバックがあれば登録する
            if (callback != null)
            {
                moveTweenRunnerTable[id].onTweenFinished += _ => callback.Invoke(target);
            }
            moveTweenRunnerTable[id].onTweenFinished += instanceID =>
            {
                moveTweenRunnerTable[instanceID].Dispose();
                moveTweenRunnerTable.Remove(instanceID);
            };
        
            return new TweenDataSet<VectorTween>(vectorTween, id);
        }
        
        /// <summary>
        /// Transformの座標移動のトゥイーンを停止する
        /// </summary>
        /// <param name="target">Transform</param>
        public static void MoveStopTween(this Transform target)
        {
            if (target == null) return;
            
            int id = target.GetInstanceID();
        
            // 前回のトゥイーンをキャンセルする
            if (moveTweenRunnerTable != null && moveTweenRunnerTable.ContainsKey(id))
            {
                moveTweenRunnerTable[id].StopTween();
                moveTweenRunnerTable[id].Dispose();
                moveTweenRunnerTable.Remove(id);
            }
        }
    }
}