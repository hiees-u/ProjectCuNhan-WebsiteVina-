using DTO.User;

namespace BLL.LoginBLL
{
    public static class ConnectionStringHelper
    {
        private static string defaultConnectionString = "Data Source=DESKTOP-L6DVGTI;Initial Catalog=CAFFEE_VINA_DBv1;User ID=sa;Password=123;Encrypt=False";
        //private static string defaultConnectionString = "Data Source=THEVINH;Initial Catalog=CAFFEE_VINA_DBv1;User ID=HiuWarehouseEmployee; Password=123; Encrypt=False";
        //private static string defaultConnectionString = "Data Source=THEVINH;Initial Catalog=CAFFEE_VINA_DBv1;User ID=HiuModerator; Password=123; Encrypt=False";
        //private static string defaultConnectionString = "Data Source=THEVINH;Initial Catalog=CAFFEE_VINA_DBv1;User ID=sa; Password=thevinh123; Encrypt=False";
        //Data Source=DESKTOP-L6DVGTI;Initial Catalog=CAFFEE_VINA_DBv1;User ID=sa;Encrypt=False
        //Data Source=DESKTOP-L6DVGTI;Initial Catalog=CAFFEE_VINA_DBv1;User ID=sa;Encrypt=False;Trust Server Certificate=True

        private static string connectionString = defaultConnectionString;
    
        public static string Get()
        {
            return connectionString;
        }

        public static string Set(LoginRequestModule module) {
            connectionString = $"Data Source=DESKTOP-L6DVGTI;Initial Catalog=CAFFEE_VINA_DBv1;User ID={module.AccountName};Password={module.Password};Encrypt=False;";
            //connectionString = $"Data Source=THEVINH;Initial Catalog=CAFFEE_VINA_DBv1;User ID={module.AccountName};Password={module.Password};Encrypt=False;";
            return connectionString;
        }

        public static void Reset()
        {
            connectionString = defaultConnectionString;
        }
    }
}
