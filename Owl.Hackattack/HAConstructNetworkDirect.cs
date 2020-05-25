using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Owl;
using Owl.GH.Common;
using Owl.Learning.Networks;
using Owl.Learning.NeuronFunctions;
using Owl.Core.Tensors;



public class ConstructOwlNetwork_Direct : GH_Component
{
    public ConstructOwlNetwork_Direct() : base(
        "Construct Network Direct", 
        "NetworkDirect", 
        "Construct Owl.Learning Network from the atomic data." + Environment.NewLine + "Use this component when importing already trained models from other frameworks.", 
        "HackAttack", 
        "Owl")
    {
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

        if (!DA.GetData(0, ref w))
            return;
        if (!DA.GetData(1, ref b))
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

    public override Guid ComponentGuid
    {
        get
        {
            return new Guid("{43206ca1-2c19-4112-a0bf-f5ba6a6ee8ed}");
        }
    }

    protected override System.Drawing.Bitmap Icon
    {
        get
        {
            // You can add image files to your project resources and access them like this:
            //return Resources.IconForThisComponent;
            return null;
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
