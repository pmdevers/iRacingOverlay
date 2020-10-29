using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iRacingTimings.Shared
{
    public class ClassBuilder<T>
    {
        public static ClassBuilder<T> Create()
        {
            return new ClassBuilder<T>();
        }

        public ClassBuilder<T> If(string className, Func<T, bool> func)
        {
            Rules.Add(new ClassBuilderRuleIf<T>(className, func));
            return this;
        }

        public ClassBuilder<T> Get(Func<T, string> func)
        {
            Rules.Add(new ClassBuilderRuleGet<T>(func));
            return this;
        }

        public ClassBuilder<T> Class(string className)
        {
            Rules.Add(new ClassBuilderRuleClass<T>(className));
            return this;
        }

        public ClassBuilder<T> Dictionary<TK>(Func<T, TK> func, IDictionary<TK, string> dictionary)
        {
            Rules.Add(new ClassBuilderRuleDictionary<T, TK>(func, dictionary));
            return this;
        }


        private readonly List<ClassBuilderRule<T>> Rules = new List<ClassBuilderRule<T>>();

        public string GetClasses(T data)
        {
            return string.Join(" ", Rules.Select(i => i.GetClass(data)).Where(i => i != null));
        }
    }

    public abstract class ClassBuilderRule<T>
    {
        public abstract string GetClass(T data);
    }

    public class ClassBuilderRuleIf<T> : ClassBuilderRule<T>
    {
        public string ClassName { get; set; }
        public Func<T, bool> Func { get; set; }

        public ClassBuilderRuleIf(string className, Func<T, bool> func)
        {
            ClassName = className;
            Func = func;
        }

        public override string GetClass(T data)
        {
            if (Func(data))
            {
                return ClassName;
            }

            return null;
        }
    }

    public class ClassBuilderRuleClass<T> : ClassBuilderRule<T>
    {
        public string ClassName { get; set; }


        public ClassBuilderRuleClass(string className)
        {
            ClassName = className;
        }

        public override string GetClass(T data)
        {
            return ClassName;
        }
    }

    public class ClassBuilderRuleDictionary<T, TK> : ClassBuilderRule<T>
    {
        public IDictionary<TK, string> Dictionary { get; set; }

        public Func<T, TK> Func { get; set; }

        public ClassBuilderRuleDictionary(Func<T, TK> func, IDictionary<TK, string> dictionary)
        {
            Func = func;
            Dictionary = dictionary;
        }

        public override string GetClass(T data)
        {
            var key = Func(data);

            if (Dictionary.TryGetValue(key, out var value))
            {
                return value;
            }

            return null;
        }
    }

    public class ClassBuilderRuleGet<T> : ClassBuilderRule<T>
    {
        public Func<T, string> Func { get; set; }

        public ClassBuilderRuleGet(Func<T, string> func)
        {
            Func = func;
        }

        public override string GetClass(T data)
        {
            return Func(data);
        }
    }
}
