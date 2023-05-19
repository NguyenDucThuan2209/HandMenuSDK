Shader "[CustomShader]/Resolve"
{
    // This is the place you declare your variables that used for your shader, which will also show on the material inspector
    Properties
    {
        // Create a base texture as a property for this shader
        _BaseColor("Base Color", COLOR) = (0,0,0,0)
        
        // Create a erosion texture as a property for this shader*/
        _ErosionTexture("Erosion Texture", 2D) = "white" {}

        // Create two Blend Mode properties for SourceBlendRatio and DestinationBlendRatio
        // Currently, I'm set the _SourceBlendRatio default value is 5 (which is SrcAlpha) and _DestinationBlendRatio default value is 10 (which is OneMinusSrcAlpha)
        [Enum(UnityEngine.Rendering.BlendMode)]
        _SourceBlendRatio("Source Blend Mode", float) = 5

        [Enum(UnityEngine.Rendering.BlendMode)]
        _DestinationBlendRatio("Destination Blend Mode", float) = 10

        // Create a Blend Operation Mode for BlendOp in the BLEND FORMULA
        [Enum(UnityEngine.Rendering.BlendOp)]
        _BlendOp("Blend Operation Mode", float) = 0

        // Create an AlphaThreshold property to manipulate the Erosion effect
        _AlphaThreshold("Alpha Threshold", float) = 1.0

        // Create a SmoothStep property to add smoothly to the Erosion effect
        _SmoothStep("Smooth Step", Range(0.0, 0.1)) = 0.1

        /* NEW: Create an ErosionColor property to add it into the zone of the Erosion effect*/
        _ErosionColor("Erosion Color", COLOR) = (1,1,1,1)

        /* NEW: Create a BurnColor property to add it into the border of the Erosion effect*/
        _BurnColor("Burn Color", COLOR) = (1,1,1,1)

        /* NEW: Create a _ErosionPower property to increase the _ErosionColor power when Erosion effect happening */
        _ErosionPower("Burn Power", float) = 1
    }

    // This is the place that we will write most of the shader's parts
    SubShader
    {
        // This Tags will tell unity to give the user different types of properties to adjust shader
        // When we set render type into Transparent, it won't be enough for our material to have transparency
        // This is just a way to tag our material as Transparent, not a completely way to do it
        Tags { "RenderType" = "Transparent" }

        // This will set that the <Pass> below will be used when the object that rendering with this shader is at 100 LevelOfDetail(LOD) 
        LOD 100

        // DECLARE THE NEW RATIO-FACTOR USED FOR BLEND FORMULA (See more in Lesson3.shader)          
        Blend [_SourceBlendRatio] [_DestinationBlendRatio]

        // DECLARE THE NEW BLEND-OPERATION USED IN BLEND FORMULA (See more in Lesson3.shader)
        BlendOp [_BlendOp]
        
        // BLEND FORMULA STRUCTURE (See more in Lesson3.shader)

        Pass
        {
            // This is the part that we will write our CGPROGRAM. It's always start with CGPROGRAM and end with ENDCG
            CGPROGRAM

            // This is the part that used to declare Vertex shader and Fragment shader
            #pragma vertex vert
            #pragma fragment frag                        

            // This will include main functions, properties from Unity that we usually used
            #include "UnityCG.cginc"
            
            // *appdata is all of the data that we can get from a mesh (that could be vertex, uv, tangents, normals, color, etc)
            // This part will declare a struct of appdata that we will use to calculate in our shader
            // Like the default struct below we will have vertex and uv data from the mesh that our shader are rendering
            struct appdata
            {
                float4 vertex : POSITION;

                // Declare uv as a new field for appdata and assign it value as TEXCOORD0
                float2 uv     : TEXCOORD0;
            };

            // *v2f is short cut of vertex to fragment 
            // This part will declare a struct which we will use to pass the vertex data that we have processed to the fragment render function
            struct v2f
            {
                // SV_POSITION (shortcut of System Value Position) is the way to call POSITION property that will ensure every devices will allow our shader to read it
                float4 vertex : SV_POSITION; 

                /* Declare two UV on a TEXCOORD_INDEX at the same time
                    => On many devices, we can use only a few or limited of TEXCOORD_INDEX to use.
                    => To optimize this limited-memory, we can assign two UV value on a same TEXCOORD_INDEX at the same time
                    => To do this, we will store the first uv map on X & Y (or R & G) channels and the secondary uv map will be store on the Z & W (or B & W)*/
                float2 erosionUV : TEXCOORD0;            
            };

            // Declare _BaseColor in CG Program zone so that this SubShader will recognize it as _BaseColor from the Properties that we declare before
            fixed4 _BaseColor;

            // Declare _ErosionTexture in CG Program zone so that this SubShader will recognize it as _ErosionTexture from the Properties that we declare before
            sampler2D _ErosionTexture;

            // Declare _ErosionTexture_ST property(MUST HAVE "_ST") so that this SubShader will recognize it as _ErosionTexture's Scale and Transform
            float4 _ErosionTexture_ST;

            // Declare _AlphaThreshold so that this SubShader will recognize it as _AlphaThreshold from the Properties that we declare before
            float _AlphaThreshold;

            // Declare _SmoothStep so that this SubShader will recognize it as _SmoothStep from the Properties that we declare before
            float _SmoothStep;

            /* NEW: Declare _ErosionColor so that this SubShader will recognize it as _ErosionColor from the Properties that we declare before */
            float4 _ErosionColor;
            
            /* NEW: Declare _BurnColor so that this SubShader will recognize it as _BurnColor from the Properties that we declare before */
            float4 _BurnColor;

            /* NEW: Declare _ErosionPower so that this SubShader will recognize it as _ErosionPower from the Properties that we declare before */
            float _ErosionPower;

            // This part is the Vertex shader function. This function will run on every single vertices of the input mesh
            // This function's input will be the appdata (which is the input come from the object's mesh)
            v2f vert(appdata i)
            {
                // This is declare the output of our Vertex shader function
                v2f o;
                
                // When we call v.vertex, this vertex is in the object space
                // So when we calculate a vertex and send it to the Fragment shader function, that vertex will need to be in the World space
                // *UnityObjectToClipPos function actually doing a matrix multiplier. It will take the vertex from object space into the right space in camera (after applied Camera's projection math)
                o.vertex = UnityObjectToClipPos(i.vertex);

                /* We can use the combineUV property to reduce the number of TEXCOORD_INDEX we use in each shader
                    => As we note before, the first UV will be save on X and Y fields, and the secondary UV will be save on the Z and W fields*/
                o.erosionUV = TRANSFORM_TEX(i.uv, _ErosionTexture);

                return o;
            }

            // This part is Fragment shader function. This function will run on every single pixels of the output screen 
            // (this is the reason why the FS is also called as Pixel Shader)
            // This function will take the Vertex shader function's output as its input
            // *SV_Target is the way to tell that we will only return a single color as output
            fixed4 frag(v2f i) : SV_Target
            {
                // Use the combineUV property of v2f to assign value for the pixel at the input UV
                fixed4 erosionTextureAtUV = tex2D(_ErosionTexture, i.erosionUV);

                /* NEW: Calculate the ErosionRatio and BurnRatio to manipulate the pixel color between ErosionColor and BurnColor
                    + ErosionRatio: erosion ratio is depend on the erosion texture value, smoothstep value and the alpha threshold value
                        -> ErosionRatio is only 0 or 1.
                        -> It depend on that the saturate of subtraction between erosionTexture value and smoothstep value is lower than the Alpha threshold or not
                        -> NOTICE: We can't use step function because its condition is >= which is wrong in this situation
                    + BurnRatio: burn ratio is depend on the erosion texture value range and the alpha threshold value
                        -> BurnRatio is in the range from 0 to 1.
                        -> It depend on how the AlphaThreshold value is proportionate with the saturate of the erosion texture value range
                    */                                                
                fixed3 erosionRatio = (saturate(erosionTextureAtUV - _SmoothStep) < _AlphaThreshold);
                fixed3 burnRatio = smoothstep(saturate(erosionTextureAtUV - _SmoothStep), saturate(erosionTextureAtUV + _SmoothStep), _AlphaThreshold);

                /* NEW: Calculate the ErosionColor and BurnColor to get the final color
                    + ErosionColor: erosion color is a lerp from baseTextureValue to the ErosionColor (base) depend on the erosionRatio
                        -> We can multiply the ErosionColor (base) with the ErosionPower to get the emitting-color effect
                    + BurnColor: burn color is a lerp from ErosionColor to the BurnColor (base) depend on the burnRatio
                        -> We will use burn color for the final color without addition editing because its has been blended with the baseTexture value and ErosionColor
                    */
                fixed3 erosionColor = lerp(_BaseColor, _ErosionColor * _ErosionPower, erosionRatio);
                fixed3 burnColor = lerp(erosionColor, _BurnColor, burnRatio);

                /* NEW: Calculate the final color for this pixel
                    + Color: will be equal to the BurnColor
                    + Alpha: will be equal to the subtraction between baseTexture value and the step:
                        1. When the AlphaThreshold is greater than or equal to the saturate of erosionTexture value plus smoothstep
                            -> This pixel's alpha will be 0 (transparent)
                        2. When the AlphaThreshold is lower than the saturate of erosionTexture value plus smoothstep
                            -> This pixel's alpha will be equal to the BaseTexture's alpha
                */
                fixed3 color = burnColor;
                fixed alpha = saturate(_BaseColor.a - step(erosionTextureAtUV + _SmoothStep, _AlphaThreshold));
                
                return fixed4(color, alpha);
            }
            
            ENDCG
        }
    }
}
