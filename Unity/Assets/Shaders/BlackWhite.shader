Shader "Custom/Diffuse_BlackWhite" {
	Properties {
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

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = lerp (c.rgb,(c.r + c.g + c.b) /3.0 ,_BWtoColor);
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
