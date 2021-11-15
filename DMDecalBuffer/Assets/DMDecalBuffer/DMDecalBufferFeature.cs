using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DMDecalBufferFeature:ScriptableRendererFeature {
    [System.Serializable]
    public class PassSettings {
        //when to inject the pass
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
        //name of the texture you can grab in shaders
        public string TextureName = "_CustomRenderBuffer";
        //only renders objects in the layers below
        public LayerMask LayerMask;
    }

    DMDecalBuffer pass;
    public PassSettings passSettings = new PassSettings();

    public override void Create() {
        pass = new DMDecalBuffer(passSettings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
        renderer.EnqueuePass(pass);
    }
}