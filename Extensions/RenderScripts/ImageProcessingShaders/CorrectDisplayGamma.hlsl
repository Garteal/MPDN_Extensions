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
sampler s0 : register(s0);
float4  p0 : register(c0);
float2  p1 : register(c1);

#define width  (p0[0])
#define height (p0[1])

#define px (p1[0])
#define py (p1[1])

// Change the following (target_gamma) to match your display's gamma correction
#define target_gamma 2.4

// Do not change the following (base_gamma) value
#define base_gamma 2.2

float4 main(float2 tex : TEXCOORD0) : COLOR 
{
    float4 c0 = tex2D(s0, tex);
    c0.rgb = pow(saturate(c0.rgb), 1.0/(1.0+(target_gamma-base_gamma)/base_gamma));
    return c0;
}
