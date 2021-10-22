using System;
using System.Collections.Generic;

namespace Unity.Netcode
{
    internal struct BufferSerializerWriter : IReaderWriter
    {
        private FastBufferWriter m_Writer;

        public BufferSerializerWriter(FastBufferWriter writer)
        {
            m_Writer = writer;
        }

        public bool IsReader => false;
        public bool IsWriter => true;

        public FastBufferReader GetFastBufferReader()
        {
            throw new InvalidOperationException("Cannot retrieve a FastBufferReader from a serializer where IsReader = false");
        }

        public FastBufferWriter GetFastBufferWriter()
        {
            return m_Writer;
        }

        public void SerializeValue(ref string s, bool oneByteChars = false)
        {
            m_Writer.WriteValueSafe(s, oneByteChars);
        }

        public void SerializeValue<T>(ref T[] array) where T : unmanaged
        {
            m_Writer.WriteValueSafe(array);
        }
        public void SerializeValue<T>(ref List<T> array) where T : unmanaged
        {
	        m_Writer.WriteValueSafe(array.ToArray());
        }

        public void SerializeValue(ref byte value)
        {
            m_Writer.WriteByteSafe(value);
        }

        public void SerializeValue<T>(ref T value) where T : unmanaged
        {
            m_Writer.WriteValueSafe(value);
        }

        public void SerializeNetworkSerializable<T>(ref T value) where T : INetworkSerializable, new()
        {
            m_Writer.WriteNetworkSerializable(value);
        }
        public void SerializeNetworkSerializable<T>(ref T[] value) where T : INetworkSerializable, new()
        {
	        m_Writer.WriteNetworkSerializable(value);
        }
        public void SerializeNetworkSerializable<T>(ref List<T> value) where T : INetworkSerializable, new()
        {	 
	        var bufferSerializer = new BufferSerializer<BufferSerializerWriter>(new BufferSerializerWriter(m_Writer));
	        var sizeInTs         = value.Count;
	        m_Writer.WriteValueSafe(sizeInTs);

	        foreach (var v in value)
	        {
		        v.NetworkSerialize(bufferSerializer);
	        }
        }

        public bool PreCheck(int amount)
        {
            return m_Writer.TryBeginWrite(amount);
        }

        public void SerializeValuePreChecked(ref string s, bool oneByteChars = false)
        {
            m_Writer.WriteValue(s, oneByteChars);
        }

        public void SerializeValuePreChecked<T>(ref T[] array) where T : unmanaged
        {
            m_Writer.WriteValue(array);
        }
        public void SerializeValuePreChecked<T>(ref List<T> array) where T : unmanaged
        {
	        m_Writer.WriteValue(array.ToArray());
        }

        public void SerializeValuePreChecked(ref byte value)
        {
            m_Writer.WriteByte(value);
        }

        public void SerializeValuePreChecked<T>(ref T value) where T : unmanaged
        {
            m_Writer.WriteValue(value);
        }
    }
}
