namespace ООО__Товары_для_животных_.Database;

internal static class DB
{
    private static user11Context _instance;

    internal static user11Context Instance
    {
        get
        {
            _instance ??= new();

            return _instance;
        }
    }
}