Shader "zFrame/Video/Default" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Emission("_Emission",float) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Transparent"}  
		LOD 200
		cull front
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert  alpha 
		sampler2D _MainTex;
		struct Input {
			float2 uv_MainTex;
		};	
		fixed4 _Color;
		float _Emission;
		void surf (Input IN, inout SurfaceOutput o) {
			
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Emission = c*_Color*_Emission;
			o.Alpha = c.a;
		}
		ENDCG

		cull back
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert  alpha 
		sampler2D _MainTex;
		struct Input {
			float2 uv_MainTex;
		};	
		fixed4 _Color;
		float _Emission;
		void surf (Input IN, inout SurfaceOutput o) {
			
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Emission = c*_Color*_Emission;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
