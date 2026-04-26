using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

public class SceneOutlineRendererFeature : ScriptableRendererFeature
{
    [Serializable]
    public class Settings
    {
        public Material OutlineMaterial;
        public RenderPassEvent InjectionPoint = RenderPassEvent.AfterRenderingTransparents;
    }

    private class SceneOutlinePass : ScriptableRenderPass
    {
        private static readonly ProfilingSampler ProfilingSampler = new("Scene Outline");

        private readonly Material _material;
        private RTHandle _temporaryColor;

        public SceneOutlinePass(Material material, RenderPassEvent injectionPoint)
        {
            _material = material;
            renderPassEvent = injectionPoint;
            requiresIntermediateTexture = true;
            ConfigureInput(ScriptableRenderPassInput.Depth | ScriptableRenderPassInput.Normal);
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = 0;
            descriptor.msaaSamples = 1;

            RenderingUtils.ReAllocateIfNeeded(
                ref _temporaryColor,
                descriptor,
                FilterMode.Bilinear,
                TextureWrapMode.Clamp,
                name: "_SceneOutlineTemp");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (_material == null)
            {
                return;
            }

            if (renderingData.cameraData.isPreviewCamera)
            {
                return;
            }

            CommandBuffer cmd = CommandBufferPool.Get();

            using (new ProfilingScope(cmd, ProfilingSampler))
            {
                RTHandle cameraColorTarget = renderingData.cameraData.renderer.cameraColorTargetHandle;
                Blitter.BlitCameraTexture(cmd, cameraColorTarget, _temporaryColor);
                Blitter.BlitCameraTexture(cmd, _temporaryColor, cameraColorTarget, _material, 0);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            if (_material == null)
            {
                return;
            }

            UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
            if (cameraData.isPreviewCamera)
            {
                return;
            }

            UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
            if (resourceData.isActiveTargetBackBuffer)
            {
                Debug.LogWarning("Skipping Scene Outline pass because the active target is the back buffer.");
                return;
            }

            TextureHandle source = resourceData.activeColorTexture;
            TextureDesc destinationDesc = renderGraph.GetTextureDesc(source);
            destinationDesc.name = "CameraColor-SceneOutline";
            destinationDesc.clearBuffer = false;

            TextureHandle destination = renderGraph.CreateTexture(destinationDesc);
            RenderGraphUtils.BlitMaterialParameters blitParameters = new(source, destination, _material, 0);
            renderGraph.AddBlitPass(blitParameters, passName: "Scene Outline");

            resourceData.cameraColor = destination;
        }

        public void Dispose()
        {
            _temporaryColor?.Release();
        }
    }

    public Settings FeatureSettings = new();

    private SceneOutlinePass _pass;

    public override void Create()
    {
        if (FeatureSettings.OutlineMaterial == null)
        {
            _pass = null;
            return;
        }

        _pass = new SceneOutlinePass(FeatureSettings.OutlineMaterial, FeatureSettings.InjectionPoint);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (_pass == null || FeatureSettings.OutlineMaterial == null)
        {
            return;
        }

        renderer.EnqueuePass(_pass);
    }

    protected override void Dispose(bool disposing)
    {
        _pass?.Dispose();
        _pass = null;
    }
}