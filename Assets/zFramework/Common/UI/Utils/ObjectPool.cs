using System;
using System.Collections.Generic;
using System.Text;
using UnityFramework.Utils;

namespace UnityFramework.Utils
{

    public class ObjectPool : Singleton<ObjectPool>, IDisposable
    {
        public const int CAPACITY = 20;

        private Dictionary<Type, int> CapacityDict = null;
        private Dictionary<Type, int> ObjectUsedDict = null;
        private Dictionary<Type, Queue<Object>> ObjectDict = null;
        private Dictionary<Type, List<Object>> ObjectUsedRef = null;
        private ObjectPool()
        {
            ObjectDict = new Dictionary<Type, Queue<object>>();
            ObjectUsedDict = new Dictionary<Type, int>();
            CapacityDict = new Dictionary<Type, int>();
            ObjectUsedRef = new Dictionary<Type, List<object>>();
        }

        public int GetIdleCount<T>()
        {
            Type p = typeof(T);
            if (ObjectDict.ContainsKey(p))
            {
                return ObjectDict[p].Count;
            }
            return 0;
        }

        public Object GetObjectInstance(Type Prototype)
        {
            Object Instance = null;
            if (!ObjectDict.ContainsKey(Prototype))
            {
                CreateObjectPool(Prototype, CAPACITY);
            }
            Queue<Object> Objects = ObjectDict[Prototype];
            List<Object> Used = ObjectUsedRef[Prototype];
            int UsedCount = ObjectUsedDict[Prototype];
            if (Objects.Count > 0)
            {
                //返回第一个
                Instance = Objects.Dequeue();
                //更新使用计数
                UsedCount++;
                ObjectUsedDict[Prototype] = UsedCount;
                //添加到使用中的队列
                Used.Add(Instance);
            }
            else
            {
                //if (UsedCount < Capacity)
                //{
                //创建新的实例
                Instance = Activator.CreateInstance(Prototype);
                //更新使用计数
                UsedCount++;
                ObjectUsedDict[Prototype] = UsedCount;

                //添加到使用中的队列
                Used.Add(Instance);
                //}
            }
            return Instance;
        }

        /**
         * 获取对象实例
         **/
        public T GetObjectInstance<T>() where T : IPoolable, new()
        {
            Type Prototype = typeof(T);
            T ins = (T)GetObjectInstance(Prototype);
            ins.ResetAndClear();
            return ins;
        }

        public void ReturnObject(System.Object obj)
        {
            Type Prototype = obj.GetType();
            if (ObjectDict.ContainsKey(Prototype))
            {
                Queue<Object> Objects;
                List<Object> Used;
                Objects = ObjectDict[Prototype];
                Used = ObjectUsedRef[Prototype];
                int Capacity = CapacityDict[Prototype];
                if (Used.Contains(obj))
                {
                    //更新使用引用计数
                    int UsedCount = ObjectUsedDict[Prototype];
                    UsedCount--;
                    ObjectUsedDict[Prototype] = UsedCount;
                    //从使用队列删除,并且将对象加入未使用队列
                    Used.Remove(obj);
                    if (Objects.Count >= Capacity)
                    {
                        //空闲对象到达阀值则将对象直接丢弃等待垃圾回收
                        ((IPoolable)obj).ResetAndClear();
                    }
                    else
                    {
                        ((IPoolable)obj).ResetAndClear();
                        Objects.Enqueue(obj);
                    }
                }
            }
        }

        /// <summary>
        /// 创建对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="CapacitySize"></param>
        /// <param name="initObjs"></param>
        public void CreateObjectPool<T>(int CapacitySize) where T : IPoolable, new()
        {
            Type Prototype = typeof(T);
            CreateObjectPool(Prototype, CapacitySize);
        }

        /// <summary>
        /// 创建对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="CapacitySize"></param>
        /// <param name="initCreate"></param>
        public void CreateObjectPool<T>(int CapacitySize, bool initCreate) where T : IPoolable, new()
        {
            Type Prototype = typeof(T);
            CreateObjectPool(Prototype, CapacitySize);
            if (initCreate)
            {
                Queue<Object> pool = ObjectDict[Prototype];
                for (int idx = 0; idx < CapacitySize; idx++)
                {
                    object ins = Activator.CreateInstance(Prototype);
                    pool.Enqueue(ins);
                }
            }
        }

        public Boolean IsPoolExists<T>()
        {
            return ObjectDict.ContainsKey(typeof(T));
        }

        /// <summary>
        /// 通过指定的原型创建
        /// </summary>
        /// <param name="prototype">Prototype.</param>
        /// <param name="capacitySize">Capacity size.</param>
        public void CreateObjectPool(Type prototype, int capacitySize, params object[] initObjs)
        {
            if (!ObjectDict.ContainsKey(prototype))
            {
                Queue<Object> Pool = new Queue<Object>(CAPACITY);
                if (null != initObjs)
                {
                    foreach (object obj in initObjs)
                    {
                        Pool.Enqueue(obj);
                    }
                }
                ObjectDict.Add(prototype, Pool);
                ObjectUsedRef.Add(prototype, new List<Object>());
                ObjectUsedDict.Add(prototype, 0);
                CapacityDict.Add(prototype, capacitySize);
            }
        }


        public void DeleteObjectPool<T>() where T : IPoolable, new()
        {
            Type Prototype = typeof(T);
            DeleteObjectPool(Prototype);
        }
        public void DeleteObjectPool(Type type)
        {
            if (!ObjectDict.ContainsKey(type))
            {
                Queue<Object> Pool = new Queue<Object>(CAPACITY);
                IPoolable ins = null;
                while (Pool.Count > 0)
                {
                    ins = (IPoolable)Pool.Dequeue();
                    //ins.Dispose();
                    ins.ResetAndClear();
                }
                Pool.Clear();
                ObjectDict.Remove(type);
            }
        }

        /**
		 * 获得指定类型对象池的状态
		 * 
		 **/
        public string DumpPoolStatus<T>()
        {
            StringBuilder Result = new StringBuilder();
            Type Prototype = typeof(T);
            if (ObjectDict.ContainsKey(Prototype))
            {
                int Capacity = 0;
                CapacityDict.TryGetValue(Prototype, out Capacity);
                Result.AppendLine("Capacity = " + Capacity);
                Queue<Object> Pool;
                ObjectDict.TryGetValue(Prototype, out Pool);
                Result.AppendLine("Idle Object = " + Pool.Count);
                int Used = 0;
                ObjectUsedDict.TryGetValue(Prototype, out Used);
                Result.AppendLine("Used=" + Used);
            }
            return Result.ToString();
        }

        public void Dispose()
        {

        }
    }
}
