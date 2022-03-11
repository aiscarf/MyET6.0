// using System;
// using System.Collections;
// using System.Collections.Generic;
//
// namespace ET
// {
//     public class IndexedSet<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
//     {
//         private readonly List<T> m_List = new List<T>();
//         private readonly Dictionary<T, int> m_Dictionary = new Dictionary<T, int>();
//
//         public void Add(T item)
//         {
//             this.m_List.Add(item);
//             this.m_Dictionary.Add(item, this.m_List.Count - 1);
//         }
//
//         public bool AddUnique(T item)
//         {
//             if (this.m_Dictionary.ContainsKey(item))
//                 return false;
//             this.m_List.Add(item);
//             this.m_Dictionary.Add(item, this.m_List.Count - 1);
//             return true;
//         }
//
//         public bool Remove(T item)
//         {
//             int index = -1;
//             if (!this.m_Dictionary.TryGetValue(item, out index))
//                 return false;
//             this.RemoveAt(index);
//             return true;
//         }
//
//         public IEnumerator<T> GetEnumerator()
//         {
//             return this.m_List.GetEnumerator();
//         }
//
//         IEnumerator IEnumerable.GetEnumerator()
//         {
//             return (IEnumerator)this.GetEnumerator();
//         }
//
//         public void Clear()
//         {
//             this.m_List.Clear();
//             this.m_Dictionary.Clear();
//         }
//
//         public bool Contains(T item)
//         {
//             return this.m_Dictionary.ContainsKey(item);
//         }
//
//         public void CopyTo(T[] array, int arrayIndex)
//         {
//             this.m_List.CopyTo(array, arrayIndex);
//         }
//
//         public int Count
//         {
//             get { return this.m_List.Count; }
//         }
//
//         public bool IsReadOnly
//         {
//             get { return false; }
//         }
//
//         public int IndexOf(T item)
//         {
//             int num = -1;
//             this.m_Dictionary.TryGetValue(item, out num);
//             return num;
//         }
//
//         public void Insert(int index, T item)
//         {
//             throw new NotSupportedException(
//                 "Random Insertion is semantically invalid, since this structure does not guarantee ordering.");
//         }
//
//         public void RemoveAt(int index)
//         {
//             this.m_Dictionary.Remove(this.m_List[index]);
//             if (index == this.m_List.Count - 1)
//             {
//                 this.m_List.RemoveAt(index);
//             }
//             else
//             {
//                 int index1 = this.m_List.Count - 1;
//                 T index2 = this.m_List[index1];
//                 this.m_List[index] = index2;
//                 this.m_Dictionary[index2] = index;
//                 this.m_List.RemoveAt(index1);
//             }
//         }
//
//         public T this[int index]
//         {
//             get { return this.m_List[index]; }
//             set
//             {
//                 T key = this.m_List[index];
//                 this.m_Dictionary.Remove(key);
//                 this.m_List[index] = value;
//                 this.m_Dictionary.Add(key, index);
//             }
//         }
//
//         public void RemoveAll(Predicate<T> match)
//         {
//             int index = 0;
//             while (index < this.m_List.Count)
//             {
//                 T obj = this.m_List[index];
//                 if (match(obj))
//                     this.Remove(obj);
//                 else
//                     ++index;
//             }
//         }
//
//         public void Sort(Comparison<T> sortLayoutFunction)
//         {
//             this.m_List.Sort(sortLayoutFunction);
//             for (int index = 0; index < this.m_List.Count; ++index)
//                 this.m_Dictionary[this.m_List[index]] = index;
//         }
//     }
// }