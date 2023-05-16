using ООО__Товары_для_животных_.Models;

namespace ООО__Товары_для_животных_.Interfaces
{
    public interface IDBAuth
    {
        User FindUserByLoginPassword(string login, string pass);
    }

}