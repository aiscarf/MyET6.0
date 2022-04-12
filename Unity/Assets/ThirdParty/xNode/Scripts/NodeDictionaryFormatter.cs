using System;
using System.Collections.Generic;
using XNode;

[assembly: Sirenix.Serialization.RegisterFormatter(typeof(Sirenix.Serialization.NodeDictionaryFormatter))]
[assembly: Sirenix.Serialization.RegisterFormatter(typeof(Sirenix.Serialization.NodeDictionaryFormatter))]

namespace Sirenix.Serialization
{
    public sealed class NodeDictionaryFormatter : BaseFormatter<Node.NodePortDictionary>
    {
        private static readonly Serializer<IEqualityComparer<string>> EqualityComparerSerializer =
            Serializer.Get<IEqualityComparer<string>>();

        private static readonly Serializer<string> KeyReaderWriter = Serializer.Get<string>();

        private static readonly Serializer<NodePort> ValueReaderWriter = Serializer.Get<NodePort>();


        static NodeDictionaryFormatter()
        {
            NodeDictionaryFormatter nodeDictionaryFormatter = new NodeDictionaryFormatter();
        }

        protected override void DeserializeImplementation(ref Node.NodePortDictionary value, IDataReader reader)
        {
            string name;
            EntryType entryType = reader.PeekEntry(out name);
            if (entryType == EntryType.StartOfArray)
            {
                try
                {
                    long length;
                    reader.EnterArray(out length);
                    value = new Node.NodePortDictionary();
                    this.RegisterReferenceID(value, reader);
                    for (int index = 0; (long)index < length; ++index)
                    {
                        if (reader.PeekEntry(out name) == EntryType.EndOfArray)
                        {
                            reader.Context.Config.DebugContext.LogError("Reached end of array after " + (object)index +
                                                                        " elements, when " + (object)length +
                                                                        " elements were expected.");
                            break;
                        }

                        bool flag = true;
                        try
                        {
                            reader.EnterNode(out Type _);
                            string key = NodeDictionaryFormatter.KeyReaderWriter.ReadValue(reader);
                            NodePort obj = NodeDictionaryFormatter.ValueReaderWriter.ReadValue(reader);
                            if ((object)key == null)
                            {
                                reader.Context.Config.DebugContext.LogWarning("Dictionary key of type '" + "string" +
                                                                              "' was null upon deserialization. A key has gone missing.");
                                continue;
                            }

                            value.keys.Add(key);
                            value.values.Add(obj);
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
                            reader.Context.Config.DebugContext.LogError("Reading array went wrong. Data dump: " +
                                                                        reader.GetDataDump());
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

        protected override void SerializeImplementation(ref Node.NodePortDictionary value, IDataWriter writer)
        {
            try
            {
                writer.BeginArrayNode((long)value.Count);
                foreach (var keyValuePair in value)
                {
                    bool flag = true;
                    try
                    {
                        writer.BeginStructNode((string)null, (Type)null);
                        NodeDictionaryFormatter.KeyReaderWriter.WriteValue("$k", keyValuePair.Key, writer);
                        NodeDictionaryFormatter.ValueReaderWriter.WriteValue("$v", keyValuePair.Value, writer);
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
                            writer.EndNode((string)null);
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