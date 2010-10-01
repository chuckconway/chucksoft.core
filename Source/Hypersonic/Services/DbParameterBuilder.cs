using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace Hypersonic.Services
{
    public class DbParameterBuilder<TParameter>
        where TParameter : DbParameter, new()
    {
        private readonly string _parameterDelimiter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbParameterBuilder&lt;TParameter&gt;"/> class.
        /// </summary>
        /// <param name="parameterDelimiter">The parameter delimiter.</param>
        public DbParameterBuilder(string parameterDelimiter)
        {
            _parameterDelimiter = parameterDelimiter;
        }

        /// <summary>
        /// Gets the values from anonymous types.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public List<DbParameter> GetValuesFromType<T>(T parameters)
        {
            Type type = typeof(T);

// ReSharper disable CompareNonConstrainedGenericWithNull
            if (parameters == null) { throw new Exception("Parameters are null."); }
// ReSharper restore CompareNonConstrainedGenericWithNull
            if (type == typeof(string)) { throw new Exception("The type of string is not supported. Strings must be encapsulated in an object as a property."); }
            if (type == typeof(DataSet)) { throw new Exception("DataSets are not supported."); }

            
            List<InstanceAndProperty> instanceAndProperties = new List<InstanceAndProperty>();
            PropertyInfo[] properties = parameters.GetType().GetProperties();
            FlattenObjectGraph(parameters, properties, instanceAndProperties);

            List<DbParameter> dbParameter = GetDbParameters(instanceAndProperties, _parameterDelimiter);
            return dbParameter;
        }

        /// <summary>
        /// Gets the db parameters.
        /// </summary>
        /// <param name="instanceAndProperties">The instance and properties.</param>
        /// <param name="parameterDelimiter">The parameter delimiter.</param>
        /// <returns></returns>
        private static List<DbParameter> GetDbParameters(IEnumerable<InstanceAndProperty> instanceAndProperties, string parameterDelimiter)
        {
            MakeParameterService<TParameter> makeParameterService = new MakeParameterService<TParameter>();

            //foreach (InstanceAndProperty instanceAndProperty in instanceAndProperties)
            //{
            //    string name = parameterDelimiter +
            //                  (string.IsNullOrEmpty(instanceAndProperty.Alias)
            //                       ? instanceAndProperty.PropertyInfo.Name
            //                       : instanceAndProperty.Alias);

            //    object value = instanceAndProperty.PropertyInfo.GetValue(instanceAndProperty.Instance, null);
            //    DbParameter parameter = makeParameterService.MakeParameter(name, value);

            //    dbParameters.Add(parameter);
            //}

            return (from instanceAndProperty in instanceAndProperties
                    let name = parameterDelimiter + (string.IsNullOrEmpty(instanceAndProperty.Alias) ? instanceAndProperty.PropertyInfo.Name : instanceAndProperty.Alias)
                    let value = instanceAndProperty.PropertyInfo.GetValue(instanceAndProperty.Instance, null)
                    select makeParameterService.MakeParameter(name, value)).ToList();
        }

        /// <summary>
        /// Flattens the object graph.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="instanceAndProperties">The instance and properties.</param>
        private static void FlattenObjectGraph(object parameters, IEnumerable<PropertyInfo> properties, ICollection<InstanceAndProperty> instanceAndProperties)
        {
            AttributeService attributeService = new AttributeService();

            foreach (PropertyInfo propertyInfo in properties)
            {
                object instance = propertyInfo.GetValue(parameters, null);

                if(propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType != typeof(string) && propertyInfo.PropertyType != typeof(DataTable))
                {
                    bool hasIgnore = attributeService.HasParameterIgnore(propertyInfo);

                    if (!hasIgnore)
                    {
                        if(instance == null)
                        {
                            throw new Exception("Property '" + propertyInfo.Name + "' is null, value is expected. To ignore property add 'IgnoreParameter' attribute.");
                        } 

                        PropertyInfo[] propertyInfos = instance.GetType().GetProperties();
                        FlattenObjectGraph(instance, propertyInfos, instanceAndProperties);
                    }
                }

                if (!propertyInfo.PropertyType.IsClass || propertyInfo.PropertyType == typeof(string) || propertyInfo.PropertyType == typeof(DataTable))
                {
                    bool hasIgnore = attributeService.HasParameterIgnore(propertyInfo);

                    if (!hasIgnore)
                    {
                        string alias = attributeService.GetAlias(propertyInfo);
                        instanceAndProperties.Add(new InstanceAndProperty
                                                        {
                                                            Instance = parameters,
                                                            Alias = alias,
                                                            PropertyInfo = propertyInfo
                                                        });
                    }
                }
            }
        }

        private class InstanceAndProperty
        {
            /// <summary>
            /// Gets or sets the instance.
            /// </summary>
            /// <value>The instance.</value>
            public object Instance { get; set; }

            /// <summary>
            /// Gets or sets the alias.
            /// </summary>
            /// <value>The alias.</value>
            public string Alias { get; set; }
            
            /// <summary>
            /// Gets or sets the property info.
            /// </summary>
            /// <value>The property info.</value>
            public PropertyInfo PropertyInfo { get; set; }
        }
    }
}
