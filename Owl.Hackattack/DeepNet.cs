using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Accord;
using Accord.Neuro;
using Accord.Neuro.Learning;
using System.Linq;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace Owl.Hackattack
{
    public class DeepNet : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public DeepNet()
          : base("Owl.Hackattack", "Nickname",
              "Description",
              "Hackattack", "AccordDeepNet")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("i0", "", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("i1", "", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("i2", "", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("i3", "", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("i4", "", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("i5", "", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("i6", "", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("i7", "", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("i8", "", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("i9", "", "", GH_ParamAccess.list);

            pManager.AddNumberParameter("o0", "", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("o1", "", "", GH_ParamAccess.list);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("inputs", "", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("predicted1", "", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("actual1", "", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("predicted2", "", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("actual2", "", "", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<double> inputZero = new List<double>();
            List<double> inputOne = new List<double>();
            List<double> inputTwo = new List<double>();
            List<double> inputThree = new List<double>();
            List<double> inputFour = new List<double>();
            List<double> inputFive = new List<double>();
            List<double> inputSix = new List<double>();
            List<double> inputSeven = new List<double>();
            List<double> inputEight = new List<double>();
            List<double> inputNine = new List<double>();

            List<double> outputZero = new List<double>();
            List<double> outputOne = new List<double>();

            if(!DA.GetDataList(0, inputZero)) { return; }
            if (!DA.GetDataList(1, inputOne)) { return; }
            if (!DA.GetDataList(2, inputTwo)) { return; }
            if (!DA.GetDataList(3, inputThree)) { return; }
            if (!DA.GetDataList(4, inputFour)) { return; }
            if (!DA.GetDataList(5, inputFive)) { return; }
            if (!DA.GetDataList(6, inputSix)) { return; }
            if (!DA.GetDataList(7, inputSeven)) { return; }
            if (!DA.GetDataList(8, inputEight)) { return; }
            if (!DA.GetDataList(9, inputNine)) { return; }

            if (!DA.GetDataList(10, outputZero)) { return; }
            if (!DA.GetDataList(11, outputOne)) { return; }


            if (!new[] {
                inputZero.Count, inputOne.Count, inputTwo.Count,
                inputThree.Count, inputFour.Count, inputFive.Count,
                inputSix.Count, inputSeven.Count, inputEight.Count,
                inputNine.Count, outputZero.Count, outputOne.Count,
            }.All(x => x == inputZero.Count))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input features are not all the same length");
                return;
            }


            double[][] inputs = new double[inputZero.Count][];

            for (int i = 0; i < inputZero.Count; i++)
            {
                inputs[i] = new double[] {
                    inputZero[i],
                    inputOne[i],
                    inputTwo[i],
                    inputThree[i],
                    inputFour[i],
                    inputFive[i],
                    inputSix[i],
                    inputSeven[i],
                    inputEight[i],
                    inputNine[i]
                };
            }

            double[][] outputs = new double[inputZero.Count][];

            for (int i = 0; i < inputZero.Count; i++)
            {
                outputs[i] = new double[] { outputZero[i], outputOne[i] };
            }



            ActivationNetwork network = new ActivationNetwork(new SigmoidFunction(2), 10, 64, 128, 64, 2);

            new NguyenWidrow(network).Randomize();

            BackPropagationLearning teacher = new BackPropagationLearning(network)
            {
                
            };

            teacher.LearningRate = 0.01;

            int EpochCount = 0;
            double error = 1.0;


            while (error > 1e-5 && EpochCount < 10)
            {
                error = teacher.RunEpoch(inputs, outputs);
                EpochCount++;
            }
            


            int predictionsToMake = 0;
            if (inputZero.Count < 100)
            {
                predictionsToMake = inputZero.Count;
            }
            else
            {
                predictionsToMake = 100;
            }





            List<string> predictionInputs = new List<string>();

            List<double> predictedOutputOne = new List<double>();
            List<double> actualOutputOne = new List<double>();

            List<double> predictedOutputTwo = new List<double>();
            List<double> actualOutputTwo = new List<double>();


            // Checks if the network has learned
            for (int i = 0; i < predictionsToMake; i++)
            {
                string predictionInput =
                    inputZero[i].ToString() + "," +
                    inputOne[i].ToString() + "," +
                    inputTwo[i].ToString() + "," +
                    inputThree[i].ToString() + "," +
                    inputFour[i].ToString() + "," +
                    inputFive[i].ToString() + "," +
                    inputSix[i].ToString() + "," +
                    inputSeven[i].ToString() + "," +
                    inputEight[i].ToString() + "," +
                    inputNine[i].ToString();

                predictionInputs.Add(predictionInput);

                double[] prediction = network.Compute(inputs[i]);
                double[] actual = outputs[i];

                predictedOutputOne.Add(prediction[0]);
                actualOutputOne.Add(actual[0]);

                predictedOutputTwo.Add(prediction[1]);
                actualOutputTwo.Add(actual[1]);

            }

            DA.SetDataList(0, predictionInputs);
            DA.SetDataList(1, predictedOutputOne);
            DA.SetDataList(2, actualOutputOne);
            DA.SetDataList(3, predictedOutputTwo);
            DA.SetDataList(4, actualOutputTwo);

            /*
            pManager.AddNumberParameter("predicted1", "", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("actual1", "", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("predicted2", "", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("actual2", "", "", GH_ParamAccess.list);
            */

            //network = new ActivationNetwork(new SigmoidFunction(2), 2, 2, 1);
            //teacher = new BackPropagationLearning(network);
            //int[] neuronsPerLayer = { 64, 128, 64};
            //ActivationNetwork network = new ActivationNetwork(new SigmoidFunction(0.01), 10, neuronsPerLayer);

            //double error = teacher.RunEpoch(input, output);

            //network.Save(saveFileDialog1.FileName);

        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("e76a0799-7eed-4968-b196-736dcf935a30"); }
        }
    }
}
