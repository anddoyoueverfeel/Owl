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

public class Param_OwlNetwork : GH_PersistentParam<GH_OwlNetwork>
{
    public Param_OwlNetwork() : base(new GH_InstanceDescription("Owl.Learning.Network", "N", "Owl.Learning.Network", "Owl.Learning", "Params"))
    {
    }

    public override Guid ComponentGuid
    {
        get
        {
            return new Guid("{9F28C691-9AA6-4CBB-9551-DF3B296ADB43}");
        }
    }

    protected override GH_GetterResult Prompt_Plural(ref List<GH_OwlNetwork> values)
    {
        return GH_GetterResult.cancel;
    }

    protected override GH_GetterResult Prompt_Singular(ref GH_OwlNetwork value)
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
