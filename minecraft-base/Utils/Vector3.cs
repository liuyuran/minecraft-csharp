using System;

namespace Base.Utils {
    [Serializable]
    public struct Vector3 {
        public float X;
        public float Y;
        public float Z;

        public Vector3(float x, float y, float z) {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(System.Numerics.Vector3 vector3) {
            X = vector3.X;
            Y = vector3.Y;
            Z = vector3.Z;
        }

        public static Vector3 operator +(Vector3 a, Vector3 b) {
            return new Vector3(
                a.X + b.X,
                a.Y + b.Y,
                a.Z + b.Z
            );
        }

        public static bool operator ==(Vector3 a, Vector3 b) {
            return Math.Abs(a.X - b.X) < Magic && Math.Abs(a.Y - b.Y) < Magic && Math.Abs(a.Z - b.Z) < Magic;
        }

        public static bool operator !=(Vector3 a, Vector3 b) {
            return Math.Abs(a.X - b.X) > Magic || Math.Abs(a.Y - b.Y) > Magic || Math.Abs(a.Z - b.Z) > Magic;
        }

        public override bool Equals(object? obj) {
            return obj is Vector3 other && Equals(other);
        }

        public bool Equals(Vector3 other) {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }

        public override int GetHashCode() {
            return HashCode.Combine(X, Y, Z);
        }

        private const float Magic = 0.0001f;
        public static Vector3 Zero => new(0, 0, 0);

        public System.Numerics.Vector3 ToNumerics() {
            return new System.Numerics.Vector3(X, Y, Z);
        }
    }
}