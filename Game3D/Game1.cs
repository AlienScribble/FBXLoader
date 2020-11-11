using Game3D.SkinModels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game3D
{
    public class Game1 : Game
    {
        // DISPLAY
        const int SCREENWIDTH = 1024, SCREENHEIGHT = 768;   // TARGET FORMAT        
        GraphicsDeviceManager graphics;
        GraphicsDevice        gpu;
        SpriteBatch           spriteBatch;
        SpriteFont            font;
        static public int     screenW, screenH;
        Camera                cam;

        // RECTANGLES
        Rectangle desktopRect;
        Rectangle screenRect;

        // RENDERTARGETS & TEXTURES
        RenderTarget2D MainTarget;

        // INPUT & UTILS
        Input inp;

        // MODELS & CHARACTERS
        SkinModelLoader   skinModel_loader; // does the work of loading our characters                
        SkinFx            skinFx;           // controls for SkinEffect
        SkinModel[]       hero;             // main character
        const int IDLE = 0, WALK = 1, RUN = 2; // (could use enum but easier to index without casting) 
        Vector3           hero_pos = new Vector3(0, 1, 0);
        Matrix            mtx_hero_rotate;



        //-----------------------
        #region C O N S T R U C T
        //-----------------------
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);            
            Window.IsBorderless   = true;            
            Content.RootDirectory = "Content";
        }
        #endregion



        //-------------
        #region I N I T 
        //-------------
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth    = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight   = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 14;
            graphics.IsFullScreen                = false;
            graphics.PreferredDepthStencilFormat = DepthFormat.None;
            graphics.ApplyChanges();
            Window.Position = new Point(0, 0);            
            gpu = GraphicsDevice;
           
            PresentationParameters pp = gpu.PresentationParameters;
            spriteBatch = new SpriteBatch(gpu);
            MainTarget  = new RenderTarget2D(gpu, SCREENWIDTH, SCREENHEIGHT, false, pp.BackBufferFormat, DepthFormat.Depth24);
            screenW     = MainTarget.Width;
            screenH     = MainTarget.Height;
            desktopRect = new Rectangle(0, 0, pp.BackBufferWidth, pp.BackBufferHeight);
            screenRect  = new Rectangle(0, 0, screenW, screenH);
            
            // INPUT
            inp = new Input(pp, MainTarget);
            // INIT 3D             
            cam = new Camera(gpu, Vector3.Up, inp);
            hero = new SkinModel[3];

            base.Initialize();
        }
        #endregion // init



        //-------------
        #region L O A D
        //-------------
        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("Font");

            // { S K I N - M O D E L   L O A D  ----------------------------------------
            skinFx           = new SkinFx(Content, cam, "SkinEffect");        // skin effect parameter controls     
            skinModel_loader = new SkinModelLoader(Content, gpu);             // need for: runtime load FBX skinned model animations
            skinModel_loader.SetDefaultOptions(0.1f, "default_gray");         // pad the animation a bit for smooth looping, set a debug texture (if no texture on a mesh)   
            
            // load animation (custom settings, size = 35%) 
            hero[IDLE] = skinModel_loader.Load("Kid/kid_idle.fbx", "Kid", true, 3, skinFx, rescale: 0.35f);
            //hero[WALK] = skinModel_loader.Load("Kid/kid_walk.fbx", "Kid", true, 3, skinFx, rescale: 0.35f);
            //hero[RUN]  = skinModel_loader.Load("Kid/kid_run.fbx",  "Kid", true, 3, skinFx, rescale: 0.35f);                       
            // } SKIN-MODEL LOADING    -------------------------------------------------

            // I n i t   P l a y e r:
            mtx_hero_rotate = Matrix.CreateFromYawPitchRoll(MathHelper.Pi, 0, 0); // let's have the character facing the camera at first          
            skinFx.world    = mtx_hero_rotate;
        }
        #endregion // load



        //-----------------
        #region U P D A T E 
        //-----------------
        bool init = true;
        protected override void Update(GameTime gameTime)
        {
            inp.Update();            
            if (init) {    // INITIALIZES STARTING ANIMATIONS, CHARACTERS, AND LEVEL (for whatever level we're on) ____________________________ 
                hero[IDLE].BeginAnimation(0, gameTime);  // begin playing animation
                init = false; 
            }// I N I T _______________________________________________________________________________________________________________________

            if (inp.back_down || inp.KeyDown(Keys.Escape)) Exit(); // change to menu for exit later

            cam.Update_Player_Cam(hero_pos);
            hero[IDLE].Update(gameTime);
            //hero[WALK].Update(gameTime);
            //hero[RUN].Update(gameTime);

            base.Update(gameTime);
        }
        #endregion // update




        #region  S E T  3 D  S T A T E S  ----------------
        RasterizerState rs_ccw = new RasterizerState() { FillMode = FillMode.Solid, CullMode = CullMode.CullCounterClockwiseFace };
        void Set3DStates()
        {
            gpu.BlendState        = BlendState.NonPremultiplied;
            gpu.DepthStencilState = DepthStencilState.Default;
            if (gpu.RasterizerState.CullMode == CullMode.None) gpu.RasterizerState = rs_ccw;
        }
        #endregion




        //--------------
        #region D R A W 
        //--------------
        protected override void Draw(GameTime gameTime)
        {
            gpu.SetRenderTarget(MainTarget);
            gpu.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Transparent, 1.0f, 0);

            Set3DStates();

            //hero[IDLE].Draw(cam, skinFx.world);  // normal way

            // RENDER CHARACTER                    // specialized way
            SkinModel kid = hero[IDLE];
            for (int i = 0; i < kid.meshes.Length; i++)
            {
                SkinModel.SkinMesh mesh = kid.meshes[i];
                if (mesh.opacity < 0.6f) continue;
                skinFx.SetDiffuseCol(Color.White.ToVector4());
                skinFx.SetSpecularCol(new Vector3(0.2f, 0.3f, 0.05f));
                skinFx.SetSpecularPow(256f);
                skinFx.world = mtx_hero_rotate * Matrix.CreateTranslation(hero_pos); // ***MUST DO THIS BEFORE: SET DRAW PARAMS***
                // (If we wanted, we could swap out a shirt or something by setting skinFx.texture = ...)
                // TO DO: set up a DrawMesh that takes a custom transforms list for animation blending
                kid.DrawMesh(i, cam, skinFx.world, false, false);
            }
            //RENDER SHINY TRANSPARENT STUFF(eyes )
            skinFx.SetShineAmplify(100f);
            for (int i = 0; i < kid.meshes.Length; i++)
            {
                SkinModel.SkinMesh mesh = kid.meshes[i];
                if (mesh.opacity >= 0.6f) continue;
                // Make adjustments for eyes: 
                float oldAlpha = skinFx.alpha;
                skinFx.alpha = 0.2f;
                skinFx.SetDiffuseCol(Color.Blue.ToVector4());
                skinFx.SetSpecularCol(new Vector3(100f, 100f, 100f));
                // TO DO: custom DrawMesh that takes a custom blendTransform
                hero[IDLE].DrawMesh(i, cam, skinFx.world, false);
                skinFx.alpha = oldAlpha;
            }
            skinFx.SetShineAmplify(1f);



            // DRAW MAINTARGET TO BACKBUFFER -------------------------------------------------------------------------------------------------------
            gpu.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone);
            spriteBatch.Draw(MainTarget, desktopRect, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        #endregion // draw
    }
}
