using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Hypersonic.Services
{
   public class TypeBuilder
   {

       /// <summary>
       /// Hydrates the type.
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="reader">The reader.</param>
       /// <returns></returns>
       public T HydrateType<T>(INullableReader reader) where T : class, new()
       {
           T instance = new T();
           Type type = instance.GetType();

           PropertyInfo[] propertyInfos = type.GetProperties();
           IEnumerable<NestedClass> flattenObject = FlattenObject(instance, propertyInfos);

           PopulateObject(flattenObject, reader);
           return instance;
       }

       /// <summary>
       /// Populates the object.
       /// </summary>
       /// <param name="nestedClasses">The nested classes.</param>
       /// <param name="reader">The reader.</param>
       private static void PopulateObject(IEnumerable<NestedClass> nestedClasses, INullableReader reader)
       {
           for (int index = 0; index < reader.FieldCount; index++)
           {
               string name = reader.GetName(index);

               foreach (NestedClass nestedClass in nestedClasses)
               {
                   bool valueFound = false;

                   foreach (PropertyInfoDecorator info in nestedClass.Properties.Where(info => string.Equals((string.IsNullOrEmpty(info.Alias) ? info.Property.Name : info.Alias), name, StringComparison.InvariantCultureIgnoreCase)))
                   {
                       object value = reader.GetValue(name);

                       if (value != DBNull.Value)
                       {
                           SetValue(nestedClass, info.Property, value);
                       }
                       else
                       {
                           if (info.Property.PropertyType.IsValueType && 
                               value == DBNull.Value && 
                               Nullable.GetUnderlyingType(info.Property.PropertyType) == null)
                           {
                               throw new InvalidOperationException(string.Format("Can't cast the property {0}, which is of type '{1}', to a 'null' value", info.Property.Name, info.Property.PropertyType.Name));
                           }
                       }

                       valueFound = true;
                       break;
                   }

                   if(valueFound)
                   {
                       break;
                   }
               }
           }
       }

       /// <summary>
       /// Sets the value.
       /// </summary>
       /// <param name="nestedClass">The nested class.</param>
       /// <param name="info">The info.</param>
       /// <param name="value">The value.</param>
       private static void SetValue(NestedClass nestedClass, PropertyInfo info, object value)
       {
           if (value != DBNull.Value)
           {
               if (info.PropertyType.IsEnum)
               {
                   info.SetValue(nestedClass.Instance, Enum.Parse(info.PropertyType, value.ToString(), true), null);
               }
               else
               {
                   object val = info.PropertyType.IsGenericType &&
                                info.PropertyType.GetGenericTypeDefinition() == typeof (Nullable<>)
                                    ? Convert.ChangeType(value, Nullable.GetUnderlyingType(info.PropertyType))
                                    : Convert.ChangeType(value, info.PropertyType);

                   info.SetValue(nestedClass.Instance, val, null);
               }
           }
           else
           {
               ThrowExceptionIfNullValueType(value, info);
           }
       }

       /// <summary>
       /// Throws the type of the exception if null value.
       /// </summary>
       /// <param name="value">The value.</param>
       /// <param name="info">The info.</param>
       private static void ThrowExceptionIfNullValueType(object value, PropertyInfo info)
       {
           if (info.PropertyType.IsValueType &&
               value == DBNull.Value &&
               Nullable.GetUnderlyingType(info.PropertyType) == null)
           {
               throw new InvalidOperationException(string.Format("Can't cast the property {0}, which is of type '{1}', to a 'null' value", info.Name, info.PropertyType.Name));
           }
       }

       /// <summary>
       /// Determines whether the specified o is collection.
       /// </summary>
       /// <param name="type">The type.</param>
       /// <returns>
       /// 	<c>true</c> if the specified o is collection; otherwise, <c>false</c>.
       /// </returns>
       private static bool IsCollection(Type type)
       {
           bool isCollection = (typeof(ICollection).IsAssignableFrom(type)
               || typeof(ICollection<>).IsAssignableFrom(type));

           return isCollection;
       }

       /// <summary>
       /// Flattens the object.
       /// </summary>
       /// <param name="instance">The instance.</param>
       /// <param name="propertyInfos">The property infos.</param>
       /// <returns></returns>
       private static IEnumerable<NestedClass> FlattenObject(object instance, PropertyInfo[] propertyInfos)
       {
           AttributeService attributeService = new AttributeService();
           List<NestedClass> classes = new List<NestedClass> { new NestedClass { Instance = instance, Properties = attributeService.SetAlias(propertyInfos) } };

           foreach (PropertyInfo info in propertyInfos)
           {
               if (info.PropertyType.IsClass && info.PropertyType != typeof(string) && !IsCollection(info.PropertyType))
               {
                   object propertyInstance = Activator.CreateInstance(info.PropertyType);
                   
                   info.SetValue(instance, propertyInstance, null);
                   PropertyInfo[] properties = propertyInstance.GetType().GetProperties();

                   if (!IsCollection(info.PropertyType))
                   {
                       IEnumerable<NestedClass> flattenObject = FlattenObject(propertyInstance, properties);
                       classes.AddRange(flattenObject);
                   }
               }
           }

           return classes;
       }
       
       private class NestedClass
       {
           /// <summary>
           /// Gets or sets the instance.
           /// </summary>
           /// <value>The instance.</value>
           public object Instance { get; set; }

           /// <summary>
           /// Gets or sets the properties.
           /// </summary>
           /// <value>The properties.</value>
           public PropertyInfoDecorator[] Properties { get; set; }
       }
   }
}
