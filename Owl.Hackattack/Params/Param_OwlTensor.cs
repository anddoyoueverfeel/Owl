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

public class Param_OwlTensor : GH_PersistentParam<GH_OwlTensor>
{
    public Param_OwlTensor() : base(new GH_InstanceDescription("Tensor", "T", "Owl Tensor", "Owl", "Params"))
    {
    }

    public override Guid ComponentGuid
    {
        get
        {
            return new Guid("{3101F182-8420-44D1-AC95-CEBD203DEE4A}");
        }
    }

    protected override GH_GetterResult Prompt_Plural(ref List<GH_OwlTensor> values)
    {
        return GH_GetterResult.cancel;
    }

    protected override GH_GetterResult Prompt_Singular(ref GH_OwlTensor value)
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
