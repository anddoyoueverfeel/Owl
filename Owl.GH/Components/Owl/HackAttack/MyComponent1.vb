Imports System.Collections.Generic

Imports Grasshopper.Kernel
Imports Rhino.Geometry
Imports Accord.Statistics.Models



Namespace Owl.GH

    Public Class MyComponent1
        Inherits GH_Component
        ''' <summary>
        ''' Each implementation of GH_Component must provide a public 
        ''' constructor without any arguments.
        ''' Category represents the Tab in which the component will appear, 
        ''' Subcategory the panel. If you use non-existing tab or panel names, 
        ''' new tabs/panels will automatically be created.
        ''' </summary>
        Public Sub New()
            MyBase.New("MyComponent1", "MyComponent1",
                "MyComponent1 description",
                "MyComponent1 category", "MyComponent1 subcategory")
        End Sub

        ''' <summary>
        ''' Registers all the input parameters for this component.
        ''' </summary>
        Protected Overrides Sub RegisterInputParams(pManager As GH_Component.GH_InputParamManager)
            pManager.AddNumberParameter("InputsTrain", "XT", "Inputs for training on", GH_ParamAccess.list)
            pManager.AddNumberParameter("OutputsTrain", "yT", "Outsputs for training on", GH_ParamAccess.list)
            pManager.AddNumberParameter("PreditX", "XP", "Inputs to predict for", GH_ParamAccess.list)
        End Sub

        ''' <summary>
        ''' Registers all the output parameters for this component.
        ''' </summary>
        Protected Overrides Sub RegisterOutputParams(pManager As GH_Component.GH_OutputParamManager)
            pManager.AddNumberParameter("Predicted Values", "yP", "Predicted values", GH_ParamAccess.list)
        End Sub

        ''' <summary>
        ''' This is the method that actually does the work.
        ''' </summary>
        ''' <param name="DA">The DA object can be used to retrieve data from input parameters and 
        ''' to store data in output parameters.</param>
        Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)
            Dim xVals As New List(Of Double)
            If Not DA.GetDataList(0, xVals) Then Return
            Dim xValsArray As Double() = xVals.ToArray

            Dim yVals As New List(Of Double)
            If Not DA.GetDataList(1, yVals) Then Return
            Dim yValsArray As Double() = yVals.ToArray


            Dim ols As Regression.Linear.OrdinaryLeastSquares = New Regression.Linear.OrdinaryLeastSquares()
            Dim regression As Regression.Linear.SimpleLinearRegression = ols.Learn(xValsArray, yValsArray)


            Dim predict As New List(Of Double)
            If Not DA.GetDataList(2, predict) Then Return
            Dim predictArray As Double() = yVals.ToArray


            Dim prediction As Double() = regression.Transform(predictArray)


            ''.Transform(predictArray)


            Dim s As Double = regression.Slope
            Dim c As Double = regression.Intercept


            DA.SetDataList(0, prediction)

        End Sub


        ''' <summary>
        ''' Provides an Icon for every component that will be visible in the User Interface.
        ''' Icons need to be 24x24 pixels.
        ''' </summary>
        Protected Overrides ReadOnly Property Icon() As System.Drawing.Bitmap
            Get
                'You can add image files to your project resources and access them like this:
                ' return Resources.IconForThisComponent;
                Return Nothing
            End Get
        End Property

        ''' <summary>
        ''' Each component must have a unique Guid to identify it. 
        ''' It is vital this Guid doesn't change otherwise old ghx files 
        ''' that use the old ID will partially fail during loading.
        ''' </summary>
        Public Overrides ReadOnly Property ComponentGuid() As Guid
            Get
                Return New Guid("f9497df7-334e-4a85-8d4b-4717d6d64332")
            End Get
        End Property
    End Class

End Namespace