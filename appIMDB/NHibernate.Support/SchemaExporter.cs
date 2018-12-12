using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using global::NHibernate.Cfg;
using global::NHibernate.Tool.hbm2ddl;

using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace NHibernate.Support
{
	public static class SchemaExporter
	{
		public static bool ExportSchema(Configuration configuration, FileInfo outputFile)
		{
			try
			{
				if (outputFile == null)
				{
					throw new ArgumentNullException("outputFile");
				}

				var schemaExport = new SchemaExport(configuration);

				if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SchemaExportDelimiter"]))
				{
					schemaExport.SetDelimiter(ConfigurationManager.AppSettings["SchemaExportDelimiter"]);
				}

				var schemaBuilder = new StringBuilder();
				using (var textWriter = new StringWriter(schemaBuilder, CultureInfo.InvariantCulture))
				{
					schemaExport.Execute(false, false, false, null, textWriter);
				}

				var schema = schemaBuilder.ToString();

				// remove drop tables/constraints statements
				schema = schema.Substring(schema.IndexOf("create table", StringComparison.OrdinalIgnoreCase));

				// cleanup generated script
				schema = schema.Replace("\r\n", "\n").Replace("\r", "\n");
				schema = Regex.Replace(schema, "^ +(create table|\\)|alter table|add constraint|foreign key|references|on delete|on update|on insert)(.*)$", "$1$2", RegexOptions.Multiline);
				schema = Regex.Replace(schema, "^ +", "\t", RegexOptions.Multiline);

				schema = schema.Replace("\n", System.Environment.NewLine);

				File.WriteAllText(outputFile.FullName, schema, Encoding.UTF8);

				return true;
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine("ERROR GENERATING DATABASE SCHEMA SCRIPT");
				Console.Error.WriteLine("=======================================");

				for (Exception ex1 = ex; ex1 != null; ex1 = ex1.InnerException)
				{
					Console.Error.WriteLine(ex.Message ?? "no message");
				}

				return false;
			}
		}
	}
}
