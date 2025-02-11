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
  public class OrientedBoundingBox : GH_Component
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
        "Oriented bounding box of a mesh.",
        "Seagull", "Polygon Mesh Processing")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
      
      pManager.AddMeshParameter("Meshes", "M", "Input mesh(es) for oriented bounding boxes.", GH_ParamAccess.list);

    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
      pManager.AddBrepParameter("OBBs", "OBBs", "The bounding boxes as breps.", GH_ParamAccess.list);
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
    /// to store data in output parameters.</param>
    protected override void SolveInstance(IGH_DataAccess DA)
    {

      List<Mesh> meshes = new List<Mesh>();
      List<Brep> boundingBoxes = new List<Brep>();

      if (!DA.GetDataList(0, meshes))
      {
          return;
      }

       Parallel.For(0, meshes.Count,
        i => {
          boundingBoxes.Add(PolygonMeshProcessing.ObbAsBrep(meshes[i]));
        });

      DA.SetDataList(0, boundingBoxes);

    }

    /// <summary>
    /// Provides an Icon for every component that will be visible in the User Interface.
    /// Icons need to be 24x24 pixels.
    /// You can add image files to your project resources and access them like this:
    /// return Resources.IconForThisComponent;
    /// </summary>
    protected override System.Drawing.Bitmap Icon => null;

    /// <summary>
    /// Each component must have a unique Guid to identify it. 
    /// It is vital this Guid doesn't change otherwise old ghx files 
    /// that use the old ID will partially fail during loading.
    /// </summary>
    public override Guid ComponentGuid => new Guid("74747e0b-8a61-424e-80eb-33a7205d998e");
  }
}