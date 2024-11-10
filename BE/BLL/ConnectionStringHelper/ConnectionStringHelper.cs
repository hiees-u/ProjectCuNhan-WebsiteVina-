﻿using DTO.User;

namespace BLL.LoginBLL
{
    public static class ConnectionStringHelper
    {
        private static string defaultConnectionString = "Data Source=DESKTOP-L6DVGTI;Initial Catalog=CAFFEE_VINA_DBv1;User ID=sa;Password=12345;Encrypt=False";

        private static string connectionString = defaultConnectionString;
    
        public static string Get()
        {
            return connectionString;
        }

        public static string Set(LoginRequestModule module) {
            connectionString = $"Data Source=DESKTOP-L6DVGTI;Initial Catalog=CAFFEE_VINA_DBv1;User ID={module.AccountName};Password={module.Password};Encrypt=False;";
            return connectionString;
        }

        public static void Reset()
        {
            connectionString = defaultConnectionString;
        }
    }
}
