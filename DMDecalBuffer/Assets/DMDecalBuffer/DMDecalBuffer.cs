using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DMDecalBuffer:ScriptableRenderPass {
    DMDecalBufferFeature.PassSettings passSettings;

    List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>();
    FilteringSettings m_FilteringSettings;
    RenderStateBlock m_RenderStateBlock;


    RenderTargetIdentifier colorBuffer, temporaryBuffer;
    int temporaryBufferID;

    public DMDecalBuffer(DMDecalBufferFeature.PassSettings passSettings) {
        this.passSettings = passSettings;
        temporaryBufferID = Shader.PropertyToID(passSettings.TextureName);

        renderPassEvent = passSettings.renderPassEvent;

        m_ShaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));
        m_ShaderTagIdList.Add(new ShaderTagId("UniversalForward"));
        m_ShaderTagIdList.Add(new ShaderTagId("LightweightForward"));

        m_FilteringSettings = new FilteringSettings(RenderQueueRange.all, passSettings.LayerMask);
        m_RenderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData) {
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
        
        //could downscale things here if you wanted, maybe depending on performance might be okay 

        ConfigureInput(ScriptableRenderPassInput.Depth);
        cmd.GetTemporaryRT(temporaryBufferID, descriptor, FilterMode.Bilinear);
        temporaryBuffer = new RenderTargetIdentifier(temporaryBufferID);
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor) {
        //cmd.SetRenderTarget(temporaryBufferID);
        cmd.name = "---DMCustomRenderBuffer---";
        ConfigureTarget(temporaryBufferID);
        ConfigureClear(ClearFlag.All, Color.clear);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {
        CommandBuffer cmd = CommandBufferPool.Get();

        //I have no idea what I"m doing ᕕ( ᐛ )ᕗ

        DrawingSettings drawSettings;
        drawSettings = CreateDrawingSettings(m_ShaderTagIdList, ref renderingData, SortingCriteria.CommonTransparent);
        context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref m_FilteringSettings, ref m_RenderStateBlock);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd) {
        if(cmd == null) throw new ArgumentNullException("cmd");
        cmd.ReleaseTemporaryRT(temporaryBufferID);
    }
}