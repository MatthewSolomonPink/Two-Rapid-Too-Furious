Shader "Custom/UnlitShader"
{
    Properties
    {
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        [MainColor] _Color("Color", Color) = (1,1,1,1)
        _Hatch0("Hatch 0", 2D) = "white" {}
		_Hatch1("Hatch 1", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            // #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"



            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv: TEXCOORD0;
                float3 norm: NORMAL;
            };

            struct Varyings
            {
                float4 positionCS  : SV_POSITION;
                float2 uv: TEXCOORD0;
                float3 nrm: TEXCOORD1;
                float3 wPos: TEXCOORD2;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
            float4 _Color;
            float4 _BaseMap_ST;
            sampler2D _Hatch0;
			sampler2D _Hatch1;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
				OUT.nrm = mul(float4(IN.norm, 0.0), GetWorldToObjectMatrix()).xyz;
                OUT.wPos = mul(GetObjectToWorldMatrix(), IN.positionOS).xyz;
                return OUT;
            }

            half3 HatchingConstantScale(float2 _uv, half _intensity, float _dist) // _dist is distance from camera, multiplied by unity_CameraInvProjection[0][0]
			{
				float log2_dist = log2(_dist);
				
				float2 floored_log_dist = floor( (log2_dist + float2(0.0, 1.0) ) * 0.5) *2.0 - float2(0.0, 1.0);				
				float2 uv_scale = min(1, pow(2.0, floored_log_dist));
				
				float uv_blend = abs(frac(log2_dist * 0.5) * 2.0 - 1.0);
				
				

				float2 scaledUVA = _uv / uv_scale.x; // 16
				float2 scaledUVB = _uv / uv_scale.y; // 8 

				half3 hatch0A = tex2D(_Hatch0, scaledUVA).rgb;
				half3 hatch1A = tex2D(_Hatch1, scaledUVA).rgb;

				half3 hatch0B = tex2D(_Hatch0, scaledUVB).rgb;
				half3 hatch1B = tex2D(_Hatch1, scaledUVB).rgb;

				half3 hatch0 = lerp(hatch0A, hatch0B, uv_blend);
				half3 hatch1 = lerp(hatch1A, hatch1B, uv_blend);

				half3 overbright = max(0, _intensity - 1.0);

				half3 weightsA = saturate((_intensity * 6.0) + half3(-0, -1, -2));
				half3 weightsB = saturate((_intensity * 6.0) + half3(-3, -4, -5));

				weightsA.xy -= weightsA.yz;
				weightsA.z -= weightsB.x;
				weightsB.xy -= weightsB.yz;

				hatch0 = hatch0 * weightsA;
				hatch1 = hatch1 * weightsB;

				half3 hatching = overbright + hatch0.r +
					hatch0.g + hatch0.b +
					hatch1.r + hatch1.g +
					hatch1.b;

				return hatching;
			}

            half3 Hatching(float2 _uv, half _intensity)
			{
				half3 hatch0 = tex2D(_Hatch0, _uv).rgb;
				half3 hatch1 = tex2D(_Hatch1, _uv).rgb;

				half3 overbright = max(0, _intensity - 1.0);

				half3 weightsA = saturate((_intensity * 6.0) + half3(-0, -1, -2));
				half3 weightsB = saturate((_intensity * 6.0) + half3(-3, -4, -5));

				weightsA.xy -= weightsA.yz;
				weightsA.z -= weightsB.x;
				weightsB.xy -= weightsB.yz;

				hatch0 = hatch0 * weightsA;
				hatch1 = hatch1 * weightsB;

				half3 hatching = overbright + hatch0.r +
					hatch0.g + hatch0.b +
					hatch1.r + hatch1.g +
					hatch1.b;

				return hatching;
			}

            half4 frag(Varyings IN) : SV_Target
            {
                float4 texel = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
				float3 diffuse = _Color * _MainLightColor * dot(_MainLightPosition, normalize(IN.nrm));

                float intensity = dot(diffuse, float3(0.2326, 0.7152, 0.0722));
                return float4(HatchingConstantScale(IN.uv * 3, intensity, distance(_WorldSpaceCameraPos.xyz, IN.wPos) * unity_CameraInvProjection[0][0]), 1);
            }
            ENDHLSL
        }
    }
}