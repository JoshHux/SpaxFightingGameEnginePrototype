Shader "Custom/Sprites/AC Character Shader"
{
    Properties
    {
       [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Tolerance("ReplaceTolerance", Range(0,1)) = 0.001


		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
        //_Color ("Tint", Color) = (1,1,1,1)
        _Cutoff ("Shadow alpha cutoff", Range(0,1)) = 0.5

	
		_Source1("Source Colour 1", Color) = (1,1,1,1)
		_Replace1("Replace Colour 1", Color) = (1,1,1,1)
		_Source2("Source Colour 2", Color) = (1,1,1,1)
		_Replace2("Replace Colour 2", Color) = (1,1,1,1)
		_Source3("Source Colour 3", Color) = (1,1,1,1)
		_Replace3("Replace Colour 3", Color) = (1,1,1,1)
		_Source4("Source Colour 4", Color) = (1,1,1,1)
		_Replace4("Replace Colour 4", Color) = (1,1,1,1)
		_Source5("Source Colour 5", Color) = (1,1,1,1)
		_Replace5("Replace Colour 5", Color) = (1,1,1,1)
		_Source6("Source Colour 6", Color) = (1,1,1,1)
		_Replace6("Replace Colour 6", Color) = (1,1,1,1)
		_Source7("Source Colour 7", Color) = (1,1,1,1)
		_Replace7("Replace Colour 7", Color) = (1,1,1,1)
		_Source8("Source Colour 8", Color) = (1,1,1,1)
		_Replace8("Replace Colour 8", Color) = (1,1,1,1)
		_Source9("Source Colour 9", Color) = (1,1,1,1)
		_Replace9("Replace Colour 9", Color) = (1,1,1,1)
		_Source10("Source Colour 10", Color) = (1,1,1,1)
		_Replace10("Replace Colour 10", Color) = (1,1,1,1)
		_Source11("Source Colour 11", Color) = (1,1,1,1)
		_Replace11("Replace Colour 11", Color) = (1,1,1,1)
		_Source12("Source Colour 12", Color) = (1,1,1,1)
		_Replace12("Replace Colour 12", Color) = (1,1,1,1)
		_Source13("Source Colour 13", Color) = (1,1,1,1)
		_Replace13("Replace Colour 13", Color) = (1,1,1,1)
		_Source14("Source Colour 14", Color) = (1,1,1,1)
		_Replace14("Replace Colour 14", Color) = (1,1,1,1)
		_Source15("Source Colour 15", Color) = (1,1,1,1)
		_Replace15("Replace Colour 15", Color) = (1,1,1,1)
		_Source16("Source Colour 16", Color) = (1,1,1,1)
		_Replace16("Replace Colour 16", Color) = (1,1,1,1)
		_Source17("Source Colour 17", Color) = (1,1,1,1)
		_Replace17("Replace Colour 17", Color) = (1,1,1,1)
		_Source18("Source Colour 18", Color) = (1,1,1,1)
		_Replace18("Replace Colour 18", Color) = (1,1,1,1)
		_Source19("Source Colour 19", Color) = (1,1,1,1)
		_Replace19("Replace Colour 19", Color) = (1,1,1,1)
		_Source20("Source Colour 20", Color) = (1,1,1,1)
		_Replace20("Replace Colour 20", Color) = (1,1,1,1)
		_Source21("Source Colour 21", Color) = (1,1,1,1)
		_Replace21("Replace Colour 21", Color) = (1,1,1,1)
		_Source22("Source Colour 22", Color) = (1,1,1,1)
		_Replace22("Replace Colour 22", Color) = (1,1,1,1)
		_Source23("Source Colour 23", Color) = (1,1,1,1)
		_Replace23("Replace Colour 23", Color) = (1,1,1,1)
		_Source24("Source Colour 24", Color) = (1,1,1,1)
		_Replace24("Replace Colour 24", Color) = (1,1,1,1)


    }

    //public ChangeColors{
   // _Replace1("Replace Colour 1", Color) = _Source1("Source Colour 1", Color);


     SubShader
	{
		    Tags
        {
          "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
 
       Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma fragment frag
			#pragma vertex vert
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ UNITY_ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			#include "PixelArtLibrary.cginc"
			#include "ColorSwapLibrary.cginc"

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;;

				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap(OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 o = SampleSpriteTexture(IN.texcoord);

				o.rgb = ReplaceFirstTwentyFourColors(o.rgb);				
				o *= IN.color; // multiply with source color from SpriteRenderer etc.
				o.rgb *= o.a;

				return o;
			}
		ENDCG
		}
	}

    SubShader
    {
        Tags
        {
            "Queue"="AlphaTest"
            "IgnoreProjector"="True"
            "RenderType"="TransparentCutout"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
 
        LOD 200
        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha
 
        CGPROGRAM
        #pragma surface surf Lambert nofog nolightmap nodynlightmap keepalpha noinstancing addshadow alphatest:_Cutoff
   #pragma multi_compile _ PIXELSNAP_ON
        #pragma multi_compile _ ETC1_EXTERNAL_ALPHA


        sampler2D _MainTex;
        fixed4 _Color;
 
        struct Input
        {
            float2 uv_MainTex;
        };

 
        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }

   
 
    Fallback "Legacy Shaders/Transparent/Cutout/VertexLit"
    CustomEditor "AC_CharacterShader_Editor_Script"
}