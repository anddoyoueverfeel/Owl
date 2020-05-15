using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GH_IO.Serialization;
using Grasshopper.Kernel.Types;
using Owl.Core.Tensors;
using Rhino.Geometry;

public partial class GH_OwlTensor : GH_Goo<Tensor>
{
    public GH_OwlTensor() : base()
    {
    }

    public GH_OwlTensor(Tensor V) : base(V)
    {
    }

    public override bool IsValid
    {
        get
        {
            return true;
        }
    }

    public override string TypeDescription
    {
        get
        {
            return "Tensor";
        }
    }

    public override string TypeName
    {
        get
        {
            return "Tensor";
        }
    }

    public override IGH_Goo Duplicate()
    {
        return new GH_OwlTensor(this.Value.Duplicate());
    }

    public override string ToString()
    {
        return this.Value.ToString();
    }

    public override bool CastTo<Q>(ref Q target)
    {
        var switchExpr = typeof(Q);
        switch (switchExpr)
        {
            case var @case when @case == typeof(Tensor):
                {
                    object obj = this.Value.Duplicate();
                    target = (Q)obj;
                    return true;
                }
        }

        return false;
    }

    public override bool CastFrom(object source)
    {
        var switchExpr = source.GetType();
        switch (switchExpr)
        {
            case var @case when @case == typeof(Tensor):
                {
                    this.Value = (Tensor)source;
                    return true;
                }

            case var case1 when case1 == typeof(TensorSet):
                {
                    TensorSet ts = (TensorSet)source;
                    if (ts.Count != 1)
                        return false;
                    this.Value = ts[0];
                    return true;
                }

            case var case2 when case2 == typeof(Point3d):
                {
                    Point3d asp = source;
                    this.Value = new Tensor(new[] { asp.X, asp.Y, asp.Z });
                    return true;
                }

            case var case3 when case3 == typeof(GH_Point):
                {
                    GH_Point asp = source;
                    this.Value = new Tensor(new[] { asp.Value.X, asp.Value.Y, asp.Value.Z });
                    return true;
                }

            case var case4 when case4 == typeof(GH_Vector):
                {
                    GH_Vector asp = source;
                    this.Value = new Tensor(new[] { asp.Value.X, asp.Value.Y, asp.Value.Z });
                    return true;
                }

            case var case5 when case5 == typeof(Vector3d):
                {
                    Vector3d asp = source;
                    this.Value = new Tensor(new[] { asp.X, asp.Y, asp.Z });
                    return true;
                }

            case var case6 when case6 == typeof(Color):
                {
                    Color asp = (Color)source;
                    this.Value = new Tensor(new[] { Convert.ToDouble(asp.A), Convert.ToDouble(asp.R), Convert.ToDouble(asp.G), Convert.ToDouble(asp.B) });
                    return true;
                }

            case var case7 when case7 == typeof(GH_Colour):
                {
                    GH_Colour asp = source;
                    this.Value = new Tensor(new[] { Convert.ToDouble(asp.Value.A), Convert.ToDouble(asp.Value.R), Convert.ToDouble(asp.Value.G), Convert.ToDouble(asp.Value.B) });
                    return true;
                }

            case var case8 when case8 == typeof(Line):
                {
                    Line asp = source;
                    this.Value = new Tensor(new[] { asp.From.X, asp.From.Y, asp.From.Z, asp.To.X, asp.To.Y, asp.To.Z });
                    return true;
                }

            case var case9 when case9 == typeof(GH_Line):
                {
                    GH_Line thisv = source;
                    Line asp = thisv.Value;
                    this.Value = new Tensor(new[] { asp.From.X, asp.From.Y, asp.From.Z, asp.To.X, asp.To.Y, asp.To.Z });
                    return true;
                }
        }

        return false;
    }

    public override bool Read(GH_IReader reader)
    {
        var shape = new List<int>();
        int cnt = reader.GetInt32("ShapeCount");
        for (int i = 0, loopTo = cnt - 1; i <= loopTo; i += 1)
            shape.Add(reader.GetInt32("S" + i));
        this.Value = new Tensor(shape, reader.GetDoubleArray("Data"));
        return true;
    }

    public override bool Write(GH_IWriter writer)
    {
        writer.SetInt32("ShapeCount", this.Value.ShapeCount);
        List<int> shape = this.Value.GetShape;
        for (int i = 0, loopTo = this.Value.ShapeCount - 1; i <= loopTo; i += 1)
            writer.SetInt32("S" + i, shape[i]);
        writer.SetDoubleArray("Data", this.Value.ToArray);
        return true;
    }
}