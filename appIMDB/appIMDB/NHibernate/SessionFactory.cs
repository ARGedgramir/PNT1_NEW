using System;
using System.Configuration;
using NHibernate;
using NHibernate.Support;


namespace appIMDB.NHibernate
{
    public static class SessionFactory
    {
        private static readonly Type[] ClassMappingTypes = new[]
        {
            typeof(ActorMapping),
            typeof(MovieMapping),
            typeof(MovieRoleMapping),
        };

        private static ISessionFactory BuildSessionFactory ()
        {
            var configuration = Configurer.Configure("AppIMDB", ConfigurationManager.ConnectionStrings["AppIMDB"], ClassMappingTypes);
            return configuration.BuildSessionFactory();
        }

        public static ISessionFactory Instance { get; } = BuildSessionFactory();
    }
}