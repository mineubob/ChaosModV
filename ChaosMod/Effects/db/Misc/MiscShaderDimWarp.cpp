#include <stdafx.h>

#include "Memory/Hooks/ShaderHook.h"

static const char* ms_szShaderSrc = R"SRC(
Texture2D HDRSampler : register(t5);
SamplerState g_samLinear : register(s5)
{
    Filter = MIN_MAG_MIP_LINEAR;
    AddressU = Wrap;
    AddressV = Wrap;
};

float4 main(float4 position	: SV_POSITION, float3 texcoord : TEXCOORD0, float4 color : COLOR0) : SV_Target0
{
	texcoord.x = saturate(texcoord.x % (0.5 - texcoord.x));
    float4 col = HDRSampler.Sample(g_samLinear, texcoord);

    return col;
}
)SRC";

static void OnStart()
{
    Hooks::OverrideShader(EOverrideShaderType::Snow, ms_szShaderSrc);
}

static void OnStop()
{
    Hooks::ResetShader();
}

static RegisterEffect registerEffect(OnStart, OnStop, EffectInfo
	{
		.Name = "Dimension Warp",
		.Id = "misc_dimwarp",
		.IsTimed = true,
		.IsShortDuration = true,
		.EffectCategory = EEffectCategory::Shader,
		.EffectGroupType = EEffectGroupType::Shader
	}
);