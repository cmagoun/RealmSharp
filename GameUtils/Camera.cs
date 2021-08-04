using Microsoft.Xna.Framework;

namespace GameUtils
{
    //Mostly copied from https://gist.github.com/FaronBracy/25c77fc572794f7bcfe4
    //Thanks to FaronBracy for this easy to use class.
    public class Camera
    {
        public Camera(int viewportWidth, int viewportHeight, Vector2 cameraPosition)
        {
            ViewportWidth = viewportWidth;
            ViewportHeight = viewportHeight;
            Position = cameraPosition;
            Zoom = 1.0f;
        }

        // Centered Position of the Camera.
        public Vector2 Position { get; private set; }
        public float Zoom { get; private set; }
        public float Rotation { get; private set; }

        // height and width of the viewport window which should adjust when the player resizes the game window.
        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }

        // Center of the Viewport does not account for scale
        public Vector2 ViewportCenter
        {
            get
            {
                return new Vector2(ViewportWidth * 0.5f, ViewportHeight * 0.5f);
            }
        }

        // create a matrix for the camera to offset everything we draw, the map and our objects. since the
        // camera coordinates are where the camera is, we offset everything by the negative of that to simulate
        // a camera moving. we also cast to integers to avoid filtering artifacts
        public Matrix TranslationMatrix
        {
            get
            {
                return Matrix.CreateTranslation(-(int)Position.X, -(int)Position.Y, 0) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                   Matrix.CreateTranslation(new Vector3(ViewportCenter, 0));
            }
        }

        public void AdjustZoom(float amount)
        {
            Zoom += amount;
            if (Zoom < 0.1f)
            {
                Zoom = 0.1f;
            }
        }

        public void MoveCamera(Vector2 cameraMovement, bool clampToMap = false)
        {
            Position += cameraMovement;
            //
            // if (clampToMap)
            // {
            //     // clamp the camera so it never leaves the visible area of the map.
            //     var cameraMax = new Vector2(Global.GameModel.Map.Width * Global.CellWidth - ViewportWidth,
            //                                  Global.GameModel.Map.Height * Global.CellHeight - ViewportHeight);
            //
            //     Position = Vector2.Clamp(Position, Vector2.Zero, cameraMax);
            // }
        }

        public Rectangle ViewportWorldBoundry()
        {
            Vector2 viewPortCorner = ScreenToWorld(new Vector2(0, 0));
            Vector2 viewPortBottomCorner = ScreenToWorld(new Vector2(ViewportWidth, ViewportHeight));

            return new Rectangle((int)viewPortCorner.X, (int)viewPortCorner.Y, (int)(viewPortBottomCorner.X - viewPortCorner.X),
                                                   (int)(viewPortBottomCorner.Y - viewPortCorner.Y));
        }
        //
        // public void CenterOn(Vector2 position)
        // {
        //     Position = position;
        // }
        //
        // public void CenterOn(Point location)
        // {
        //     Position = CenteredPosition(location);
        // }
        //
        // private Vector2 CenteredPosition(Point location, bool clampToMap = false)
        // {
        //     var cameraPosition = new Vector2(location.X * Global.CellWidth, location.Y * Global.CellHeight);
        //     var cameraCenteredOnTilePosition = new Vector2(cameraPosition.X + Global.HalfCellWidth,
        //                                                     cameraPosition.Y + Global.HalfCellHeight);
        //     if (clampToMap)
        //     {
        //         // clamp the camera so it never leaves the visible area of the map.
        //         var cameraMax = new Vector2(Global.GameModel.Map.Width * Global.CellWidth - ViewportWidth,
        //                                      Global.GameModel.Map.Height * Global.CellHeight - ViewportHeight);
        //
        //         return Vector2.Clamp(cameraCenteredOnTilePosition, Vector2.Zero, cameraMax);
        //     }
        //
        //     return cameraCenteredOnTilePosition;
        // }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, TranslationMatrix);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(TranslationMatrix));
        }

    }
}
