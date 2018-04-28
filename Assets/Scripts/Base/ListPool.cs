using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    internal static class ListPool<T>
    {
        private static readonly ObjectPool<List<T>> s_ListPool;

        [CompilerGenerated]
        private static UnityAction<List<T>> cache;

		private static void Clear(List<T> l)
        {
            l.Clear();
        }

        public static List<T> Get()
        {
            return s_ListPool.Get();
        }

        public static void Release(List<T> toRelease)
        {
            s_ListPool.Release(toRelease);
        }

        static ListPool()
        {
            // 注意: 此类型已标记为 'beforefieldinit'.
            UnityAction<List<T>> arg_1E_0 = null;
            if (cache == null)
			{
                cache = new UnityAction<List<T>>(Clear);
            }
            s_ListPool = new ObjectPool<List<T>>(arg_1E_0, cache);
        }
    }
}
