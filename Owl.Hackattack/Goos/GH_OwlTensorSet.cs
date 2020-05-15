using System.Collections.Generic;
using System.Linq;
using GH_IO.Serialization;
using Grasshopper.Kernel.Types;
using Owl.Core.IO;
using Owl.Core.Tensors;

public partial class GH_OwlTensorSet : GH_Goo<TensorSet>
{
    public GH_OwlTensorSet() : base()
    {
    }

    public GH_OwlTensorSet(TensorSet V) : base(V)
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
            return "TensorSet";
        }
    }

    public override string TypeName
    {
        get
        {
            return "TensorSet";
        }
    }

    public override IGH_Goo Duplicate()
    {
        return new GH_OwlTensorSet(this.Value.Duplicate);
    }

    public override string ToString()
    {
        return this.Value.ToString;
    }

    public override bool CastTo<Q>(ref Q target)
    {
        var switchExpr = typeof(Q);
        switch (switchExpr)
        {
            case var @case when @case == typeof(TensorSet):
                {
                    TensorSet mev = this.Value.Duplicate;
                    object obj = mev;
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
                    Tensor tens = (Tensor)source;
                    tens = tens.Duplicate;
                    this.Value = new TensorSet(new[] { tens });
                    return true;
                }

            case var case1 when case1 == typeof(TensorSet):
                {
                    this.Value = (TensorSet)source;
                    return true;
                }

            case var case2 when case2 == typeof(GH_String):
                {
                    GH_String thiss = source;
                    this.Value = new TensorSet(thiss.Value);
                    return true;
                }

            case var case3 when case3 == typeof(string):
                {
                    string @this = Conversions.ToString(source);
                    this.Value = new TensorSet(@this);
                    return true;
                }
        }

        return false;
    }

    public override bool Read(GH_IReader reader)
    {
        var ts = new TensorSet();
        for (int i = 0, loopTo = reader.GetInt64("TensorSetCount") - 1; i <= loopTo; i += 1)
            ts.Add(ReadOne(reader, i));
        this.Value = ts;
        return true;
    }

    public override bool Write(GH_IWriter writer)
    {
        writer.SetInt64("TensorSetCount", this.Value.Count);
        for (int i = 0, loopTo = this.Value.Count - 1; i <= loopTo; i += 1)
            WriteOne(this.Value(i), writer, i);
        return true;
    }

    public Tensor ReadOne(GH_IReader reader, string Suffix)
    {
        var shape = new List<int>();
        int cnt = reader.GetInt32("ShapeCount_" + Suffix);
        for (int i = 0, loopTo = cnt - 1; i <= loopTo; i += 1)
            shape.Add(reader.GetInt32("S" + i + "_" + Suffix));
        return new Tensor(shape, reader.GetDoubleArray("Data_" + Suffix));
    }

    public bool WriteOne(Tensor tens, GH_IWriter writer, string Suffix)
    {
        writer.SetInt32("ShapeCount_" + Suffix, tens.ShapeCount);
        List<int> shape = tens.GetShape;
        for (int i = 0, loopTo = tens.ShapeCount - 1; i <= loopTo; i += 1)
            writer.SetInt32("S" + i + "_" + Suffix, shape[i]);
        writer.SetDoubleArray("Data_" + Suffix, tens.ToArray);
        return true;
    }
}