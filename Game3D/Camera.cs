using MathAndCollisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Game3D       // C A M E R A
{
    class Camera
    {
        public float CAM_HEIGHT  = 12;        // default up-distance from player's root position (depends on character size - 80 up in y direction to look at head)
        public float HEAD_OFFSET = 12;
        public const float FAR_PLANE = 2000;  // farthest camera can see (clip out things further away) 

        public Vector3 pos, target;           // camera position, target to look at
        public Matrix  view, proj, view_proj; // viewing/projection transforms used to transform world vertices to screen coordinates relative to camera
        public Vector3 up;         // up direction for camera and world geometry (may depend on imported geometry's up direction [ie: is up -1 or 1 in y direction] 
        float current_angle;       // player-relative angle offset of camera (will explain more later)
        float angle_velocity;      // speed of camera rotation
        float radius = 100.0f;     // distance of camera from player (to look at)
        Vector3 unit_direction;    // direction of camera (normalized to distance of 1 unit) 
        Input inp;                 // allow access to input class so can control camera from this class if want to
        Maf   maf;


        // C O N S T R U C T 
        public Camera(GraphicsDevice gpu, Vector3 UpDirection, Input input)
        {
            up             = UpDirection;
            inp            = input;
            pos            = new Vector3(20, 12, -90);
            target         = Vector3.Zero;
            maf            = new Maf();
            view           = Matrix.CreateLookAt(pos, target, up);
            proj           = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, gpu.Viewport.AspectRatio, 1.0f, FAR_PLANE);
            view_proj      = view * proj;
            unit_direction = view.Forward;   unit_direction.Normalize();
        }


        // M O V E   C A M E R A   (simple manual camera movement [set pos to put at an exact position] - [probably won't use this] )
        public void MoveCamera(Vector3 move)
        {
            pos += move;
            view = Matrix.CreateLookAt(pos, target, up);
            view_proj = view * proj;
        }


        // U P D A T E   T A R G E T   (use this mostly [new_target should be player position usually] - [probably will use] )
        public void UpdateTarget(Vector3 new_target)
        {
            target    = new_target;
            view      = Matrix.CreateLookAt(pos, target, up);
            view_proj = view * proj;
        }



        // U P D A T E    P L A Y E R    C A M 
        public void Update_Player_Cam(Vector3 hero_pos)
        {
            if (target == Vector3.Zero) target = hero_pos;

            float CamPad_LeftRight = inp.gp.ThumbSticks.Right.X;
            float CamPad_UpDown    = inp.gp.ThumbSticks.Right.Y;

            Vector3 forward = hero_pos - pos;  // vector from camera pointing to hero
            float x1 = forward.X;
            float y1 = forward.Y;
            float z1 = forward.Z;

            // GET UP-DOWN LOOK                        
            pos.Y -= (pos.Y - (hero_pos.Y + CAM_HEIGHT)) * 0.06f;                       //pos.Y -= (pos.Y - (hero_pos.Y - CAM_HEIGHT)) * 0.06f;  // for upside-down camera

            if (CamPad_UpDown > Input.DEADZONE || CamPad_UpDown < -Input.DEADZONE)
            {
                if (CamPad_UpDown < -Input.DEADZONE * 2)
                {                    
                    float targ_height = hero_pos.Y + CAM_HEIGHT + 120f;                 //float targ_height = hero_pos.Y - CAM_HEIGHT - 120f;              // flipped camera version      //if (pos.Y > targ_height) pos.Y -= (pos.Y - targ_height) * 0.06f; // "  
                    if (pos.Y < targ_height) pos.Y += (targ_height - pos.Y) * 0.06f;
                }                
                if (CamPad_UpDown > Input.DEADZONE * 2) { if (pos.Y > hero_pos.Y - 5) pos.Y -= (pos.Y - (hero_pos.Y - 5)) * 0.06f; } //if (CamPad_UpDown > 0.0f) { if (pos.Y < hero_pos.Y + 5) pos.Y += (hero_pos.Y + 5 - pos.Y) * 0.06f; } // flipped cam version
            }

            // ROTATE CAMERA (accelerate rotation in a direction)
            if (inp.KeyDown(Keys.OemPeriod))
            {
                angle_velocity -= Maf.RADIANS_QUARTER; if (angle_velocity < -Maf.RADIANS_1) angle_velocity = -Maf.RADIANS_1; // ANALOG ROTATE CAMERA
            }
            if (inp.KeyDown(Keys.OemComma))
            {
                angle_velocity += Maf.RADIANS_QUARTER; if (angle_velocity > Maf.RADIANS_1) angle_velocity = Maf.RADIANS_1;  // ANALOG ROTATE CAMERA
            }
            if ((CamPad_LeftRight > Input.DEADZONE) || (CamPad_LeftRight < -Input.DEADZONE))
            {
                if (CamPad_LeftRight < 0f)
                {
                    angle_velocity -= CamPad_LeftRight * 0.0038f;
                    if (angle_velocity < -Maf.RADIANS_1) angle_velocity = -Maf.RADIANS_1; // ANALOG ROTATE CAMERA
                }
                if (CamPad_LeftRight > 0f)
                {
                    angle_velocity -= CamPad_LeftRight * 0.0038f;
                    if (angle_velocity > Maf.RADIANS_1) angle_velocity = Maf.RADIANS_1;  // ANALOG ROTATE CAMERA
                }
            }
            // G E T   N E W   R O T A T I O N   A N G L E  ( and update camera position )
            radius = (float)Math.Sqrt(x1 * x1 + z1 * z1);
            if (angle_velocity != 0.0f)
            {
                current_angle = maf.Calculate2DAngleFromZero(-x1, -z1);                   // get the angle
                current_angle += angle_velocity;                                          // add aditional angle velocity
                current_angle = maf.ClampAngle(current_angle);
                pos.X = hero_pos.X + radius * (float)Math.Cos(current_angle);             // camera is at hero_pos + radius(distance) at current angle (2D rotation around Y axis)
                pos.Z = hero_pos.Z + radius * (float)Math.Sin(current_angle);
                angle_velocity *= 0.9f;
            }

            // C A M E R A   Z O O M   (move camera toward player if too far away)            
            unit_direction = forward; unit_direction.Normalize();
            float adjust = 0.02f;
            if ((radius > 400) || (radius < 40)) adjust = 1f;
            pos.X += unit_direction.X * (radius - 40f) * adjust;
            pos.Z += unit_direction.Z * (radius - 40f) * adjust;

            target.X += (hero_pos.X - target.X) * 0.1f;
            target.Y += (hero_pos.Y + HEAD_OFFSET - target.Y) * 0.1f;
            target.Z += (hero_pos.Z - target.Z) * 0.1f;
            UpdateTarget(target);
        }

    }
}
