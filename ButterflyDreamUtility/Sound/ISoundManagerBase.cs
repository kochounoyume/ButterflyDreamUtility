using UnityEngine;

namespace ButterflyDreamUtility.Sound
{
    /// <summary>
    /// BGMのループモード
    /// </summary>
    public enum BgmLoopMode : int
    {
        /// <summary>ループなし</summary>
        None = 0,
        /// <summary>インタラクティブなループ</summary>
        InteractiveLoop = 1,
        /// <summary>強制ループ</summary>
        Force = 2,
    }
    
    /// <summary>
    /// ゲーム内の音楽再生を管理する処理の基底インターフェイス
    /// </summary>
    public interface ISoundManagerBase
    {
        /// <summary>
        /// BGMの再生状態を取得
        /// </summary>
        /// <returns>再生中なら true </returns>
        bool isPlayBgm { get; }

        /// <summary>
        /// BGMを再生
        /// </summary>
        /// <param name="bgmAudioClip">再生したいAudioClip</param>
        /// <param name="startTime">途中から再生したければこちらを設定</param>
        /// <param name="isFadein">初回のみフェードインして再生するか否か</param>
        /// <param name="duration">初回のフェードインにかける時間</param>
        /// <param name="bgmLoopMode">ループ設定オプション　デフォルトではただループではなく次のループをランダムな部分から再生して飽きさせないループにする</param>
        void PlayBgm(AudioClip bgmAudioClip, float startTime, bool isFadein, float duration, BgmLoopMode bgmLoopMode);

        /// <summary>
        /// BGMを停止
        /// </summary>
        /// <param name="isFadeout">フェードアウトして停止するか否か</param>
        /// <param name="duration">フェードアウトにかける時間</param>
        void StopBgm(bool isFadeout, float duration);
        
        /// <summary>
        /// 現在再生中のBGMを一時停止
        /// </summary>
        void PauseBgm();

        /// <summary>
        /// 現在一時停止中のBGMを再生
        /// </summary>
        void PlayAgainBgm();

        /// <summary>
        /// BGMを割り込み再生（再生中のBGMの再生状態を保持したまま、任意のBGMを再生する）
        /// </summary>
        /// <param name="bgmAudioClip">割り込み再生したいAudioClip</param>
        /// <param name="startTime">途中から再生したければこちらを設定</param>
        /// <param name="isLoop">ループ設定の可否　デフォルトはtrue</param>
        void InterruptBgm(AudioClip bgmAudioClip, float startTime, bool isLoop);
        
        /// <summary>
        /// 割り込んだBGMの再生を終了し、元のBGMを保持した状態から再生する
        /// </summary>
        void ResumeBgm();

        /// <summary>
        /// ユーザーから指定された任意の値をBGMのボリュームに反映する
        /// </summary>
        /// <param name="value">任意の値</param>
        void SetVolumeBgm(float value);

        /// <summary>
        /// SEを再生
        /// </summary>
        /// <param name="seAudioClip">再生したいAudioClip</param>
        void PlaySe(AudioClip seAudioClip);
        
        /// <summary>
        /// ユーザーから指定された任意の値をSEのボリュームに反映する
        /// </summary>
        /// <param name="value">任意の値</param>
        void SetVolumeSe(float value);
        
        /// <summary>
        /// ボイスを再生
        /// </summary>
        /// <param name="voiceAudioClip">再生したいAudioClip</param>
        void PlayVoice(AudioClip voiceAudioClip);
        
        /// <summary>
        /// ユーザーから指定された任意の値をボイスのボリュームに設定する
        /// </summary>
        /// <param name="value">任意の値</param>
        void SetVolumeVoice(float value);

        /// <summary>
        /// 全ての音を停止
        /// </summary>
        /// <param name="isFadeout">フェードアウトして停止するか否か</param>
        /// <param name="duration">フェードアウトにかける時間</param>
        void StopAllSound(bool isFadeout, float duration);
        
        /// <summary>
        /// 現在再生中の全ての音を一時停止
        /// </summary>
        void PauseAllSound();
        
        /// <summary>
        /// 現在一時停止中の全ての音を再生
        /// </summary>
        void PlayAgainAllSound();
        
        /// <summary>
        /// ユーザーから指定された任意の値をマスターボリュームに設定する
        /// </summary>
        /// <param name="value">任意の値</param>
        void SetVolumeMaster(float value);
    }
}