using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Accord;
using Accord.Neuro;
using Accord.Neuro.Learning;
using System.Linq;


public class ConstructOwlNetwork_Direct : GH_Component
{
    public ConstructOwlNetwork_Direct() : base("Construct Network Ex", "NetworkEx", "Construct Owl.Learning Network from the atomic data." + Constants.vbCrLf + "Use this component when importing already trained models from other frameworks.", "Owl.Learning", "Supervised")
    {
    }

    public override Guid ComponentGuid
    {
        get
        {
            return new Guid("{29977F8E-492F-431D-B308-2A45AFF3B3F8}");
        }
    }

    protected override Bitmap Icon
    {
        get
        {
            return My.Resources.Icons_new_22;
        }
    }

    public override GH_Exposure Exposure
    {
        get
        {
            return GH_Exposure.primary;
        }
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
        pManager.AddParameter(new Param_OwlTensorSet(), "Weights", "W", "Weights", GH_ParamAccess.item);
        pManager.AddParameter(new Param_OwlTensorSet(), "Biases", "B", "Biases", GH_ParamAccess.item);
        pManager.AddIntegerParameter("Activation", "F(x)", "Activation function per layer. 0:Linear 1:Relu 2:Sigmoid 3:Tanh", GH_ParamAccess.list);
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
        pManager.AddParameter(new Param_OwlNetwork());
    }

    protected override void SolveInstance(IGH_DataAccess DA)
    {
        TensorSet w = new TensorSet();
        TensorSet b = new TensorSet();
        List<int> f = new List<int>();

        if (!DA.GetData(0, w))
            return;
        if (!DA.GetData(1, b))
            return;
        if (!DA.GetDataList(2, f))
            return;

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

        Network nn = new Network(w, b, fs);
        DA.SetData(0, new GH_OwlNetwork(nn));
    }
}
