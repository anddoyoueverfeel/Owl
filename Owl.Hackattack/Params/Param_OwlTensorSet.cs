using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Grasshopper.Kernel;
using Owl.Core.Tensors;

public class Param_OwlTensorSet : Grasshopper.Kernel.GH_PersistentParam<GH_OwlTensorSet>
{
    public static string Nick = "TS";

    public Param_OwlTensorSet() : base(new GH_InstanceDescription("TensorSet", Nick, "Owl TensorSet", "Owl", "Params"))
    {
    }

    public override Guid ComponentGuid
    {
        get
        {
            return new Guid("{E7D0C937-2A79-4DDF-BA05-25BE37B1F05C}");
        }
    }

    protected override GH_GetterResult Prompt_Plural(ref List<GH_OwlTensorSet> values)
    {
        return GH_GetterResult.cancel;
    }

    protected override GH_GetterResult Prompt_Singular(ref GH_OwlTensorSet value)
    {
        return GH_GetterResult.cancel;
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
}
