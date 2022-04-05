using System;
using System.Collections.Immutable;
using System.Data.SqlClient;
using System.Reflection;
using System.Linq;

namespace Common {
    /// <summary>
    /// Get the parameters for the different Models
    /// </summary>
    public class SqlParameters {

        /// <summary>
        /// Creates Insert Parameters
        /// </summary>
        /// <typeparam name="T">can be any given Model</typeparam>
        /// <param name="model">Model with any given Type</param>
        /// <returns>Parameters from the given Type</returns>
        public static ImmutableList<SqlParameter> GetImmutableInsertParameters<T>(T model) {
            if (model is null) { throw new ArgumentNullException($"{nameof(model)}"); }
            Type t = model.GetType();
            PropertyInfo[] props = t.GetProperties();
            int i = 1;
            ImmutableList<SqlParameter>.Builder builder = ImmutableList.CreateBuilder<SqlParameter>();
            foreach (PropertyInfo prop in props.Skip(count: 1)) {
                builder.Add(new SqlParameter($"p{i++}", prop.GetValue(model)));
            }
            return builder.ToImmutable();
        }

        /// <summary>
        /// Creates Update Parameters
        /// </summary>
        /// <typeparam name="T">can be any given Model</typeparam>
        /// <param name="oldModel">old Model with any given Type</param>
        /// <param name="newModel">new Model with any given Type</param>
        /// <returns>Parameters from the given Type</returns>
        public static ImmutableList<SqlParameter> GetImmutableUpdateParameters<T>(T oldModel, T newModel)
        {
            if (oldModel is null) { throw new ArgumentNullException($"{nameof(oldModel)}"); }
            if (newModel is null) { throw new ArgumentNullException($"{nameof(newModel)}"); }
            PropertyInfo[] propsOld = oldModel.GetType().GetProperties();
            PropertyInfo[] propsNew = newModel.GetType().GetProperties();
            int i = 1;
            ImmutableList<SqlParameter>.Builder builder = ImmutableList.CreateBuilder<SqlParameter>();
            foreach (PropertyInfo propNew in propsNew.Skip(count: 1))
            {
                //builder.Add(new SqlParameter($"p{i++}", propNew.GetValue(newModel)));
                builder.Add(new SqlParameter($"@" + propNew.Name, propNew.GetValue(newModel)));
            }
            foreach (PropertyInfo propOld in propsOld)
            {
                //builder.Add(new SqlParameter($"p{i++}", propOld.GetValue(oldModel)));
                builder.Add(new SqlParameter($"@Original_" + propOld.Name, propOld.GetValue(oldModel)));
                if (i != 1)
                {
                    if (propOld.GetValue(oldModel) == null)
                    {
                        builder.Add(new SqlParameter(parameterName: $"@IsNull_" + propOld.Name, true));
                    }
                    else
                    {
                        builder.Add(new SqlParameter($"@IsNull_" + propOld.Name, false));
                    }
                }
                i++;
            }
            return builder.ToImmutable();
        }
    }
}