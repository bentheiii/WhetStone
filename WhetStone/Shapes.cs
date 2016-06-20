using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WhetStone.Looping;
using WhetStone.NumbersMagic;
using WhetStone.Shapes.Lines;
using WhetStone.SystemExtensions;
using WhetStone.Units;
using WhetStone.Units.Angles;

namespace WhetStone.Shapes
{
    public static class PointExtentions
    {
        public static Point multiply(this Point p, double factor)
        {
            return new Point((int)(p.X * factor), (int)(p.Y * factor));
        }
        public static PointF multiply(this PointF p, double factor)
        {
            return new PointF((int)(p.X * factor), (int)(p.Y * factor));
        }
        public static PointF add(this PointF p, Point q)
        {
            return new PointF((p.X + q.X), (p.Y + q.Y));
        }
        public static PointF add(this PointF p, PointF q)
        {
            return new PointF((p.X + q.X), (p.Y + q.Y));
        }
        public static PointF add(this PointF p, float x, float y)
        {
            return new PointF((p.X + x), (p.Y + y));
        }
        public static Point add(this Point p, Point q)
        {
            return new Point((p.X + q.X), (p.Y + q.Y));
        }
        public static Point add(this Point p, int x, int y)
        {
            return new Point((p.X + x), (p.Y + y));
        }
        public static Point subtract(this Point p, Point q)
        {
            return p.add(q.multiply(-1));
        }
        public static PointF subtract(this PointF p, Point q)
        {
            return p.add(q.multiply(-1));
        }
        public static PointF subtract(this PointF p, PointF q)
        {
            return p.add(q.multiply(-1));
        }
        public static PointF Center(this Point @this, Point other)
        {
            return new PointF((@this.X+other.X)/2f, (@this.Y + other.Y) / 2f);
        }
        public static Point Round(this PointF @this)
        {
            return Point.Round(@this);
        }
        public static Point Center(this IEnumerable<Point> points)
        {
            return points.Aggregate((a,b)=>a.add(b)).multiply(1.0 / points.Count());
        }
        public static PointF Center(this IEnumerable<PointF> points)
        {
            return points.Aggregate((a, b) => a.add(b)).multiply(1.0 / points.Count());
        }
        public static Point RotateAround(this Point @this, Point origin, Angle angle)
        {
            Point n = @this.subtract(origin);
            n = new Point((int) (n.X*angle.Cos() - n.Y*angle.Sin()), (int) (n.X*angle.Sin() + n.Y*angle.Cos()));
            return n.add(origin);
        }
        public static PointF RotateAround(this PointF @this, PointF origin, Angle angle)
        {
            var n = @this.subtract(origin);
            n = new PointF((float) (n.X * angle.Cos() - n.Y * angle.Sin()), (float) (n.X * angle.Sin() + n.Y * angle.Cos()));
            return n.add(origin);
        }
        public static Point ScaleAround(this Point @this, Point origin, double scaleX, double scaleY)
        {
            var n = @this.subtract(origin);
            n = new Point((int)(n.X * scaleX), (int)(n.Y * scaleY));
            return n.add(origin);
        }
        public static PointF ScaleAround(this PointF @this, PointF origin, double scaleX, double scaleY)
        {
            var n = @this.subtract(origin);
            n = new PointF((float) (n.X * scaleX), (float) (n.Y * scaleY));
            return n.add(origin);
        }
        public static double Distance(this PointF @this, PointF other)
        {
            var n = @this.subtract(other);
            return Math.Sqrt(n.X * n.X + n.Y * n.Y);
        }
        public static bool isNear(this PointF @this, Point p)
        {
            return @this.subtract(p).Distance(PointF.Empty) < 1;
        }
        public static float Quotient(this PointF @this)
        {
            return @this.Y / @this.X;
        }
        public static bool IsWithin(this Size @this, int x, int y)
        {
            return (x >= 0 && y >= 0 && x < @this.Width && y < @this.Height);
        }
        public static bool IsWithin(this Size @this, Point p)
        {
            return @this.IsWithin(p.X,p.Y);
        }
    }
    namespace Lines
    {
        public class Line
        {
            public readonly PointF Origin;
            public readonly Angle Slope;
            public Line(Angle slope, PointF origin)
            {
                this.Slope = slope;
                this.Origin = origin;
            }
            public virtual bool iswithinbounds(PointF p)
            {
                return true;
            }
            public PointF? intersect(Line other)
            {
                if (this.Slope.Tan() == other.Slope.Tan())
                    return null;
                float x = (float)((this.Origin.X * this.Slope.Tan() - this.Origin.Y - (other.Origin.X * this.Slope.Tan() - other.Origin.Y)) / (this.Slope.Tan() - other.Slope.Tan()));
                PointF ret = get(x);
                return (iswithinbounds(ret) && other.iswithinbounds(ret)) ? ret : (PointF?)null;
            }
            private PointF get(float x)
            {
                return new PointF(x, (float)((x - this.Origin.X) * this.Slope.Tan() + this.Origin.X));
            }
        }
        public class LineSegment : Line
        {
            public readonly Tuple<PointF, PointF> Points;
            public override bool iswithinbounds(PointF p)
            {
                return p.X.iswithin(this.Points.Item1.X, this.Points.Item2.X) && p.Y.iswithin(this.Points.Item1.Y, this.Points.Item2.Y);
            }
            public LineSegment(PointF start, PointF end) : base(Angle.ATan(start.subtract(end).Quotient()).Normalize(), new PointF(0, 0))
            {
                this.Points = new Tuple<PointF, PointF>(start, end);
            }
            public float Length()
            {
                return (float)this.Points.Item1.Distance(this.Points.Item2);
            }
        }
        public class Ray : Line
        {
            public Ray(Angle slope, PointF origin) : base(slope, origin) { }
            public override bool iswithinbounds(PointF p)
            {
                Angle ang = Angle.ATan(p.subtract(this.Origin).Quotient()).Normalize();
                return (ang - this.Slope).InUnits(Angle.Turn).abs() <= 0.1;
            }
        }
    }
    namespace Polygons
    {
        public abstract class IPolygon
        {
            public virtual float Area()
            {
                return this.WhetStones().Trail2(true).Sum(a => a.Item1.X * a.Item2.Y - a.Item1.Y * a.Item1.X) / 2;
            }
            public virtual float Perimiter()
            {
                return this.Segments().Sum(a => a.Length());
            }
            public virtual PointF Center()
            {
                return this.WhetStones().Center();
            }
            public abstract IEnumerable<PointF> WhetStones();
            public virtual IEnumerable<LineSegment> Segments()
            {
                return this.WhetStones().Trail2(true).Select(a => new LineSegment(a.Item1, a.Item2));
            }
            public virtual IPolygon Rotate(Angle angle)
            {
                return new TransformationShape(this, angle, 1, 1, new PointF(0, 0));
            }
            public virtual IPolygon Scale(float scaleX, float scaleY)
            {
                return new TransformationShape(this, new Angle(0), scaleX, scaleY, new PointF(0, 0));
            }
            public virtual IPolygon Move(float x, float y)
            {
                return new TransformationShape(this, new Angle(0), 1, 1, new PointF(x, y));
            }
            public virtual bool isWithin(PointF point)
            {
                Ray r = new Ray(new Angle(0), point);
                bool ret = false;
                foreach (var segment in this.Segments())
                {
                    if (r.intersect(segment).HasValue)
                        ret = !ret;
                }
                return ret;
            }
            public static PointF? Collides(IPolygon a, IPolygon b)
            {
                return a.Segments()
                         .Join(b.Segments())
                         .Select(x => x.Item1.intersect(x.Item2))
                         .FirstOrDefault(x => x.HasValue);
            }
            public IPolygon Transform(Angle a, float scalex, float scaley, float x, float y)
            {
                return Rotate(a).Scale(scalex, scaley).Move(x, y);
            }
        }
        public class TransformationShape : IPolygon
        {
            private readonly float _scalex;
            private readonly float _scaley;
            private readonly PointF _location;
            private readonly Angle _rotation;
            private readonly IPolygon _interior;
            protected PointF _focus;
            public TransformationShape(IPolygon interior, Angle rotation, float scalex, float scaley, PointF location)
            {
                _rotation = rotation;
                _scalex = scalex;
                _scaley = scaley;
                _focus = interior.Center();
                this._location = location;
                this._interior = interior;
            }
            private PointF Transform(PointF original)
            {
                return
                    original.RotateAround(_focus, _rotation)
                            .ScaleAround(_focus, _scalex, _scaley)
                            .add(_location);
            }
            public override float Area()
            {
                return this._interior.Area() * _scalex * _scaley;
            }
            public override PointF Center()
            {
                return this._interior.Center().add(_location);
            }
            public override IEnumerable<PointF> WhetStones()
            {
                return this._interior.WhetStones().Select(this.Transform);
            }
            public override IPolygon Rotate(Angle angle)
            {
                return new TransformationShape(_interior, (_rotation + angle).Normalize(), _scalex, _scaley, _location);
            }
            public override IPolygon Scale(float scaleX, float scaleY)
            {
                return new TransformationShape(_interior, _rotation, _scalex * scaleX, _scaley * scaleY, _location);
            }
            public override IPolygon Move(float x, float y)
            {
                return new TransformationShape(_interior, _rotation, _scalex, _scaley, _location.add(x, y));
            }
            
        }
        public class Triangle : IPolygon
        {
            private readonly Tuple<PointF, PointF, PointF> _points;
            public Triangle(PointF a, PointF b, PointF c)
            {
                _points = new Tuple<PointF, PointF, PointF>(a, b, c);
            }
            public override float Area()
            {
                return
                    ((_points.Item1.X * (_points.Item2.Y - _points.Item3.Y) +
                      _points.Item2.X * (_points.Item3.Y - _points.Item1.Y) +
                      _points.Item3.X * (_points.Item1.Y - _points.Item2.Y)) / 2).abs();
            }
            public override IEnumerable<PointF> WhetStones()
            {
                yield return _points.Item1;
                yield return _points.Item2;
                yield return _points.Item3;
            }
        }
        public class Rectangle : IPolygon
        {
            private readonly float _width, _height;
            public Rectangle(float width, float height)
            {
                this._height = height;
                this._width = width;
            }
            public override IPolygon Scale(float scaleX, float scaleY)
            {
                return new Rectangle(_width * scaleX, _height * scaleY);
            }
            public override float Area()
            {
                return _width * _height;
            }
            public override PointF Center()
            {
                return new PointF(_width / 2, _height / 2);
            }
            public override IEnumerable<PointF> WhetStones()
            {
                yield return new PointF(0, 0);
                yield return new PointF(0, _height);
                yield return new PointF(_width, _height);
                yield return new PointF(_width, 0);
            }
            public override float Perimiter()
            {
                return 2 * (_width + _height);
            }
            public override bool isWithin(PointF point)
            {
                return point.X.iswithinexclusive(0, _width) && point.Y.iswithinexclusive(0, _height);
            }
        }
        public class RegularPolygon : IPolygon
        {
            private readonly int _sides;
            private readonly PointF _center;
            private readonly float _radius;
            public RegularPolygon(int sides, PointF center, float radius)
            {
                this._sides = sides;
                this._center = center;
                this._radius = radius;
            }
            public override float Area()
            {
                return (float)(2 * this._radius * (2 * Math.PI / this._sides).sin());
            }
            public override PointF Center()
            {
                return _center;
            }
            public override IEnumerable<PointF> WhetStones()
            {
                var rot = new Angle(1.0 / _sides, Angle.Turn);
                var ret = _center.add(_radius, 0);
                foreach (int i in Loops.Range(_sides))
                {
                    yield return ret;
                    ret = ret.RotateAround(this.Center(), rot);
                }
            }
        }
        public class Polygon : IPolygon
        {
            private readonly IReadOnlyCollection<PointF> _points;
            public Polygon(IEnumerable<PointF> points)
            {
                this._points = new ArraySegment<PointF>(points.ToArray());
            }
            public override float Area()
            {
                return _points.Trail2(true).Sum(a => a.Item1.X * a.Item2.Y - a.Item1.Y * a.Item1.X) / 2;
            }
            public override IEnumerable<PointF> WhetStones()
            {
                return _points;
            }
        }
        public class EmptyShape : IPolygon
        {
            public override bool isWithin(PointF point)
            {
                return false;
            }
            public override IEnumerable<LineSegment> Segments()
            {
                yield break;
            }
            public override IPolygon Rotate(Angle angle)
            {
                return this;
            }
            public override float Perimiter()
            {
                return 0f;
            }
            public override IPolygon Move(float x, float y)
            {
                return this;
            }
            public override PointF Center()
            {
                throw new NotSupportedException("empty shape has no center");
            }
            public override float Area()
            {
                return 0f;
            }
            public override IPolygon Scale(float scaleX, float scaleY)
            {
                return this;
            }
            public override IEnumerable<PointF> WhetStones()
            {
                yield break;
            }
        }
    }
}