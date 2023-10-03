using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace ButterflyDreamUtility.Sound
{
    using Extensions;

    /// <summary>
    /// ゲーム内の音楽再生を管理する基底クラス
    /// </summary>
    public class SoundManagerBase : ISoundManagerBase
    {
        /// <summary>
        /// BGM各チャンネル
        /// </summary>
        private enum BgmChannel : int
        {
            /// <summary>メインチャンネル1(自然なループ切り替え用にメインチャンネルが2つある)</summary>
            Main1 = 0,
            /// <summary>メインチャンネル2(自然なループ切り替え用にメインチャンネルが2つある)</summary>
            Main2 = 1,
            /// <summary>割り込みチャンネル</summary>
            Interrupt = 2,
            /// <summary>フェードチャンネル</summary>
            Fade = 3,
        }

        /// <summary>
        /// AudioSourceがアタッチされたゲームオブジェクト名
        /// </summary>
        private const string OBJECT_NAME = "SoundManager";

        /// <summary>
        /// BGMのチャンネル数
        /// </summary>
        private const int BGM_CHANNEL = 4;

        /// <summary>
        /// マスターボリュームを管理するAudioMixerGroup名
        /// </summary>
        private const string MASTER_GROUP_NAME = "Master";

        /// <summary>
        /// BGMのボリュームを管理するAudioMixerGroup名
        /// </summary>
        private const string BGM_GROUP_NAME = "BGM";

        /// <summary>
        /// SEのボリュームを管理するAudioMixerGroup名
        /// </summary>
        private const string SE_GROUP_NAME = "SE";

        /// <summary>
        /// ボイスのボリュームを管理するAudioMixerGroup名
        /// </summary>
        private const string VOICE_GROUP_NAME = "Voice";

        /// <summary>
        /// フェード時間定数
        /// </summary>
        private const float BGM_FADE_TIME = 5.0f;

        /// <summary>
        /// オーディオミキサー
        /// </summary>
        private readonly AudioMixer audioMixer = null;

        /// <summary>
        /// BGM用のAudioSourceチャンネル配列
        /// </summary>
        private readonly AudioSource[] bgmAudioSources = null;

        /// <summary>
        /// SE用のAudioSourceチャンネル
        /// </summary>
        private readonly AudioSource seAudioSource = null;

        /// <summary>
        /// ボイス用のAudioSourceチャンネル
        /// </summary>
        private readonly AudioSource voiceAudioSource = null;

        /// <inheritdoc />
        public bool isPlayBgm { get; private set; } = false;

        /// <summary>
        /// BGM用のキャンセルトークンソース
        /// </summary>
        private CancellationTokenSource bgmCancellationTokenSource = null;

        /// <summary>
        /// 現在再生中のBGMのAudioClip
        /// </summary>
        private AudioClip currentBgmClip = null;

        public SoundManagerBase(AudioMixer audioMixer)
        {
            this.audioMixer = audioMixer;

            const int amountChannel = BGM_CHANNEL + 2;
            Type[] components = new Type[amountChannel];
            for (int i = 0; i < amountChannel; i++)
            {
                components[i] = typeof(AudioSource);
            }
            AudioSource[] audioSources = new GameObject(OBJECT_NAME, components).GetComponents<AudioSource>();

            bgmAudioSources = new AudioSource[BGM_CHANNEL];
            Array.Copy(audioSources, bgmAudioSources, BGM_CHANNEL);
            foreach (var bgmAudioSource in bgmAudioSources)
            {
                bgmAudioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups(BGM_GROUP_NAME)[0];
            }

            seAudioSource = audioSources[amountChannel - 2];
            seAudioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups(SE_GROUP_NAME)[0];

            voiceAudioSource = audioSources[amountChannel - 1];
            voiceAudioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups(VOICE_GROUP_NAME)[0];
        }

        /// <inheritdoc />
        public void PlayBgm(
            AudioClip bgmAudioClip,
            float startTime = 0,　
            bool isFadein = false,
            float duration = BGM_FADE_TIME,
            BgmLoopMode bgmLoopMode = BgmLoopMode.InteractiveLoop)
        {
            // 現在再生しているBGMと同じBGMを再生コールしようとしているときは何もしない
            if (currentBgmClip == bgmAudioClip) return;

            bgmCancellationTokenSource?.Cancel();
            bgmCancellationTokenSource?.Dispose();
            bgmCancellationTokenSource = null;

            // インタラクティブループではない場合
            if (bgmLoopMode != BgmLoopMode.InteractiveLoop)
            {
                if (isPlayBgm)
                {
                    // 一回全停止
                    foreach (var bgmAudioSource in bgmAudioSources)
                    {
                        if (!bgmAudioSource.isPlaying) continue;
                        bgmAudioSource.Stop();
                        bgmAudioSource.volume = 1;
                    }
                }

                AudioSource mainBgmChannel = bgmAudioSources[(int) BgmChannel.Main1];
                mainBgmChannel.clip = bgmAudioClip;
                mainBgmChannel.time = startTime % bgmAudioClip.length;
                mainBgmChannel.loop = bgmLoopMode == BgmLoopMode.Force;
                mainBgmChannel.Play();
                isPlayBgm = true;
                currentBgmClip = bgmAudioClip;
                if (isFadein)
                {
                    mainBgmChannel.volume = 0;
                    mainBgmChannel.FadeTween(1,duration);
                }
                return;
            }

            // インタラクティブループの場合；ループ処理にUniTask使うのでキャンセルトークン作成
            bgmCancellationTokenSource = new CancellationTokenSource();

            // 現時点で再生しているBGMがないとき
            if (!isPlayBgm)
            {
                AudioSource mainChannel = bgmAudioSources[(int) BgmChannel.Main1];
                mainChannel.clip = bgmAudioClip;
                mainChannel.time = startTime % bgmAudioClip.length;
                mainChannel.loop = false;
                mainChannel.Play();
                isPlayBgm = true;
                currentBgmClip = bgmAudioClip;
                if (isFadein)
                {
                    mainChannel.volume = 0;
                    mainChannel.FadeTween(1,duration);
                }
                InteractiveLoop().Forget();
                return;
            }

            // 現時点で既にBGMを再生しているとき
            // 再生中のメインチャンネルを取得
            List<AudioSource> playingChannels = new List<AudioSource>(1);
            if (bgmAudioSources[(int) BgmChannel.Main1].isPlaying)
            {
                playingChannels.Add(bgmAudioSources[(int)BgmChannel.Main1]);
            }
            if (bgmAudioSources[(int)BgmChannel.Main2].isPlaying)
            {
                playingChannels.Add(bgmAudioSources[(int)BgmChannel.Main2]);
            }

            // フェードチャンネルが空いていないとき
            if (bgmAudioSources[(int) BgmChannel.Fade].isPlaying)
            {
                foreach (var playingChannel in playingChannels)
                {
                    // もしトゥイーンしてたら停止
                    playingChannel.FadeStopTween();
                    // 停止
                    playingChannel.Stop();
                    // AudioClipもリセットしておく
                    playingChannel.clip = null;
                }
                // フェードチャンネルをメインチャンネルに移動
                (bgmAudioSources[(int)BgmChannel.Main1], bgmAudioSources[(int)BgmChannel.Fade])
                    = (bgmAudioSources[(int)BgmChannel.Fade], bgmAudioSources[(int)BgmChannel.Main1]);
                // 再生中のチャンネルに変化が生じたので、playingChannelsを更新する
                playingChannels.Clear();
                playingChannels.Add(bgmAudioSources[(int)BgmChannel.Main1]);
            }

            // 現在再生中のチャンネルをフェードアウト
            foreach (var playingChannel in playingChannels)
            {
                // 念のためループ設定無効化
                playingChannel.loop = false;
                // フェードアウト
                playingChannel.FadeTween(0, BGM_FADE_TIME, false, channel =>
                {
                    channel.Stop();
                    channel.clip = null;
                });
            }

            // 次のBGMをフェードチャンネルで再生
            AudioSource fadeChannel = bgmAudioSources[(int) BgmChannel.Fade];
            fadeChannel.clip = bgmAudioClip;
            fadeChannel.time = startTime % bgmAudioClip.length;
            fadeChannel.Play();
            // フェードイン
            fadeChannel.volume = 0;
            fadeChannel.FadeTween(1, BGM_FADE_TIME);
            // 現在再生中のBGMを更新
            currentBgmClip = bgmAudioClip;
            // BGM音楽の切り替えをして、その後はインタラクティブループ
            UniTask.Void(async token =>
            {
                await UniTask.Delay(TimeSpan.FromSeconds(BGM_FADE_TIME), cancellationToken: token);
                (bgmAudioSources[(int) BgmChannel.Main1], bgmAudioSources[(int) BgmChannel.Fade])
                    = (bgmAudioSources[(int) BgmChannel.Fade], bgmAudioSources[(int) BgmChannel.Main1]);
                InteractiveLoop().Forget();
            }, bgmCancellationTokenSource.Token);

            // インタラクティブループのループ処理
            async UniTaskVoid InteractiveLoop()
            {
                // 現在再生されているBGMのランダムな時点を取得
                float bgmLength = bgmAudioSources[(int) BgmChannel.Main1].clip.length;
                // 初回は再生状態をリセットするためにここで待機
                await UniTask.Delay(
                    TimeSpan.FromSeconds(bgmLength - bgmAudioSources[(int) BgmChannel.Main1].time - BGM_FADE_TIME),
                    cancellationToken: bgmCancellationTokenSource.Token);
                while (true)
                {
                    // ランダムな始点を取得
                    float randomTime = bgmLength > 30 ? Random.Range(0, bgmLength - 30) : 0;
                    (AudioSource prevChannel, AudioSource nextChannel) = bgmAudioSources[(int) BgmChannel.Main1].isPlaying
                        ? (bgmAudioSources[(int) BgmChannel.Main1], bgmAudioSources[(int) BgmChannel.Main2])
                        : (bgmAudioSources[(int) BgmChannel.Main2], bgmAudioSources[(int) BgmChannel.Main1]);
                    // フェードアウト
                    prevChannel.FadeTween(0, BGM_FADE_TIME, false, channel => channel.Stop());
                    nextChannel.clip ??= bgmAudioClip;
                    nextChannel.time = randomTime;
                    nextChannel.Play();
                    nextChannel.volume = 0;
                    // フェードイン
                    nextChannel.FadeTween(1, BGM_FADE_TIME);
                    await UniTask.Delay(TimeSpan.FromSeconds(bgmLength - randomTime - BGM_FADE_TIME), cancellationToken: bgmCancellationTokenSource.Token);
                }
            }
        }

        /// <inheritdoc />
        public void StopBgm(bool isFadeout = false, float duration = BGM_FADE_TIME)
        {
            foreach (var bgmAudioSource in bgmAudioSources)
            {
                if (!bgmAudioSource.isPlaying) continue;
                if (isFadeout)
                {
                    bgmAudioSource.FadeTween(0, duration, false, channel =>
                    {
                        channel.Stop();
                        channel.clip = null;
                    });
                }
                else
                {
                    bgmAudioSource.Stop();
                    bgmAudioSource.clip = null;
                }
            }
            isPlayBgm = false;
            bgmCancellationTokenSource?.Cancel();
            bgmCancellationTokenSource?.Dispose();
            bgmCancellationTokenSource = null;
        }

        /// <inheritdoc />
        public void PauseBgm()
        {
            foreach (var bgmAudioSource in bgmAudioSources)
            {
                if (bgmAudioSource.isPlaying)
                {
                    bgmAudioSource.Pause();
                }
            }
        }

        /// <inheritdoc />
        public void PlayAgainBgm()
        {
            foreach (var bgmAudioSource in bgmAudioSources)
            {
                bgmAudioSource.UnPause();
            }
        }

        /// <inheritdoc />
        public void InterruptBgm(AudioClip bgmAudioClip, float startTime = 0, bool isLoop = true)
        {
            PauseBgm();
            AudioSource interruptChannel = bgmAudioSources[(int) BgmChannel.Interrupt];
            interruptChannel.clip = bgmAudioClip;
            interruptChannel.time = startTime;
            interruptChannel.loop = isLoop;
            interruptChannel.Play();
        }

        /// <inheritdoc />
        public void ResumeBgm()
        {
            PlayAgainBgm();
            AudioSource interruptChannel = bgmAudioSources[(int) BgmChannel.Interrupt];
            interruptChannel.Stop();
            interruptChannel.clip = null;
        }

        /// <inheritdoc />
        public void SetVolumeBgm(float value)
        {
            audioMixer.SetFloat(BGM_GROUP_NAME, Mathf.Clamp(Mathf.Log10(value) * 20, -80f, 0f));
        }

        /// <inheritdoc />
        public void PlaySe(AudioClip seAudioClip)
        {
            seAudioSource.PlayOneShot(seAudioClip);
        }

        /// <inheritdoc />
        public void SetVolumeSe(float value)
        {
            audioMixer.SetFloat(SE_GROUP_NAME, Mathf.Clamp(Mathf.Log10(value) * 20, -80f, 0f));
        }

        /// <inheritdoc />
        public void PlayVoice(AudioClip voiceAudioClip)
        {
            voiceAudioSource.PlayOneShot(voiceAudioClip);
        }

        /// <inheritdoc />
        public void SetVolumeVoice(float value)
        {
            audioMixer.SetFloat(VOICE_GROUP_NAME, Mathf.Clamp(Mathf.Log10(value) * 20, -80f, 0f));
        }

        /// <inheritdoc />
        public void StopAllSound(bool isFadeout, float duration)
        {
            StopBgm(isFadeout, duration);
            if (seAudioSource.isPlaying)
            {
                if (isFadeout)
                {
                    seAudioSource.FadeTween(0, duration, false, channel =>
                    {
                        channel.Stop();
                        channel.clip = null;
                    });
                }
                else
                {
                    seAudioSource.Stop();
                    seAudioSource.clip = null;
                }
            }
            if (voiceAudioSource.isPlaying)
            {
                if (isFadeout)
                {
                    voiceAudioSource.FadeTween(0, duration, false, channel =>
                    {
                        channel.Stop();
                        channel.clip = null;
                    });
                }
                else
                {
                    voiceAudioSource.Stop();
                    voiceAudioSource.clip = null;
                }
            }
        }

        /// <inheritdoc />
        public void PauseAllSound()
        {
            PauseBgm();
            if (seAudioSource.isPlaying)
            {
                seAudioSource.Pause();
            }

            if (voiceAudioSource.isPlaying)
            {
                voiceAudioSource.Pause();
            }
        }

        /// <inheritdoc />
        public void PlayAgainAllSound()
        {
            PlayAgainBgm();
            if (seAudioSource.isPlaying)
            {
                seAudioSource.UnPause();
            }

            if (voiceAudioSource.isPlaying)
            {
                voiceAudioSource.UnPause();
            }
        }

        /// <inheritdoc />
        public void SetVolumeMaster(float value)
        {
            audioMixer.SetFloat(MASTER_GROUP_NAME, Mathf.Clamp(Mathf.Log10(value) * 20, -80f, 0f));
        }
    }
}