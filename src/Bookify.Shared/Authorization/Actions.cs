namespace Bookify.Shared.Authorization;

public static class Actions
{
    public const string View = "view";
    public const string Search = "search";
    public const string Create = "create";
    public const string Update = "update";
    public const string Delete = "delete";
    public const string Import = "import";
    public const string Export = "export";
    
    public static string[] All => [View, Search, Create, Update, Delete, Import, Export];
}