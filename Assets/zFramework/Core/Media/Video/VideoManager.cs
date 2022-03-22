using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System;


namespace zFrame.Media
{
    /// <summary>
    /// 视频管理器
    /// </summary>
    public class VideoManager : MonoSingleton<VideoManager>
    {
        /// <summary>
        /// 初始化Manager
        /// </summary>
        public override void OnInit()
        {

        }
        /// <summary>
        ///清空缓存
        /// </summary>
        public  void Clean()
        {
            VideoExtend.Clean();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            Clean();
        }

    }
    public static class VideoExtend
    {
        /// <summary>
        /// 驱动器缓存
        /// </summary>
        private static List<VideoEventDriver> list = new List<VideoEventDriver>();
        /// <summary>
        /// 清空缓存
        /// </summary>
        public static void Clean()
        {
            foreach (var item in list)
            {
                VideoPlayer videoPlayer = item.gameObject.GetComponent<VideoPlayer>();
                GameObject.Destroy(item);
                GameObject.Destroy(videoPlayer);
            }
            list = new List<VideoEventDriver>();
        }
        /// <summary>
        /// MeshRenderer 渲染
        /// <summary>
        private static bool SetMeshRenderer(VideoPlayer videoPlayer, VideoStyle videoStyle, bool Add = false)
        {
            MeshRenderer meshRenderer = videoPlayer.gameObject.GetComponent<MeshRenderer>();
            Material[] materials;
            Vector2 offset = Vector2.one;
            if (meshRenderer == null && !Add)
            {
                return false;
            }
            else if (meshRenderer == null)
            {
                meshRenderer = videoPlayer.gameObject.AddComponent<MeshRenderer>();
            }

            switch (videoStyle)
            {
                case VideoStyle.Transparent:
                    materials = new Material[] { new Material(Shader.Find("zFrame/Video/Transparent")) };
                    offset = new Vector2(0.5f, 1);
                    break;
                case VideoStyle.Default:
                default:
                    materials = new Material[] { new Material(Shader.Find("zFrame/Video/Default")) };
                    break;
            }
            RenderTexture renderTexture = new RenderTexture((int)videoPlayer.clip.width, (int)videoPlayer.clip.height, 24);
            videoPlayer.renderMode = VideoRenderMode.RenderTexture;
            videoPlayer.targetTexture = renderTexture;
            materials[0].SetTexture("_MainTex", renderTexture);
            materials[0].SetTextureScale("_MainTex", offset);
            meshRenderer.materials = materials;
            return true;
        }
        /// <summary>
        /// RawImage 渲染
        /// <summary>
        /// <param name="videoPlayer"></param>
        /// <param name="videoStyle"> </param>
        /// <param name="Add"></param>
        /// <returns></returns>
        private static bool SetRawImage(VideoPlayer videoPlayer, VideoStyle videoStyle, bool Add = false)
        {
            RawImage rawImage = videoPlayer.gameObject.GetComponent<RawImage>();
            Material material;
            Vector2 offset = Vector2.one;
            if (rawImage == null && !Add)
            {
                return false;
            }
            else if (rawImage == null)
            {
                rawImage = videoPlayer.gameObject.AddComponent<RawImage>();
            }
            RenderTexture renderTexture = new RenderTexture((int)videoPlayer.clip.width, (int)videoPlayer.clip.height, 24);
            switch (videoStyle)
            {
                case VideoStyle.Transparent:
                    material = new Material(Shader.Find("zFrame/Video/Transparent"));
                    offset = new Vector2(0.5f, 1);
                    material.SetTexture("_MainTex", renderTexture);
                    material.SetTextureScale("_MainTex", offset);
                    rawImage.material = material;
                    break;
                case VideoStyle.Default:
                default:
                    material = new Material(Shader.Find("zFrame/Video/Default"));
                    break;
            }
            videoPlayer.renderMode = VideoRenderMode.RenderTexture;
            videoPlayer.targetTexture = renderTexture;
            rawImage.texture = renderTexture;

            return true;
        }
        /// <summary>
        /// 渲染顺序封装 
        /// </summary>
        private static void SetMaterial(VideoPlayer videoPlayer, VideoStyle videoStyle)
        {
            if (SetRawImage(videoPlayer, videoStyle))
            {
                return;
            }
            else if (SetMeshRenderer(videoPlayer, videoStyle))
            {
                return;
            }
            SetMeshRenderer(videoPlayer, videoStyle, true);
        }
        /// <summary>
        /// 设置视频样式 
        /// </summary>
        /// <param name="videoPlayer">视频组件</param>
        /// <param name="videoStyle">样式枚举，UGUI播放透明视频Canvas渲染模式为：ScreenSpace - Camera</param>
        /// <returns></returns>
        public static VideoPlayer SetStyle(this VideoPlayer videoPlayer, VideoStyle videoStyle = VideoStyle.Default)
        {
            SetMaterial(videoPlayer, videoStyle);

            return videoPlayer;
        }
        /// <summary>
        /// 获得视频事件驱动
        /// </summary>
        /// <param name="videoPlayer"></param>
        /// <returns></returns>
        private static VideoEventDriver GetEventDriver(VideoPlayer videoPlayer)
        {
            VideoEventDriver videoEventDriver = videoPlayer.gameObject.GetComponent<VideoEventDriver>();
            if (videoEventDriver == null)
            {
                videoEventDriver = videoPlayer.gameObject.AddComponent<VideoEventDriver>();
                list.Add(videoEventDriver);
            }
            return videoEventDriver;
        }
        /// <summary>
        ///  播放结束或播放到循环的点时回调
        /// </summary>
        /// <param name="videoPlayer"></param>
        /// <param name="onLoopPointReached"></param>
        /// <returns></returns>
        public static VideoPlayer OnLoopPointReached(this VideoPlayer videoPlayer, VideoPlayer.EventHandler onLoopPointReached)
        {
            videoPlayer.loopPointReached += onLoopPointReached;
            return videoPlayer;
        }
        /// <summary>
        /// 视频准备完成时回调
        /// </summary>
        /// <param name="videoPlayer"></param>
        /// <param name="onPrepareCompleted"></param>
        /// <returns></returns>
        public static VideoPlayer OnPrepareCompleted(this VideoPlayer videoPlayer, VideoPlayer.EventHandler onPrepareCompleted)
        {
            videoPlayer.prepareCompleted += onPrepareCompleted;
            return videoPlayer;
        }
        /// <summary>
        /// 视频播放中回调
        /// </summary>
        /// <param name="videoPlayer"></param>
        /// <param name="onUpdate"></param>
        /// <returns></returns>
        public static VideoPlayer OnUpdate(this VideoPlayer videoPlayer, VideoEventDriver.FrameReadyEventHandler onUpdate)
        {
            GetEventDriver(videoPlayer).OnUpdate(videoPlayer, onUpdate);
            return videoPlayer;
        }
        /// <summary>
        /// 卸载视频事件驱动
        /// </summary>
        /// <param name="videoPlayer"></param>
        /// <returns></returns>
        public static VideoPlayer UninEventDriver(this VideoPlayer videoPlayer)
        {
            VideoEventDriver videoEventDriver = GetEventDriver(videoPlayer);
            if (!videoEventDriver)
            {
                list.Remove(videoEventDriver);
                GameObject.Destroy(videoEventDriver);
            }
            return videoPlayer;
        }
    }
    /// <summary>
    /// 视频事件驱动器
    /// </summary>
    public class VideoEventDriver : MonoBehaviour
    {
        /// <summary>
        /// 委托定义
        /// </summary>
        /// <param name="source">播放器</param>
        /// <param name="frameIdx">当前帧</param>
        /// <param name="time">当前时间</param>
        public delegate void FrameReadyEventHandler(VideoPlayer source, float frameIdx, float time);
        /// <summary>
        /// 委托实例
        /// </summary>
        public VideoEventDriver.FrameReadyEventHandler onUpdate = null;
        private VideoPlayer videoPlayer = null;
        /// <summary>
        /// 驱动方法
        /// </summary>
        /// <param name="videoPlayer_"></param>
        /// <param name="onUpdate_"></param>
        public void OnUpdate(VideoPlayer videoPlayer_, VideoEventDriver.FrameReadyEventHandler onUpdate_)
        {
            onUpdate = onUpdate_;
            videoPlayer = videoPlayer_;
        }
        /// <summary>
        /// MonoBehaviour Update 更新驱动函数
        /// </summary>
        private void Update()
        {
            if (onUpdate != null && videoPlayer != null)
            {
                if (videoPlayer.isPlaying)
                {
                    onUpdate(videoPlayer, (float)videoPlayer.frame, (float)videoPlayer.time);
                }
            }
        }
    }
    public enum VideoStyle
    {
        /// <summary>
        /// 默认样式
        /// </summary>
        Default,
        /// <summary>
        /// 透明样式
        /// </summary>
        Transparent
    }

}
