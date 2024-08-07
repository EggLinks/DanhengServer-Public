using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Util;

public class Position
{
    public Position(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Position(Vector vector)
    {
        X = vector.X;
        Y = vector.Y;
        Z = vector.Z;
    }

    public Position()
    {
        X = 0;
        Y = 0;
        Z = 0;
    }

    public Position(Position position)
    {
        X = position.X;
        Y = position.Y;
        Z = position.Z;
    }

    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public void Set(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public void Set(Position position)
    {
        X = position.X;
        Y = position.Y;
        Z = position.Z;
    }

    public void Set(Vector vector)
    {
        X = vector.X;
        Y = vector.Y;
        Z = vector.Z;
    }

    public void Add(int x, int y, int z)
    {
        X += x;
        Y += y;
        Z += z;
    }

    public void Add(Position position)
    {
        X += position.X;
        Y += position.Y;
        Z += position.Z;
    }

    public void Sub(int x, int y, int z)
    {
        X -= x;
        Y -= y;
        Z -= z;
    }

    public void Sub(Position position)
    {
        X -= position.X;
        Y -= position.Y;
        Z -= position.Z;
    }

    public void Mul(int x, int y, int z)
    {
        X *= x;
        Y *= y;
        Z *= z;
    }

    public void Mul(Position position)
    {
        X *= position.X;
        Y *= position.Y;
        Z *= position.Z;
    }

    public void Div(int x, int y, int z)
    {
        X /= x;
        Y /= y;
        Z /= z;
    }

    public void Div(Position position)
    {
        X /= position.X;
        Y /= position.Y;
        Z /= position.Z;
    }

    public Vector ToProto()
    {
        return new Vector
        {
            X = X,
            Y = Y,
            Z = Z
        };
    }

    public long GetFast2dDist(Position pos)
    {
        long x = X - pos.X;
        long z = Z - pos.Z;
        return x * x + z * z;
    }
}