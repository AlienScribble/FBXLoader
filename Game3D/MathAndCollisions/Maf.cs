using Microsoft.Xna.Framework;
using System;

namespace MathAndCollisions
{
    // M A T H   S T U F F  ( i n c l u d i n g    E X T E N S I O N S  - - -  s e e   b e l o w )
    class Maf {
        public const float PI = 3.1415926536f;
        public const float RADIANS_1  = ((float)((double)3.1415926536 / (double)180.0));
        public const float RADIANS_2  = RADIANS_1 * 2.0f;
        public const float RADIANS_3  = RADIANS_1 * 3.0f;
        public const float RADIANS_4  = RADIANS_1 * 4.0f;
        public const float RADIANS_5  = RADIANS_1 * 5.0f;
        public const float RADIANS_6  = RADIANS_1 * 6.0f;
        public const float RADIANS_10 = RADIANS_1 * 10.0f;
        public const float RADIANS_90 = RADIANS_1 * 90.0f;
        public const float RADIANS_180 = RADIANS_1 * 180.0f;
        public const float RADIANS_270 = RADIANS_1 * 270.0f;
        public const float RADIANS_360 = RADIANS_1 * 360.0f;
        public const float RADIANS_HALF = RADIANS_1 / 2.0f;
        public const float RADIANS_QUARTER = RADIANS_1 / 4.0f;
        public const float EPSILON = 0.0001f;


        // C A L C U L A T E  2 D  A N G L E  F R O M  Z E R O
        public float Calculate2DAngleFromZero(float x, float y) {
            if ((x == 0.0f) && (y == 0.0f)) return 0.0f;
            if (x > 0.0f) {
                if (x < EPSILON) x = EPSILON;
                if (y > 0.0f) {
                    if (y < EPSILON) y = EPSILON;             // +x,+y		
                    return (float)Math.Atan((double)(y / x)); // get angle (depends on quadrant so 4 conditions)
                } else {
                    if (y > -EPSILON) y = -EPSILON;           // +x,-y
                    return (RADIANS_270 + (float)Math.Atan((double)(x / (-y))));
                }
            } else {
                if (x > -EPSILON) x = -EPSILON;
                if (y > 0.0f) {
                    if (y < EPSILON) y = EPSILON;             // -x,+y
                    return (RADIANS_90 + (float)Math.Atan((double)((-x) / y)));
                } else {
                    if (y > -EPSILON) y = -EPSILON;           // -x,-y ( = +y/+x)
                    return (RADIANS_180 + (float)Math.Atan((double)(y / x)));
                }
            }
        }//END Calculate2DAngleFromZero


        public float ClampAngle(float angle)
        {
            while (angle > Maf.RADIANS_360) angle -= Maf.RADIANS_360;
            while (angle < 0) angle += Maf.RADIANS_360;
            return angle;
        }
    }



    // M A F   E X T E N S I O N S
    static class MafExtensions
    {
        public static float ToAngle(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }
    }

}
