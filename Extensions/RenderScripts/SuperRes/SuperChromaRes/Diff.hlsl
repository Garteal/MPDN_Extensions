// This file is a part of MPDN Extensions.
// https://github.com/zachsaw/MPDN_Extensions
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3.0 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library.
// 
// -- Misc --
sampler s0 : register(s0);
sampler sUV : register(s1);
float4 p0 :  register(c0);
float2 p1 :  register(c1);
float4 args0 : register(c2);

// -- Colour space Processing --
#include "../../Common/ColourProcessing.hlsl"
#define Kb args0[0] //redefinition
#define Kr args0[1] //redefinition

// -- Main code --
float4 main(float2 tex : TEXCOORD0) : COLOR {
	float4 c0 = tex2D(s0, tex);
	float4 chroma = tex2D(sUV, tex);

	c0.rgb = Gamma(c0.rgb);

	return float4(c0.rgb - chroma.rgb, 0);
}
