
sampler2D _MainTex;
sampler2D _AlphaTex;
fixed4 _Color;
float _pixelsPerUnit;
float _AlphaSplitEnabled;

/*
	TYPICAL STRUCTS
*/

struct appdata_t
{
	float4 vertex   : POSITION;
	float4 color    : COLOR;
	float2 texcoord : TEXCOORD0;
};

struct v2f
{
	float4 vertex   : SV_POSITION;
	fixed4 color : COLOR;
	float2 texcoord  : TEXCOORD0;
};

/*
	PIXELART GRID
*/

float4 AlignToPixelGrid(float4 vertex)
{
	float4 worldPos = mul(unity_ObjectToWorld, vertex);

	float offset = 0.5 / _pixelsPerUnit;
	worldPos.x = floor(worldPos.x * _pixelsPerUnit) / _pixelsPerUnit + offset;
	worldPos.y = floor(worldPos.y * _pixelsPerUnit + 0.5) / _pixelsPerUnit;

	return mul(unity_WorldToObject, worldPos);
}

/*
	OUTLINES AND TINTS
*/

fixed4 SampleSpriteTextureTinted(float2 uv, float4 tint)
{
	fixed4 color = tex2D(_MainTex, uv);

#if UNITY_ETC1_EXTERNAL_ALPHA
	if (_AlphaSplitEnabled)
		color.a = tex2D(_AlphaTex, uv).r;
#endif //UNITY_ETC1_EXTERNAL_ALPHA

	fixed4 tintedColor = tint;
	tintedColor.a = color.a * tint.a;

	return tintedColor;
}

/*
	SAMPLING
*/

// from the Standard Unity Sprite Shader
fixed4 SampleSpriteTexture(float2 uv)
{
	fixed4 color = tex2D(_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
	// get the color from an external texture (usecase: Alpha support for ETC1 on android)
	color.a = tex2D(_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA				

	return color;
}