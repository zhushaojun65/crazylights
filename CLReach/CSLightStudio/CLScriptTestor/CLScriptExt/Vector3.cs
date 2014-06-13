﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CLScriptExt
{

    struct Vector3
    {

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x + right.x, left.y + right.y, left.z + right.z);
        }
        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x - right.x, left.y - right.y, left.z - right.z);
        }
        public static bool operator ==(Vector3 left, Vector3 right)
        {
            return (left.x == right.x && left.y == right.y && left.z == right.z);
        }
        public static bool operator !=(Vector3 left, Vector3 right)
        {
            return !(left.x == right.x && left.y == right.y && left.z == right.z);
        }
        public override string ToString()
        {
            return "(" + x + "," + y + "," + z+")";
        }
        public float x;
        public float y;
        public float z;

        public float len
        {
            get
            {
                return (float)Math.Sqrt(x * x + y * y + z * z);
            }

        }

        public Vector3 Normalized()
        {
            float _len = len;
            return new Vector3(x / _len, y / _len, z / _len);
        }

        public static Vector3 Cross(Vector3 left,Vector3 right)
        {
            return new Vector3(left.y * right.z - left.z * right.y, left.z * right.x - left.x * right.z, left.x * right.y - left.y * right.x);
        }
        public static Vector3 Cross(Vector3 left, float v=0)
        {
            return left;
        }
        public static float Dot(Vector3 left,Vector3 right)
        {
            return left.x * right.x + left.y * right.y + left.z * right.z;
        }
        public static Vector3 One
        {
            get
            {
                return new Vector3(1, 1, 1);
            }
        }
        public static Vector3 Zero
        {
            get
            {
                return new Vector3(0, 0, 0);
            }
        }
        public static string typetag;
        //测试支持性
        public static explicit operator int(Vector3 b) { return 0; } //这是一个显式转换
        public static implicit operator float(Vector3 a) { return 1; } //这是一个隐式转换
    }
}
