using System;
using Grasshopper.Kernel;
using Accord;
using Accord.Neuro;
using Accord.Neuro.Learning;
using System.Linq;
using Owl.Learning.Networks

public class Compute : GH_Component
{
    public Compute() : base("Compute", "Compute", "Compute the output values for the given input TensorSet", "Owl.Learning", "Supervised")
    {
    }

    public override Guid ComponentGuid
    {
        get
        {
            //update GUID
            return new Guid("{2F75DA58-C7C1-4AE7-AC8A-9B7962259F76}");
        }
    }

    protected override Bitmap Icon
    {
        get
        {
            return null;
        }
    }

    public override GH_Exposure Exposure
    {
        get
        {
            return GH_Exposure.secondary;
        }
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
        pManager.AddParameter(new Param_OwlNetwork());
        pManager.AddParameter(new Param_OwlTensorSet(), "Input", "I", "Input TensorSet", GH_ParamAccess.item);
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
        pManager.AddParameter(new Param_OwlTensorSet(), "Output", "O", "Output TensorSet", GH_ParamAccess.item);
    }

    protected override void SolveInstance(IGH_DataAccess DA)
    {
        GH_OwlNetwork nn = null/* TODO Change to default(_) if this is not a reference type */;
        GH_OwlTensorSet ins = new GH_OwlTensorSet();

        if (!DA.GetData(0, nn))
            return;
        if (!DA.GetData(1, ins))
            return;

        TensorSet outs = nn.Value.ComputeOptimized(ins.Value);

        // For i As Integer = 0 To ins.Value.Count - 1 Step 1
        // outs.Add(nn.Value.Compute(ins.Value(i)))
        // Next

        DA.SetData(0, outs);
    }
}
