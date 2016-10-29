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

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;
using Mpdn.Extensions.Framework;

namespace Mpdn.Extensions.PlayerExtensions
{
    public class AssFilterModProvider : PlayerExtension
    {
        public override ExtensionUiDescriptor Descriptor
        {
            get
            {
                return new ExtensionUiDescriptor
                {
                    Guid = new Guid("7F64BAD0-CFD3-4CF5-8900-2ADFA01B8C9A"),
                    Name = "AssFilterMod",
                    Description = "Adds AssFilterMod to MPDN"
                };
            }
        }

        public override void Initialize()
        {
            Media.Loading += OnMediaLoading;
        }

        public override void Destroy()
        {
            Media.Loading -= OnMediaLoading;
        }

        private class AssFilterMod : CustomSourceFilter
        {
            [ComImport, Guid("8A3704F3-BE3B-4944-9FF3-EE8757FDBDA5")]
            private class AssFilterModSource { }

            [ComImport, Guid("B98D13E7-55DB-4385-A33D-09FD1BA26338")]
            private class LavSplitterSource { }

            public AssFilterMod(IGraphBuilder graph, string filename)
            {
                m_subFilter = (IBaseFilter) new AssFilterModSource();
                m_Splitter = (IBaseFilter) new LavSplitterSource();

                var fileSourceFilter = (IFileSourceFilter)m_Splitter;
                DsError.ThrowExceptionForHR(fileSourceFilter.Load(filename, null));
                DsError.ThrowExceptionForHR(graph.AddFilter(m_Splitter, "LAV Splitter Source"));
                DsError.ThrowExceptionForHR(graph.AddFilter(m_subFilter, "AssFilterMod"));

                try
                {
                    var subs = DsFindPin.ByName(m_Splitter, "Subtitle");
                    ConnectPins(graph, subs, m_subFilter, "Input0");

                    VideoOutputPin = DsFindPin.ByName(m_Splitter, "Video");
                    AudioOutputPin = DsFindPin.ByName(m_Splitter, "Audio");
                    SubtitleOutputPins = GetPins(m_subFilter, "Input0");

                    VideoStreamSelect = (IAMStreamSelect)m_Splitter;
                    AudioStreamSelect = (IAMStreamSelect)m_Splitter;
                    SubtitleStreamSelect = (IAMStreamSelect)m_Splitter;
                }
                catch(Exception ex)
                {
                    Trace.WriteLine(ex.StackTrace);
                    Trace.WriteLine(ex.Message);
                }
            }

            private readonly IBaseFilter m_subFilter;
            private readonly IBaseFilter m_Splitter;
        }

        private static void OnMediaLoading(object sender, MediaLoadingEventArgs e)
        {
            var filename = e.Filename;

            e.CustomSourceFilter = graph =>
            {
                try
                {
                    return new AssFilterMod(graph, filename);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                    return null;
                }
            };
        }
    }
}
