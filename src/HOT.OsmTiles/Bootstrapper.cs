// The MIT License (MIT)

// Copyright (c) 2016 Ben Abelshausen

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using HOT.OsmTiles.Tiles;
using OsmSharp.Streams;
using System;
using System.IO;

namespace HOT.OsmTiles
{
    /// <summary>
    /// Keeps static info around and initializes stuff.
    /// </summary>
    public static class Bootstrapper
    { 
        /// <summary>
        /// Bootstraps this service.
        /// </summary>
        public static void Start(string dataPath)
        {
            Bootstrapper.DataPath = dataPath;

            Bootstrapper.GetData = (stream, box, file) =>
            {
                var fullFilePath = Path.Combine(dataPath, file);
                if (!File.Exists(fullFilePath))
                {
                    throw new FileNotFoundException();
                }
                using (var inputStream = File.Open(fullFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    OsmStreamSource source;
                    if (fullFilePath.EndsWith("osm"))
                    {
                        source = new XmlOsmStreamSource(inputStream);
                    }
                    else if(fullFilePath.EndsWith("osm.pbf"))
                    {
                        source = new PBFOsmStreamSource(inputStream);
                    }
                    else
                    {
                        throw new NotSupportedException("File need to be an OSM-XML or OSM-PBF file.");
                    }

                    var filtered = source.FilterBox(box.Left, box.Top, box.Right, box.Bottom, true);

                    var target = new XmlOsmStreamTarget(stream);
                    target.RegisterSource(filtered);
                    target.Pull();
                }
            };
        }

        /// <summary>
        /// Gets or sets the path to the data to serve.
        /// </summary>
        public static string DataPath { get; set; }

        /// <summary>
        /// Gets or sets a function to get data into the given stream for the given box and filename.
        /// </summary>
        public static Action<Stream, Box, string> GetData
        {
            get;
            set;
        }
    }
}