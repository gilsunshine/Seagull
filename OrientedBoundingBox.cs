using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using Seagull.CGAL.Wrapper;

namespace Seagull
{
  public class OrientedBoundingBox : GH_TaskCapableComponent<OrientedBoundingBox.SolveResults>
  {
    /// <summary>
    /// Each implementation of GH_Component must provide a public 
    /// constructor without any arguments.
    /// Category represents the Tab in which the component will appear, 
    /// Subcategory the panel. If you use non-existing tab or panel names, 
    /// new tabs/panels will automatically be created.
    /// </summary>
    public OrientedBoundingBox()
      : base("Oriented Bounding Box", "OBB",
        "Computes the oriented bounding box of a mesh or meshes.",
        "Mesh", "Analysis")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
      
      pManager.AddMeshParameter("Mesh", "M", "Input mesh or meshes for calculation.", GH_ParamAccess.item);

    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
      pManager.AddBrepParameter("Oriented Bounding Box", "OBB", "The computed bounding boxes as breps.", GH_ParamAccess.item);
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
    /// to store data in output parameters.</param>
    public class SolveResults
    {
      public Brep BoundingBox { get; set;}
    }

    private static SolveResults ComputeBoundingBox(Mesh mesh){
      SolveResults result = new SolveResults();
      result.BoundingBox = PolygonMeshProcessing.ObbAsBrep(mesh);
      return result;
    }

    protected override void SolveInstance(IGH_DataAccess DA)
    {

      if (InPreSolve)
      {
        Mesh mesh = null;
        DA.GetData(0, ref mesh);
        TaskList.Add(Task.Run(() => ComputeBoundingBox(mesh), CancelToken));
        return;
      }

      if (!GetSolveResults(DA, out SolveResults result))
      {
        Mesh mesh = null;
        if (!DA.GetData(0, ref mesh))
          return;
        result = ComputeBoundingBox(mesh);
      }

      if (null != result)
      {
        DA.SetData(0, result.BoundingBox);
      }

    }

    public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

    /// <summary>
    /// Provides an Icon for every component that will be visible in the User Interface.
    /// Icons need to be 24x24 pixels.
    /// You can add image files to your project resources and access them like this:
    /// return Resources.IconForThisComponent;
    /// </summary>
    protected override System.Drawing.Bitmap Icon => IconLoader.ObbIcon;

    /// <summary>
    /// Each component must have a unique Guid to identify it. 
    /// It is vital this Guid doesn't change otherwise old ghx files 
    /// that use the old ID will partially fail during loading.
    /// </summary>
    public override Guid ComponentGuid => new Guid("45e25ba0-0a9d-4748-b62b-af322f2ede3e");
  }
}