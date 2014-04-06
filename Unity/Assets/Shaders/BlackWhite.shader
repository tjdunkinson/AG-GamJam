Shader "Custom/Diffuse_BlackWhite" {
	Properties {
		_ColorToChange ("Color to change", color) = (1.0,1.0,1.0,1.0)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BWtoColor ("Grayscale to Color", Range (1,0)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		float _BWtoColor;
		float4 _ColorToChange;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			half3 BW = (c.r + c.g + c.b) /3.0;
			half3 colorToBW = lerp (c.rgb,(c.r + c.g + c.b) /3.0 ,_BWtoColor);
			half cD = lerp (BW, 1, 1 - c.a);
			half3 finalC = lerp ( colorToBW, (_ColorToChange * 0.75) * cD, c.a);
			o.Emission =  finalC;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
