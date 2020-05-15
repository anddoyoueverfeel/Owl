using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using GH_IO.Serialization;
using Grasshopper.Kernel.Types;
using Owl;
using Owl.Core.Tensors;
using Owl.Learning.Networks;

public partial class GH_OwlNetwork : GH_Goo<Network>
{
    public GH_OwlNetwork() : base()
    {
    }

    public GH_OwlNetwork(Network Net) : base(Net)
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
            return "Owl.Learning.Network";
        }
    }

    public override string TypeName
    {
        get
        {
            return "Owl.Learning.Network";
        }
    }

    public override IGH_Goo Duplicate()
    {
        return new GH_OwlNetwork(this.Value.Duplicate());
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
            case var @case when @case == typeof(Network):
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
            case var @case when @case == typeof(Network):
                {
                    Network net = (Network)source;
                    net = net.Duplicate();
                    this.Value = net;
                    return true;
                }
        }

        return false;
    }

    public override bool Read(GH_IReader reader)
    {
        var nn = new BinaryFormatter();
        var f = new List<Owl.Learning.NeuronFunctions.NeuronFunctionBase>();
        int fc = reader.GetInt32("FunctionCount");
        TensorSet w = default;
        TensorSet b = default;
        for (int i = 0, loopTo = fc - 1; i <= loopTo; i += 1)
        {
            using (var mstr = new MemoryStream(reader.GetByteArray("Function_" + i)))
            {
                f.Add(nn.Deserialize(mstr));
            }
        }

        using (var mstr = new MemoryStream(reader.GetByteArray("Biases")))
        {
            b = Owl.Core.IO.ReadTensors(mstr);
        }

        using (var mstr = new MemoryStream(reader.GetByteArray("Weigths")))
        {
            w = Owl.Core.IO.ReadTensors(mstr);
        }

        this.Value = new Owl.Learning.Networks.Network(w, b, f);
        return true;
    }

    public override bool Write(GH_IWriter writer)
    {
        var nn = new BinaryFormatter();
        writer.SetInt32("FunctionCount", this.Value.NeuronFunctions.Count);
        for (int i = 0, loopTo = this.Value.NeuronFunctions.Count - 1; i <= loopTo; i += 1)
        {
            using (var mstr = new MemoryStream())
            {
                nn.Serialize(mstr, this.Value.NeuronFunctions(i));
                writer.SetByteArray("Function_" + i, mstr.ToArray());
            }
        }

        using (var mstr = new MemoryStream())
        {
            Owl.Core.IO.WriteTensors(mstr, this.Value.Biases);
            writer.SetByteArray("Biases", mstr.ToArray());
        }

        using (var mstr = new MemoryStream())
        {
            Owl.Core.IO.WriteTensors(mstr, this.Value.Weights);
            writer.SetByteArray("Weights", mstr.ToArray());
        }

        return true;
    }
}