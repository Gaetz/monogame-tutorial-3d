using Microsoft.Xna.Framework.Content.Pipeline;
using System.Collections.Generic;
using System.IO;

using TImport = System.String;

namespace Tutorial_Pipeline
{
    [ContentImporter(".json", DisplayName = "WavesImporter", DefaultProcessor = "WavesProcessor")]
    public class WavesImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            string jsonData = File.ReadAllText(filename);
            return jsonData;
        }
    }

    public class Level
    {
        public int id;
        public List<Wave> waves = new List<Wave>();
    }

    public class Wave
    {
        public float time;
        public int id;
        public List<WaveElement> elements = new List<WaveElement>();
    }

    public class WaveElement
    {
        public string type;
        public ScreenSide screenSideEnter;
        public ScreenSide screenSideExit;
        public float x;
        public float y;
        public float z;
        public float mainPhaseDuration;
    }

    public enum ScreenSide
    {
        Top,
        Bottom,
        Left,
        Right,
        Horizon,
        Back
    }
}
