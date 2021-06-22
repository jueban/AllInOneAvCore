using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class ReflectHelper
    {
        public static List<string> GetProperties<T>(T t)
        {
            List<string> ListStr = new();
            if (t == null)
            {
                return ListStr;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length <= 0)
            {
                return ListStr;
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name; //名称
                object value = item.GetValue(t, null);  //值

                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    ListStr.Add(name);
                }
                else
                {
                    GetProperties(value);
                }
            }
            return ListStr;
        }

        public static List<string> GetFields<T>(T t)
        {
            List<string> ListStr = new();
            if (t == null)
            {
                return ListStr;
            }
            FieldInfo[] fields = t.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (fields.Length <= 0)
            {
                return ListStr;
            }
            foreach (FieldInfo item in fields)
            {
                string name = item.Name; //名称
                object value = item.GetValue(t);  //值

                if (item.FieldType.IsValueType || item.FieldType.Name.StartsWith("String"))
                {
                    ListStr.Add(name);
                }
                else
                {
                    GetFields(value);
                }
            }
            return ListStr;
        }

        public static (List<PropertyInfo> PorpertyInfos, List<FieldInfo> FieldInfos, List<MethodInfo> MethodInfos) GetInfos<T>() where T : new()
        {
            (List<PropertyInfo> PorpertyInfos, List<FieldInfo> FieldInfos, List<MethodInfo> MethodInfos) ret = new();
            ret.PorpertyInfos = new List<PropertyInfo>();
            ret.FieldInfos = new List<FieldInfo>();
            ret.MethodInfos = new List<MethodInfo>();

            T a = default(T);
            a = new T();
            Type t = a.GetType();

            //都是公共的
            FieldInfo[] fieldInfos = t.GetFields();//字段

            PropertyInfo[] propertyInfos = t.GetProperties();//属性

            MethodInfo[] methodInfos = t.GetMethods();//方法

            foreach (PropertyInfo item in propertyInfos)
            {
                ret.PorpertyInfos.Add(item);
            }

            foreach (FieldInfo item in fieldInfos)
            {
                ret.FieldInfos.Add(item);
            }

            foreach (MethodInfo item in methodInfos)
            {
                ret.MethodInfos.Add(item);
            }

            return ret;
        }
    }
}
