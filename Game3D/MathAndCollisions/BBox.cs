using Microsoft.Xna.Framework;

namespace Game3D.MathAndCollisions
{
    class BBox
    {
        Vector3 min, max;

        // CONSTRUCT
        public BBox(Vector3 Min, Vector3 Max)   { min = Min; max = Max; }       
    }
}
