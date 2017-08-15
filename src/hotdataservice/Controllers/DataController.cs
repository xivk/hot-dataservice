using Microsoft.AspNetCore.Mvc;
using System.IO;
using OsmSharp.Streams;
using hotdataservice.Tiles;

namespace hotdataservice.Controllers
{
    [Route("[controller]")]
    public class DataController : Controller
    {
        // GET data/{x}/{y}/{z}/{file}
        [HttpGet("{x}/{y}/{z}/{file}")]
        public IActionResult Get(int x, int y, int z, string file)
        {
            var completeFilePath = Path.Combine(Startup.DataFolder, file);
            var fileInfo = new FileInfo(completeFilePath);
            if (!fileInfo.Exists)
            {
                return new NotFoundResult();
            }

            var resultStream = new MemoryStream();
            using (var stream = fileInfo.OpenRead())
            {
                OsmStreamSource source;
                if (fileInfo.Name.EndsWith(".osm.pbf"))
                {
                    source = new PBFOsmStreamSource(stream);
                }
                else if (fileInfo.Name.EndsWith(".osm"))
                {
                    source = new XmlOsmStreamSource(stream);
                }
                else
                {
                    return new NotFoundResult();
                }

                // parse the zoom, x and y.
                var tile = new Tile(x, y, z);
                tile = tile.InvertY();
                var filter = source.FilterBox(tile.Left, tile.Top, tile.Right, tile.Bottom,
                    true);

                var target = new XmlOsmStreamTarget(resultStream);
                target.RegisterSource(filter);
                target.Pull();

                resultStream.Seek(0, SeekOrigin.Begin);
            }
            return new FileStreamResult(resultStream, "text/xml");
        }
    }
}
