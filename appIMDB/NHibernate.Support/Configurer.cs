using System;
using System.Collections.Generic;

using global::NHibernate.Cfg;
using global::NHibernate.Mapping.ByCode;

using ConnectionStringSettings = System.Configuration.ConnectionStringSettings;

namespace NHibernate.Support
{
	using Type = System.Type;

	public static class Configurer
	{
		public static Configuration Configure(string sessionFactoryName, ConnectionStringSettings connectionStringSettings, params Type[] classMappingTypes)
		{
			var configuration = new Configuration().Configure(ConfigurationSectionHandler.ConfigurationSection, sessionFactoryName);

			if (connectionStringSettings != null)
			{
				SetupConnectionStringSettings(configuration, connectionStringSettings);
			}

			configuration.AddClassMappingTypes(classMappingTypes);

			return configuration;
		}

		/// <summary>
		/// Setups the session factory connection string related properties, if they were not already set.
		/// </summary>
		/// <param name="configuration">The NHibernate session factory configuration.</param>
		/// <param name="connectionStringSettings">The connection string settings.</param>
		private static void SetupConnectionStringSettings(Configuration configuration, ConnectionStringSettings connectionStringSettings)
		{
			if (string.IsNullOrEmpty(configuration.GetProperty(global::NHibernate.Cfg.Environment.ConnectionString))
				&& string.IsNullOrEmpty(configuration.GetProperty(global::NHibernate.Cfg.Environment.ConnectionStringName)))
			{
				configuration.SetProperty(global::NHibernate.Cfg.Environment.ConnectionString, connectionStringSettings.ConnectionString);

				string dialect;
				switch (connectionStringSettings.ProviderName ?? string.Empty)
				{
					case "System.Data.OracleClient":
						dialect = "NHibernate.Dialect.Oracle10gDialect";
						break;

					case "Npgsql":
						dialect = "NHibernate.Dialect.PostgreSQLDialect";
						break;

					case "Firebird":
						dialect = "NHibernate.Dialect.FirebirdDialect";
						break;

					case "MySQL":
						dialect = "NHibernate.Dialect.MySQLDialect";
						break;

					case "System.Data.SQLite":
						dialect = "NHibernate.Dialect.SQLiteDialect";
						break;

					case "System.Data.SqlClient":
						dialect = "NHibernate.Dialect.MsSql2008Dialect";
						break;

					default:
						dialect = null;
						break;
				}

				if (string.IsNullOrEmpty(configuration.GetProperty(global::NHibernate.Cfg.Environment.ConnectionProvider)))
				{
					configuration.SetProperty(global::NHibernate.Cfg.Environment.ConnectionProvider, "NHibernate.Connection.DriverConnectionProvider");
				}

				if (!string.IsNullOrEmpty(dialect) && string.IsNullOrEmpty(configuration.GetProperty(global::NHibernate.Cfg.Environment.Dialect)))
				{
					configuration.SetProperty(global::NHibernate.Cfg.Environment.Dialect, dialect);
				}
			}
		}

		/*private static IEnumerable<Type> GetAncestorTypes(this Type targetType, bool includeSelfType)
		{
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}

			if (includeSelfType)
			{
				yield return targetType;
			}

			for (var baseType = targetType.BaseType; baseType != null; baseType = baseType.BaseType)
			{
				yield return baseType;
			}
		}

		private static Type GetGenericBaseOrInterfaceType(this Type targetType, Type genericTypeDefinition)
		{
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}

			if (genericTypeDefinition == null)
			{
				throw new ArgumentNullException("genericTypeDefinition");
			}

			if (!genericTypeDefinition.IsGenericTypeDefinition)
			{
				throw new ArgumentException("Argument is not a generic type definition", "genericTypeDefinition");
			}

			var baseOrInterfaceTypes = genericTypeDefinition.IsInterface ? targetType.GetInterfaces() : targetType.GetAncestorTypes(false);

			foreach (var targetTypeInterfaceType in new Type[] { targetType }.Concat(baseOrInterfaceTypes).Where(t => t.IsGenericType))
			{
				if (targetTypeInterfaceType.GetGenericTypeDefinition() == genericTypeDefinition)
				{
					return targetTypeInterfaceType;
				}
			}

			return null;
		}*/

		public static void AddClassMappingTypes(this Configuration configuration, params Type[] classMappingTypes)
		{
			/*classMappingTypes = classMappingTypes.Select(t => new { MappingType = t, EntityType = GetGenericBaseOrInterfaceType(t, typeof(PropertyContainerCustomizer<>)).GenericTypeArguments[0] })
				.OrderBy(cm => cm.EntityType, new TypeHierarchyComparer())
				.Select(cm => cm.MappingType)
				.ToArray();*/

			var modelMapper = new ModelMapper();
			modelMapper.AddMappings(classMappingTypes);

			configuration.AddMapping(modelMapper.CompileMappingForAllExplicitlyAddedEntities());
		}

		/// <summary>
		/// Defines a method that a type implements to compare two types.
		/// </summary>
		private class TypeHierarchyComparer : IComparer<Type>
		{
			/// <summary>
			/// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
			/// </summary>
			/// <param name="x">The first object to compare.</param>
			/// <param name="y">The second object to compare.</param>
			/// <returns>A signed integer that indicates the relative values of x and y, as shown in the following table.
			/// Less than zero: x is less than y.
			///	Zero: x equals y.
			///	Greater than zero: x is greater than y.</returns>
			public int Compare(Type x, Type y)
			{
				if (x == null)
				{
					throw new ArgumentNullException("x");
				}

				if (y == null)
				{
					throw new ArgumentNullException("y");
				}

				if (x == y)
				{
					return 0;
				}

				if (x.IsAssignableFrom(y))
				{
					return -1;
				}

				if (y.IsAssignableFrom(x))
				{
					return 1;
				}

				return 0;
			}
		}
	}
}
