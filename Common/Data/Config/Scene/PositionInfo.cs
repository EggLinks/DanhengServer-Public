using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.Data.Config.Scene;

public class PositionInfo
{
    public int ID { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }
    public bool IsDelete { get; set; }
    public string Name { get; set; } = "";
    public float RotX { get; set; }
    public float RotY { get; set; }
    public float RotZ { get; set; }

    public Position ToPositionProto()
    {
        return new Position
        {
            X = (int)(PosX * 1000f),
            Y = (int)(PosY * 1000f),
            Z = (int)(PosZ * 1000f)
        };
    }

    public Position ToRotationProto()
    {
        return new Position
        {
            Y = (int)(RotY * 1000f),
            X = (int)(RotX * 1000f),
            Z = (int)(RotZ * 1000f)
        };
    }
}