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

using System.IO;
using Mpdn.Extensions.Framework.Chain;
using Mpdn.Extensions.Framework.Chain.Dialogs;
using Mpdn.Extensions.Framework.RenderChain.Shader;
using Mpdn.OpenCl;
using Mpdn.RenderScript;

namespace Mpdn.Extensions.Framework.RenderChain
{
    public abstract class RenderChain : FilterChain<ITextureFilter>
    {
        protected RenderChain()
        {
            ShaderCache.Load();
        }

        #region Shader Compilation

        protected virtual string ShaderPath
        {
            get { return GetType().Name; }
        }

        protected string ShaderDataFilePath
        {
            get { return Path.Combine(ShaderCache.ShaderPathRoot, ShaderPath); }
        }

        protected ShaderFromFile FromFile(string shaderFileName, string profile = "ps_3_0", string entryPoint = "main", string macroDefinitions = null)
        {
            return new ShaderFromFile(Path.Combine(ShaderDataFilePath, shaderFileName), profile, entryPoint, macroDefinitions);
        }

        protected ShaderFromByteCode FromByteCode(string bytecodeFileName)
        {
            return new ShaderFromByteCode(Path.Combine(ShaderDataFilePath, bytecodeFileName));
        }

        protected ShaderFromString FromString(string shadercode, string profile = "ps_3_0", string entryPoint = "main", string macroDefinitions = null)
        {
            return new ShaderFromString(shadercode, profile, entryPoint, macroDefinitions);
        }

        protected IShaderFilterSettings<IShader> CompileShader(string shaderFileName, string profile = "ps_3_0", string entryPoint = "main", string macroDefinitions = null)
        {
            return ShaderCache.CompileShader(Path.Combine(ShaderDataFilePath, shaderFileName), profile, entryPoint, macroDefinitions).Configure(name: shaderFileName);
        }

        protected IShaderFilterSettings<IShader11> CompileShader11(string shaderFileName, string profile, string entryPoint = "main", string macroDefinitions = null)
        {
            return ShaderCache.CompileShader11(Path.Combine(ShaderDataFilePath, shaderFileName), profile, entryPoint, macroDefinitions).Configure(name: shaderFileName);
        }

        protected IShaderFilterSettings<IKernel> CompileClKernel(string sourceFileName, string entryPoint, string options = null)
        {
            return ShaderCache.CompileClKernel(Path.Combine(ShaderDataFilePath, sourceFileName), entryPoint, options).Configure(name: sourceFileName);
        }

        protected IShaderFilterSettings<IShader> CompileShaderFromString(string code, string profile = "ps_3_0", string entryPoint = "main", string macroDefinitions = null)
        {
            return Renderer.CompileShaderFromString(code, entryPoint, profile, macroDefinitions).Configure();
        }

        protected IShaderFilterSettings<IShader11> CompileShader11FromString(string code, string profile, string entryPoint = "main", string macroDefinitions = null)
        {
            return Renderer.CompileShader11FromString(code, entryPoint, profile, macroDefinitions).Configure();
        }

        protected IShaderFilterSettings<IKernel> CompileClKernelFromString(string code, string entryPoint, string options = null)
        {
            return Renderer.CompileClKernelFromString(code, entryPoint, options).Configure();
        }

        protected IShaderFilterSettings<IShader> LoadShader(string shaderFileName)
        {
            return ShaderCache.LoadShader(Path.Combine(ShaderDataFilePath, shaderFileName)).Configure(name: shaderFileName);
        }

        protected IShaderFilterSettings<IShader11> LoadShader11(string shaderFileName)
        {
            return ShaderCache.LoadShader11(Path.Combine(ShaderDataFilePath, shaderFileName)).Configure(name: shaderFileName);
        }

        #endregion
    }

    public class RenderScriptChain : ScriptChain<ITextureFilter, IRenderScript> { }
    public class RenderScriptChainDialog : ScriptChainDialog<ITextureFilter, IRenderScript> { }

    public class RenderScriptGroup : ScriptGroup<ITextureFilter, IRenderScript> { }
    public class RenderScriptGroupDialog : ScriptGroupDialog<ITextureFilter, IRenderScript> { }
}