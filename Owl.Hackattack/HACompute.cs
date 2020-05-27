using System;
using Grasshopper.Kernel;
using System.Linq;
using Owl;
using Owl.Core.Tensors;
using Owl.GH;
using Owl.GH.Common;
using Owl.Learning.Networks;

public class Compute : GH_Component
{
    public Compute() : base(
        "HA_Compute", 
        "HA_Compute", 
        "Compute the output values for the given input TensorSet (HackAttack version)",
        "HackAttack",
        "HA_Owl")
    {
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

        if (!DA.GetData(0, ref nn))
            return;
        if (!DA.GetData(1, ref ins))
            return;

        TensorSet outs = nn.Value.ComputeOptimized(ins.Value);

        // For i As Integer = 0 To ins.Value.Count - 1 Step 1
        // outs.Add(nn.Value.Compute(ins.Value(i)))
        // Next

        DA.SetData(0, outs);
    }

    public override Guid ComponentGuid
    {
        get
        {
            return new Guid("{2591c2c2-130a-4bc7-a639-b4d9ffc9954c}");
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
