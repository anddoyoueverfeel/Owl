using System;
using System.Collections.Generic;
using Rhino.Geometry;
using Grasshopper.Kernel;
using Owl;
using Owl.Core.Structures;
using Owl.GH.Common;
using Owl.Learning.Initializers;
using Owl.Learning.Networks;
using Owl.Learning.NeuronFunctions;


public class ConstructOwlNetwork : GH_Component
{
    public ConstructOwlNetwork() : base(
        "HA_Construct Network", 
        "HA_Network", 
        "Construct Owl.Learning Network (HackAttack version)",
        "HackAttack",
        "HA_Owl")
    {
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
        pManager.AddIntegerParameter("Layers", "L", "Neurons per layer, first number = Inputs", GH_ParamAccess.list);
        pManager.AddIntegerParameter("Activation", "F(x)", "Activation function per layer. 0:Linear 1:Relu 2:Sigmoid 3:Tanh", GH_ParamAccess.list);
        pManager.AddIntervalParameter("Range", "R", "Initializer range", GH_ParamAccess.item, new Interval(0, 1));
        pManager.AddIntegerParameter("Seed", "S", "Random seed", GH_ParamAccess.item, 123);
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
        pManager.AddParameter(new Param_OwlNetwork());
    }

    protected override void SolveInstance(IGH_DataAccess DA)
    {
        List<int> l = new List<int>();
        List<int> f = new List<int>();

        Interval rng = new Interval();
        int seed = 123;

        if (!DA.GetDataList(0, l))
            return;
        if (!DA.GetDataList(1, f))
            return;
        if (!DA.GetData(2, ref rng))
            return;
        if (!DA.GetData(3, ref seed))
            return;

        List<int> lays = new List<int>(l);
        lays.RemoveAt(0);

        if (f.Count == 1)
        {
            for (int i = 1; i <= lays.Count - 1; i += 1)
                f.Add(f[0]);
        }

        List<NeuronFunctionBase> fs = new List<NeuronFunctionBase>();

        foreach (int val in f)
        {
            switch (val)
            {
                case 0:
                    {
                        fs.Add(new Linear());
                        break;
                    }

                case 1:
                    {
                        fs.Add(new Relu());
                        break;
                    }

                case 2:
                    {
                        fs.Add(new Sigmoid());
                        break;
                    }

                case 3:
                    {
                        fs.Add(new Tanh());
                        break;
                    }
            }
        }

        Network nn = new Network(fs, l[0], lays, new RandomInitializer(Range.Create(rng.T0, rng.T1), seed));

        DA.SetData(0, nn);
    }

    public override Guid ComponentGuid
    {
        get
        {
            return new Guid("{8c8b8025-541b-4682-87a2-d461b9fab11e}");
        }
    }

    public override GH_Exposure Exposure
    {
        get
        {
            return GH_Exposure.primary;
        }
    }

}

