using System.Xml.Serialization;

namespace MonoGame.Extended.Tiled.Serialization
{
    public class TiledMapTilesetTileAnimationFrameContent
    {
        [XmlAttribute(AttributeName = "tileid")]
        public int TileIdentifier { get; set; }

        [XmlAttribute(AttributeName = "duration")]
        public int Duration { get; set; }

        public override string ToString()
        {
            return $"TileID: {TileIdentifier}, Duration: {Duration}";
        }
    }
}