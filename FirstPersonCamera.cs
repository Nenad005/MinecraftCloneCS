using OpenTK.Mathematics;


namespace BasicOpenTK
{
    public class FirstPersonCamera
    {
        private float sens = 0.1f;
        public Vector3 pos;
        private Vector3 up;

        public float Yaw = 0;
        private float Pitch = 0;

        private Matrix4 translate = Matrix4.CreateTranslation(-0.5f, -0.5f, -0.5f);

        public FirstPersonCamera(float x = 0, float y = 0, float z = 0)
        {
            pos = new Vector3(x, y, z);
            up = new Vector3(0, 1, 0);
        }

        private Vector3 TPpos = new Vector3(0f, 0f, 0f);

        public void move(float x, float y, float z)
        {
            Vector2 alpha = new Vector2((float)Math.Cos(MathHelper.DegreesToRadians(Yaw)), (float)Math.Sin(MathHelper.DegreesToRadians(Yaw)));
            Vector2 beta = new Vector2((float)-Math.Sin(MathHelper.DegreesToRadians(Yaw)), (float)Math.Cos(MathHelper.DegreesToRadians(Yaw)));
            Vector2 offset = (z * alpha) + (x * beta);

            pos.X += offset.X;
            pos.Y += y;
            pos.Z += offset.Y;
        }

        public void SetLookDirection(Vector2 mouse)
        {
            Yaw = mouse.X * sens;
            Pitch = -mouse.Y * sens;

            if (Pitch > 89.9f) Pitch = 89.9f;
            if (Pitch < -89.9f) Pitch = -89.9f;
        }

        public Matrix4 GetMatrix()
        {
            Vector3 LookDir = new Vector3(0, 0, 1);

            LookDir.X = (float)Math.Cos(MathHelper.DegreesToRadians(Pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(Yaw));
            LookDir.Y = (float)Math.Sin(MathHelper.DegreesToRadians(Pitch));
            LookDir.Z = (float)Math.Cos(MathHelper.DegreesToRadians(Pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(Yaw));
            LookDir = Vector3.Normalize(LookDir);


            LookDir.X += pos.X;
            LookDir.Y += pos.Y;
            LookDir.Z += pos.Z;

            Matrix4 matrix = translate * Matrix4.LookAt(pos, LookDir, up);
            return matrix;
        }

        public void SetTP()
        {
            TPpos = pos;
        }

        public void SetTP(float x, float y, float z)
        {
            TPpos = new Vector3(x, y, z);
        }

        public void TP()
        {
            pos = TPpos;
        }

        public void PrintPos()
        {
            Console.WriteLine($"Kamera : X : {pos.X:F2}, Y : {pos.Y:F2}, Z : {pos.Z:F2}");
        }
    }
}
