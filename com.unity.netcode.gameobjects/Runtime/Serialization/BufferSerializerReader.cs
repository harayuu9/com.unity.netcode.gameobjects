using System;
using System.Collections.Generic;

namespace Unity.Netcode
{
    internal struct BufferSerializerReader : IReaderWriter
    {
        private FastBufferReader m_Reader;

        public BufferSerializerReader(FastBufferReader reader)
        {
            m_Reader = reader;
        }

        public bool IsReader => true;
        public bool IsWriter => false;

        public FastBufferReader GetFastBufferReader()
        {
            return m_Reader;
        }

        public FastBufferWriter GetFastBufferWriter()
        {
            throw new InvalidOperationException("Cannot retrieve a FastBufferWriter from a serializer where IsWriter = false");
        }

        public void SerializeValue(ref string s, bool oneByteChars = false)
        {
            m_Reader.ReadValueSafe(out s, oneByteChars);
        }

        public void SerializeValue<T>(ref T[] array) where T : unmanaged
        {
            m_Reader.ReadValueSafe(out array);
        }
        public void SerializeValue<T>(ref List<T> list) where T : unmanaged
        {
	        m_Reader.ReadValueSafe(out T[] array);
	        list = new List<T>(array);
        }

        public void SerializeValue(ref byte value)
        {
            m_Reader.ReadByteSafe(out value);
        }

        public void SerializeValue<T>(ref T value) where T : unmanaged
        {
            m_Reader.ReadValueSafe(out value);
        }

        public void SerializeNetworkSerializable<T>(ref T value) where T : INetworkSerializable, new()
        {
            m_Reader.ReadNetworkSerializable(out value);
        }
        public void SerializeNetworkSerializable<T>(ref T[]     value) where T : INetworkSerializable, new()
        {
	        m_Reader.ReadNetworkSerializable(out value);
        }
        public void SerializeNetworkSerializable<T>(ref List<T> value) where T : INetworkSerializable, new()
        {
	        m_Reader.ReadNetworkSerializable(out T[] array);
	        value = new List<T>(array);
        }

        public bool PreCheck(int amount)
        {
            return m_Reader.TryBeginRead(amount);
        }

        public void SerializeValuePreChecked(ref string s, bool oneByteChars = false)
        {
            m_Reader.ReadValue(out s, oneByteChars);
        }

        public void SerializeValuePreChecked<T>(ref T[] array) where T : unmanaged
        {
            m_Reader.ReadValue(out array);
        }
        public void SerializeValuePreChecked<T>(ref List<T> array) where T : unmanaged
        {
	        throw new NotImplementedException();
        }

        public void SerializeValuePreChecked(ref byte value)
        {
            m_Reader.ReadValue(out value);
        }

        public void SerializeValuePreChecked<T>(ref T value) where T : unmanaged
        {
            m_Reader.ReadValue(out value);
        }
    }
}
