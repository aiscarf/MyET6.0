using System;
using System.Collections.Generic;

[assembly: Sirenix.Serialization.RegisterFormatter(typeof(Sirenix.Serialization.UnitySerializeDictionaryFormatter<,>))]
[assembly: Sirenix.Serialization.RegisterFormatter(typeof(Sirenix.Serialization.UnitySerializeDictionaryFormatter<,>))]
namespace Sirenix.Serialization
{
    public sealed class UnitySerializeDictionaryFormatter<TKey, TValue> : BaseFormatter<UnitySerializedDictionary<TKey, TValue>>
  {
    private static readonly bool KeyIsValueType = typeof (TKey).IsValueType;
    private static readonly Serializer<IEqualityComparer<TKey>> EqualityComparerSerializer = Serializer.Get<IEqualityComparer<TKey>>();
    private static readonly Serializer<TKey> KeyReaderWriter = Serializer.Get<TKey>();
    private static readonly Serializer<TValue> ValueReaderWriter = Serializer.Get<TValue>();

    static UnitySerializeDictionaryFormatter()
    {
      UnitySerializeDictionaryFormatter<int, string> dictionaryFormatter = new UnitySerializeDictionaryFormatter<int, string>();
    }

    protected override UnitySerializedDictionary<TKey, TValue> GetUninitializedObject() => (UnitySerializedDictionary<TKey, TValue>) null;

    protected override void DeserializeImplementation(
      ref UnitySerializedDictionary<TKey, TValue> value,
      IDataReader reader)
    {
      string name;
      EntryType entryType = reader.PeekEntry(out name); 
      IEqualityComparer<TKey> comparer = (IEqualityComparer<TKey>) null;
      if (name == "comparer" || entryType != EntryType.StartOfArray)
      {
        comparer = UnitySerializeDictionaryFormatter<TKey, TValue>.EqualityComparerSerializer.ReadValue(reader);
        entryType = reader.PeekEntry(out name);
      }
      if (entryType == EntryType.StartOfArray)
      {
        try
        {
          long length;
          reader.EnterArray(out length);
          value = comparer == null ? new UnitySerializedDictionary<TKey, TValue>((int) length) : new UnitySerializedDictionary<TKey, TValue>((int) length, comparer);
          this.RegisterReferenceID(value, reader);
          for (int index = 0; (long) index < length; ++index)
          {
            if (reader.PeekEntry(out name) == EntryType.EndOfArray)
            {
              reader.Context.Config.DebugContext.LogError("Reached end of array after " + (object) index + " elements, when " + (object) length + " elements were expected.");
              break;
            }
            bool flag = true;
            try
            {
              reader.EnterNode(out Type _);
              TKey key = UnitySerializeDictionaryFormatter<TKey, TValue>.KeyReaderWriter.ReadValue(reader);
              TValue obj = UnitySerializeDictionaryFormatter<TKey, TValue>.ValueReaderWriter.ReadValue(reader);
              if (!UnitySerializeDictionaryFormatter<TKey, TValue>.KeyIsValueType && (object) key == null)
              {
                reader.Context.Config.DebugContext.LogWarning("Dictionary key of type '" + typeof (TKey).FullName + "' was null upon deserialization. A key has gone missing.");
                continue;
              }
              value[key] = obj;
            }
            catch (SerializationAbortException ex)
            {
              flag = false;
              throw ex;
            }
            catch (Exception ex)
            {
              reader.Context.Config.DebugContext.LogException(ex);
            }
            finally
            {
              if (flag)
                reader.ExitNode();
            }
            if (!reader.IsInArrayNode)
            {
              reader.Context.Config.DebugContext.LogError("Reading array went wrong. Data dump: " + reader.GetDataDump());
              break;
            }
          }
        }
        finally
        {
          reader.ExitArray();
        }
      }
      else
        reader.SkipEntry();
    }

    protected override void SerializeImplementation(
      ref UnitySerializedDictionary<TKey, TValue> value,
      IDataWriter writer)
    {
      try 
      {
        if (value.Comparer != null)
          UnitySerializeDictionaryFormatter<TKey, TValue>.EqualityComparerSerializer.WriteValue("comparer", value.Comparer, writer);
        writer.BeginArrayNode((long) value.Count);
        foreach (KeyValuePair<TKey, TValue> keyValuePair in value)
        {
          bool flag = true;
          try
          {
            writer.BeginStructNode((string) null, (Type) null);
            UnitySerializeDictionaryFormatter<TKey, TValue>.KeyReaderWriter.WriteValue("$k", keyValuePair.Key, writer);
            UnitySerializeDictionaryFormatter<TKey, TValue>.ValueReaderWriter.WriteValue("$v", keyValuePair.Value, writer);
          }
          catch (SerializationAbortException ex)
          {
            flag = false;
            throw ex;
          }
          catch (Exception ex)
          {
            writer.Context.Config.DebugContext.LogException(ex);
          }
          finally
          {
            if (flag)
              writer.EndNode((string) null);
          }
        }
      }
      finally
      {
        writer.EndArrayNode();
      }
    }
  }
}