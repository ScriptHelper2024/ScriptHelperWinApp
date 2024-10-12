using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Helpers
{
    public static class ListHelpers
    {
        public static void MergeLists<TTarget>(IList Source, IList<TTarget> Target,
            Func<object, TTarget> createMethod, Action<object, TTarget> updateMethod)
            where TTarget : class
        {
            if (Source.Count > Target.Count)
            {
                // adding
                int start = Target.Count;
                for (int index = start; index < Source.Count; index++)
                {
                    var item = Source[index];
                    var child = createMethod(item);
                    Target.Add(child);
                }
                if (start > 0)
                {
                    for (int index = 0; index < start; index++)
                    {
                        var item = Source[index];
                        updateMethod(item, Target[index]);
                    }
                }
            }
            else if (Source.Count < Target.Count)
            {
                if (Source.Count == 0)
                {
                    Target.Clear();
                    return;
                }

                // removing
                while (Target.Count > Source.Count)
                    Target.RemoveAt(Target.Count - 1);

                for (int index = 0; index < Source.Count; index++)
                {
                    var item = Source[index];
                    updateMethod(item, Target[index]);
                }
            }
            else
            {
                for (int index = 0; index < Source.Count; index++)
                {
                    var item = Source[index];
                    updateMethod(item, Target[index]);
                }
            }
        }

        public static void UpdateList<TTarget>(IList<TTarget> Source, IList<TTarget> Target)
            where TTarget : class
        {
            var toBeRemoved = Target.Where(x => !Source.Contains(x)).ToList();
            foreach (var item in toBeRemoved)
            {
                Target.Remove(item);
            }
            if (Target.Count == 0)
            {

            }
            int index = 0;
            foreach (var item in Source)
            {
                if (Target.Contains(item))
                    index = Target.IndexOf(item) + 1;
                else
                {
                    Target.Insert(index, item);
                    index++;
                }
            }
        }

       
        public static bool HasData(this ICollection list)
        {
            return list != null && list.Count > 0;
        }

        public static bool HasItems(this ICollection list)
        {
            return list.HasData();
        }

        public static bool IsEmpty(this ICollection list)
        {
            return list == null || list.Count == 0;
        }

    }
}
