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

using System;
using Nancy;
using System.Globalization;
using HOT.OsmTiles.Tiles;

namespace HOT.OsmTiles.Modules
{
    /// <summary>
    /// A module serving parts of OSM-files.
    /// </summary>
    public class FileModule : NancyModule
    {
        /// <summary>
        /// Creates a new file module.
        /// </summary>
        public FileModule()
        {
            Get("box/{file}", _ =>
            {
                return this.DoBox(_);
            });
            Get("/data/{x}/{y}/{z}/{file}", _ =>
            {
                return this.DoTile(_);
            });
        }

        /// <summary>
        /// Executes a tile request.
        /// </summary>
        private object DoTile(dynamic _)
        {
            // parse the zoom, x and y.
            int x, y, zoom;
            if (!int.TryParse(_.x, out x))
            {
                // invalid x.
                throw new InvalidCastException("Cannot parse x!");
            }
            if (!int.TryParse(_.y, out y))
            {
                // invalid y.
                throw new InvalidCastException("Cannot parse y!");
            }
            if (!int.TryParse(_.z, out zoom))
            {
                // invalid zoom.
                throw new InvalidCastException("Cannot parse zoom!");
            }

            // create the filter.
            var tile = new Tile(x, y, zoom);

            // invert the y-coordinate, system of HOT-tasking manager is inverted.
            tile = tile.InvertY();

            return new Response()
            {
                Contents = (stream) =>
                {
                    Bootstrapper.GetData(stream, new Box()
                    {
                        Bottom = tile.Bottom,
                        Left = tile.Left,
                        Right = tile.Right,
                        Top = tile.Top
                    }, _.file);
                }
            };
        }

        /// <summary>
        /// Executes a boundingbox request.
        /// </summary>
        private object DoBox(dynamic _)
        {
            // split arguments.
            var bounds = this.Request.Query.bbox.Split(',');
            if (bounds == null || bounds.Length != 4)
            {
                throw new ArgumentException(string.Format(
                    "Invalid request: bbox invalid: {0}!", this.Request.Query.bbox));
            }

            // parse bounds.
            float left = 0, bottom = 0, right = 0, top = 0;
            if (!float.TryParse(bounds[0], NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture,
                                out left) ||
                !float.TryParse(bounds[1], NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture,
                                out bottom) ||
                !float.TryParse(bounds[2], NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture,
                                out right) ||
                !float.TryParse(bounds[3], NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture,
                                out top))
            {
                throw new ArgumentException(string.Format(
                    "Invalid request: bbox invalid: {0}!", this.Request.Query.bbox));
            }

            return new Response()
            {
                Contents = (stream) =>
                {
                    Bootstrapper.GetData(stream, new Box()
                    {
                        Bottom = bottom,
                        Left = left,
                        Right = right,
                        Top = top
                    }, _.file);
                }
            };
        }
    }
}