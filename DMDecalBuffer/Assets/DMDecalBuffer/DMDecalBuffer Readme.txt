Using Unity 2021.1.25f1 and URP 11

Steps:
- Create a new unity layer and name it "DecalBuffer". All objects placed in this layer will be rendered by our special render pass into our buffer. 
- Make sure your forward renderer quality profiles all have "Depth Texture" enabled, as decals need this to be projected onto surfaces
- On your forward renderer Opaque Layer Mask and Transparent Layer Make, make sure your new "DecalBuffer" layer is NOT checked.  We do not want to render objects in these layers except to our offscreen buffer, so if they are checked here they will render twice!
- Add a new DMDecalBufferFeature to your Forward Renderer
- Render Pass Event should be BeforeOpaques
- Texture name will be the name of the global shader you will sample in shader, feel free to change
- Layer Mask, set this to "DecalBuffer" you created in step 1. This is where our objects render

- SETUP COMPLETE

- Now to test. See the demo scene. 
You need to create a cube with a shader on it (DM/DecalBuffer/Texture, for example) and it will project the texture as a decal and render it to the custom offscreen buffer
Next, you need to PULL in this data and sample it in the object surface's shader and use it to control whatever properties you want. Shaders are made with ASE so open them up and view them or use the DM/DecalBuffer/Debug shader on a material to visualize the buffer!

HAVE FUN

Questions, or PR welcome.  Tweet me @DMeville
