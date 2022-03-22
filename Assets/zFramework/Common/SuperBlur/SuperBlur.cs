﻿using UnityEngine;

namespace SuperBlurring
{

    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Effects/Super Blur", -1)]
    public class SuperBlur : SuperBlurBase
    {
        public float Interaction
        {
            set
            {
                enabled = value != 0;
                this.interpolation = value;
            }
        }
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {

            //if (interpolation == 0)
            //{
            // //   Graphics.Blit(source, destination);
            //    return;
            //}
            if (blurMaterial == null || UIMaterial == null) return;

            int tw = source.width >> downsample;
            int th = source.height >> downsample;

            var rt = RenderTexture.GetTemporary(tw, th, 0, source.format);

            Graphics.Blit(source, rt);

            if (renderMode == RenderMode.Screen)
            {
                Blur(rt, destination);
            }
            else if (renderMode == RenderMode.UI)
            {
                Blur(rt, rt);
                UIMaterial.SetTexture(Uniforms._BackgroundTexture, rt);
                Graphics.Blit(source, destination);
            }
            else if (renderMode == RenderMode.OnlyUI)
            {
                Blur(rt, rt);
                UIMaterial.SetTexture(Uniforms._BackgroundTexture, rt);
            }

            RenderTexture.ReleaseTemporary(rt);
        }

    }

}
