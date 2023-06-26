using UnityEngine.UI;

namespace ZGH.Core
{
    public class NoDrawingRayCast : Graphic
    {
        protected NoDrawingRayCast()
        {
            useLegacyMeshGeneration = false;
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }
    }
}