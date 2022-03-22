using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zFrame;
namespace zFrame.Media
{
    /// <summary>
    /// 音频管理器
    /// </summary>
    public class AudioManager : MonoSingleton<AudioManager>
    {
        /// <summary>
        /// 清空
        /// </summary>
        public  void Clean()
        {
            AudioExtend.Clean();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public override void OnInit()
        {

        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            Clean();
        }
    }


    public static class AudioExtend
    {
        /// <summary>
        /// 驱动器缓存
        /// </summary>
        private static List<AudioEventDriver> list = new List<AudioEventDriver>();
        /// <summary>
        /// 清空缓存
        /// </summary>
        public static void Clean()
        {
            foreach (var item in list)
            {
                AudioEventDriver audioEventDriver = item.gameObject.GetComponent<AudioEventDriver>();
                GameObject.Destroy(audioEventDriver);
                
            }
            list = new List<AudioEventDriver>();
        }
        /// <summary>
        /// 获得一个音频播放器
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static AudioSource GetAudio(this GameObject gameObject, bool add = false)
        {
           
            AudioSource audio = null;
            if (!add)
            {
                audio = gameObject.GetComponent<AudioSource>();
                if (audio == null)
                {
                    audio = gameObject.AddComponent<AudioSource>();
                }
            }
            if (add)
            {
                audio = gameObject.AddComponent<AudioSource>();
            }
            return audio;
        }
        /// <summary>
        /// 设置Clip
        /// </summary>
        /// <param name="audioSource"></param>
        /// <returns></returns>
        public static AudioSource SetClip(this AudioSource audioSource, AudioClip clip)
        {
            if (clip)
            {
                audioSource.clip = clip;
            }
            return audioSource;
        }
        /// <summary>
        /// 获取音频事件驱动
        /// </summary>
        /// <param name="audioSource"></param>
        /// <returns></returns>
        private static AudioEventDriver GetEventDriver(this AudioSource audioSource)
        {
            AudioEventDriver AudioEventDriver = audioSource.gameObject.GetComponent<AudioEventDriver>();
            if (AudioEventDriver == null)
            {
                AudioEventDriver = audioSource.gameObject.AddComponent<AudioEventDriver>();
                list.Add(AudioEventDriver);
            }
            return AudioEventDriver;
        }
        /// <summary>
        /// 音频播放中回调
        /// </summary>
        /// <param name="audioSource"></param>
        /// <param name="onUpdateEventHandler"></param>
        /// <returns></returns>
        public static AudioSource OnUpdate(this AudioSource audioSource, AudioEventDriver.OnUpdateEventHandler onUpdateEventHandler)
        {
            GetEventDriver(audioSource).OnUpdate(audioSource, onUpdateEventHandler);
            return audioSource;
        }
        /// <summary>
        /// 音频播放完成回调
        /// </summary>
        /// <param name="audioSource"></param>
        /// <param name="onUpdateEventHandler"></param>
        /// <returns></returns>
        public static AudioSource OnComplete(this AudioSource audioSource, AudioEventDriver.OnCompleteEventHandler onCompleteEventHandler)
        {
            GetEventDriver(audioSource).OnComplete(audioSource, onCompleteEventHandler);
            return audioSource;
        }
    }
    /// <summary>
    /// 音频事件驱动器
    /// </summary>
    public class AudioEventDriver : MonoBehaviour
    {
        /// <summary>
        /// 委托定义
        /// </summary>
        /// <param name="Percentage">百分比</param>
        public delegate void OnUpdateEventHandler(float Percentage);
        public delegate void OnCompleteEventHandler();
        /// <summary>
        ///  委托实例
        /// </summary>
        public AudioEventDriver.OnUpdateEventHandler onUpdate = null;
        public AudioEventDriver.OnCompleteEventHandler onComplete = null;
        private AudioSource audioSource = null;
        /// <summary>
        /// OnUpdate 驱动方法
        /// </summary>
        /// <param name="audioSource_"></param>
        /// <param name="onUpdateEventHandler"></param>
        public void OnUpdate(AudioSource audioSource_, AudioEventDriver.OnUpdateEventHandler onUpdateEventHandler)
        {
            onUpdate = onUpdateEventHandler;
            audioSource = audioSource_;
        }
        public void OnComplete(AudioSource audioSource_, AudioEventDriver.OnCompleteEventHandler onCompleteEventHandler)
        {
            audioSource = audioSource_;
            onComplete = onCompleteEventHandler;
        }
        private void Update()
        {
            //OnUpdate 驱动
            if (onUpdate != null && audioSource != null)
            {
                if (audioSource.isPlaying)
                {
                    onUpdate(audioSource.time / audioSource.clip.length);
                }
            }

            //OnComplete 驱动
            if (audioSource.time == audioSource.clip.length)
            {
                onComplete();
            }
        }
    }
}
