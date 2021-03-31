using TenmoClient.Models;

namespace TenmoClient.DAL
{
    public interface IAccountApiDAO
    {
        decimal  GetAccountBalance(int userId);
    }
}