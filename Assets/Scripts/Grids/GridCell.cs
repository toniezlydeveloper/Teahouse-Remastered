using Newtonsoft.Json;

namespace Grids
{
    public class GridCell
    {
        [JsonProperty("c")] public int Column { get; set; }
        [JsonProperty("r")] public int Row { get; set; }
    }
}