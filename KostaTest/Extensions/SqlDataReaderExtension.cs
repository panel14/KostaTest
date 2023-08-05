using System.Data.SqlClient;

namespace KostaTest.Extensions
{
    public static class SqlDataReaderExtension
    {
        public static string SafeGetString(this SqlDataReader reader, int columnIndex)
        {
            if (!reader.IsDBNull(columnIndex))
            {
                return reader.GetString(columnIndex);
            }
            return string.Empty;
        }

        public static Guid SafeGetGuid(this SqlDataReader reader, int columnIndex)
        {
            if (!reader.IsDBNull(columnIndex))
            {
                return reader.GetGuid(columnIndex);
            }
            return Guid.Empty;
        }
    }
}
