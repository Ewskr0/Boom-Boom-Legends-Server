using System.Collections.Generic;
using System;
using System.Text;
using System.Numerics;

namespace BBL.Utils.ByteBuffer
{
    public class ByteBuffer : IDisposable
    {
        private List<byte> Buff;
        private byte[] readBuff;
        private int readPos;
        private int fixedReadPos = 0;
        private bool buffUpdated = false;

        public ByteBuffer()
        {
            Buff = new List<byte>();
            readPos = 0;
        }
        public int GetReadPos()
        {
            return readPos;
        }
        public byte[] ToArray()
        {
            return Buff.ToArray();
        }
        public int Count()
        {
            return Buff.Count;
        }
        public int Length()
        {
            return Count() - readPos;
        }
        public void Clear()
        {
            Buff.Clear();
            readPos = 0;
        }

        public void ResetPosition()
        {
            readPos = fixedReadPos;
        }

        public void ResetPositionTo(int position)
        {
            fixedReadPos = position;
            readPos = position;
        }

        public void FixReadPosition()
        {
            fixedReadPos = readPos;
        }

        public void WriteBytes(byte[] Input)
        {
            Buff.AddRange(Input);
            buffUpdated = true;
        }
        public void WriteShort(short Input)
        {
            Buff.AddRange(BitConverter.GetBytes(Input));
            buffUpdated = true;
        }
        public void WriteInt(int Input)
        {
            Buff.AddRange(BitConverter.GetBytes(Input));
            buffUpdated = true;
        }
        public void WriteFloat(float Input)
        {
            Buff.AddRange(BitConverter.GetBytes(Input));
            buffUpdated = true;
        }
        public void WriteLong(long Input)
        {
            Buff.AddRange(BitConverter.GetBytes(Input));
            buffUpdated = true;
        }
        public void WriteString(string Input)
        {
            Buff.AddRange(BitConverter.GetBytes(Input.Length));
            Buff.AddRange(Encoding.ASCII.GetBytes(Input));
            buffUpdated = true;
        }
        public void WriteVector3(Vector3 input)
        {
            byte[] vectorArray = new byte[sizeof(float) * 3];

            Buffer.BlockCopy(BitConverter.GetBytes(input.X), 0, vectorArray, 0 * sizeof(float), sizeof(float));
            Buffer.BlockCopy(BitConverter.GetBytes(input.Y), 0, vectorArray, 1 * sizeof(float), sizeof(float));
            Buffer.BlockCopy(BitConverter.GetBytes(input.Z), 0, vectorArray, 2 * sizeof(float), sizeof(float));

            Buff.AddRange(vectorArray);
            buffUpdated = true;
        }
        public void WriteVector2(Vector2 input)
        {
            byte[] vectorArray = new byte[sizeof(float) * 3];

            Buffer.BlockCopy(BitConverter.GetBytes(input.X), 0, vectorArray, 0 * sizeof(float), sizeof(float));
            Buffer.BlockCopy(BitConverter.GetBytes(input.Y), 0, vectorArray, 1 * sizeof(float), sizeof(float));

            Buff.AddRange(vectorArray);
            buffUpdated = true;
        }

        public int ReadInt(bool Peek = true)
        {
            if (Buff.Count > readPos)
            {
                if (buffUpdated)
                {
                    readBuff = Buff.ToArray();
                    buffUpdated = false;
                }
                int ret = BitConverter.ToInt32(readBuff, readPos);
                if (Peek & Buff.Count > readPos)
                    readPos += 4;
                return ret;
            }
            else
            {
                throw new Exception("ByteBuffer is past the limit!");
            }
        }
        public byte[] ReadBytes(int Length, bool Peek = true)
        {
            if (buffUpdated)
            {
                readBuff = Buff.ToArray();
                buffUpdated = false;
            }
            byte[] ret = Buff.GetRange(readPos, Length).ToArray();
            if (Peek)
                readPos += Length;
            return ret;
        }
        public short ReadShort(bool Peek = true)
        {
            if (Buff.Count > readPos)
            {
                if (buffUpdated)
                {
                    readBuff = Buff.ToArray();
                    buffUpdated = false;
                }
                short ret = BitConverter.ToInt16(readBuff, readPos);
                if (Peek & Buff.Count > readPos)
                    readPos += 2;
                return ret;
            }
            else
            {
                throw new Exception("ByteBuffer is past the limit!");
            }
        }
        public float ReadFloat(bool Peek = true)
        {
            if (Buff.Count > readPos)
            {
                if (buffUpdated)
                {
                    readBuff = Buff.ToArray();
                    buffUpdated = false;
                }
                float ret = BitConverter.ToSingle(readBuff, readPos);
                if (Peek & Buff.Count > readPos)
                    readPos += 4;
                return ret;
            }
            else
            {
                throw new Exception("ByteBuffer is past the limit");
            }
        }
        public long ReadLong(bool Peek = true)
        {
            if (Buff.Count > readPos)
            {
                if (buffUpdated)
                {
                    readBuff = Buff.ToArray();
                    buffUpdated = false;
                }
                long ret = BitConverter.ToInt64(readBuff, readPos);
                if (Peek & Buff.Count > readPos)
                    readPos += 8;
                return ret;
            }
            else
            {
                throw new Exception("ByteBuffer is past the limit");
            }
        }
        public string ReadString(bool Peek = true)
        {
            int Len = ReadInt(true);
            if (buffUpdated)
            {
                readBuff = Buff.ToArray();
                buffUpdated = false;
            }
            string ret = Encoding.ASCII.GetString(readBuff, readPos, Len);
            if (Peek & Buff.Count > readPos)
            {
                if (ret.Length > 0)
                    readPos += Len;
            }
            return ret;
        }

        public Vector2 ReadVector2(bool Peek = true)
        {
            if (buffUpdated)
            {
                readBuff = Buff.ToArray();
                buffUpdated = false;
            }

            byte[] value = Buff.GetRange(readPos, sizeof(float) * 2).ToArray();

            Vector2 vector2;
            vector2.X = BitConverter.ToSingle(value, 0 * sizeof(float));
            vector2.Y = BitConverter.ToSingle(value, 1 * sizeof(float));

            if (Peek)
            {
                readPos += sizeof(float) * 2;
            }
            return vector2;
        }

        public Vector3 ReadVector3(bool Peek = true)
        {
            if (buffUpdated)
            {
                readBuff = Buff.ToArray();
                buffUpdated = false;
            }

            byte[] value = Buff.GetRange(readPos, sizeof(float) * 3).ToArray();

            Vector3 vector3;
            vector3.X = BitConverter.ToSingle(value, 0 * sizeof(float));
            vector3.Y = BitConverter.ToSingle(value, 1 * sizeof(float));
            vector3.Z = BitConverter.ToSingle(value, 2 * sizeof(float));

            if (Peek)
            {
                readPos += sizeof(float) * 3;
            }
            return vector3;
        }

        private bool disposedValue = false; // To detect rendudant calls.

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    Buff.Clear();
                readPos = 0;
            }
            disposedValue = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}