using System.Data;
using AKDbHelpers.DataBaseHelpers;
using Front.Models;

namespace Front.Repositories
{
    public class UserReaders
    {
        public UserSession ReadUserSession(IDataReader reader)
        {
            var user = new UserSession
            {
                UserName = reader.GetValue<string>("UserName"),
                SessionId = reader.GetValue<int>("SessionId"),
                UserId = reader.GetValue<int>("UserId"),
                LoginName = reader.GetValue<string>("LoginName")
            };
            return user;
        }
    }
}
