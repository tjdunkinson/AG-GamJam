Shader "Custom/VertColour"
{
	Properties
	{
		_Ramp ("Shading Ramp", 2D) = "gray" {}
	}
	SubShader
	{
		Tags
		{
			"RenderType"="Opaque"
		}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Ramp
		
		sampler2D _Ramp;
		
		half4 LightingRamp (SurfaceOutput s, half3 lightDir, half atten)
		{
			half NdotL = dot (s.Normal, lightDir);
			half diff = NdotL * 0.5 + 0.5;
			half3 ramp = tex2D (_Ramp, float2(diff)).rgb;
			
			// Returne
			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
			c.a = s.Alpha;
			return c;
		}
		
		struct Input
		{
			float4 color : COLOR;
		};
		
		void surf (Input IN, inout SurfaceOutput o)
		{
			half3 VertColor = IN.color.rgb;
			o.Albedo = VertColor;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
