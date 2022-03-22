Shader "zFrame/Video/Transparent" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
		_TransVal("Transparency Value", Range(0,10)) = 1.0
		_Color("_Color",Color) = (1,1,1,1)
		_Emission("_Emission",float) = 1
	}
		SubShader{
			Tags { "RenderType"="Opaque" "IgnoreProjector"="True" "Queue"="Transparent"}  
			LOD 200
			cull front
			CGPROGRAM
		#pragma surface surf Lambert  alpha 
		sampler2D _MainTex;
		float4  _Color;
		float _TransVal;
		float _Emission;
		struct Input {
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			half4 h = half4(0, 0, 0, 1);
			if (IN.uv_MainTex.x >= 0.5)
			{
				h.a = 0;
			}
			else
			{
				half4 he = tex2D(_MainTex, half2(IN.uv_MainTex.x + 0.5, IN.uv_MainTex.y));
				h.a = (he.r + he.g + he.b)*_TransVal;
				if(h.a>1)
				{
					h.a =1;	
				}
				half4 c = tex2D(_MainTex, IN.uv_MainTex);
				//o.Albedo = c.rgb;
				o.Emission = c*_Emission*_Color;
			}
			o.Alpha = h.a;
		}

		ENDCG

		cull back
		CGPROGRAM

		#pragma surface surf Lambert alpha  

		sampler2D _MainTex;
		float4  _Color;
		float _TransVal;
		float _Emission;
		struct Input {
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			half4 h = half4(0, 0, 0, 1);
			if (IN.uv_MainTex.x >= 0.5)
			{
				h.a = 0;
			}
			else
			{
				half4 he = tex2D(_MainTex, half2(IN.uv_MainTex.x + 0.5, IN.uv_MainTex.y));
				h.a = (he.r + he.g + he.b)*_TransVal;
				if(h.a>1)
				{
					h.a =1;	
				}
				half4 c = tex2D(_MainTex, IN.uv_MainTex);
				
				o.Emission = c*_Color*_Emission;
			}

			o.Alpha = h.a;
		}
		ENDCG
	}
		 
    

}