
[System.Serializable]
public struct Point
{
    public int x;
    public int z;

    public Point(int x, int y)
    {
        this.x = x;
        this.z = y;
    }

    public override bool Equals(object obj)
    {
        if (obj is Point)
        {
            Point p = (Point)obj;
            return this.x == p.x && this.z == p.z;
        }
        return false;
    }

    // optimisation, uselly, we will compare two points
    public bool Equals(Point p)
    {
        return this.x == p.x && this.z == p.z;
    }

    public override int GetHashCode()
    {
        return x ^ z;
    }

    public override string ToString()
    {
        return string.Format("({0}, {1})", x, z);
    }

    public static Point operator +(Point p1, Point p2)
    {
        return new Point(p1.x + p2.x, p1.z + p2.z);
    }

    public static Point operator -(Point p1, Point p2)
    {
        return new Point(p1.x - p2.x, p1.z - p2.z);
    }

    public static bool operator ==(Point p1, Point p2)
    {
        return p1.x == p2.x && p1.z == p2.z;
    }

    public static bool operator !=(Point p1, Point p2)
    {
        return !(p1 == p2);
    }
}
