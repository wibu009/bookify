namespace Bookify.Shared.Authorization;

public static class Permissions
{
    public static readonly string[] All =
    [
        #region Users
        Build(Resources.Users, Actions.View),
        Build(Resources.Users, Actions.Search),
        Build(Resources.Users, Actions.Create),
        Build(Resources.Users, Actions.Update),
        Build(Resources.Users, Actions.Delete),
        Build(Resources.Users, Actions.Import),
        Build(Resources.Users, Actions.Export),
        #endregion
        
        #region Roles
        Build(Resources.Roles, Actions.View),
        Build(Resources.Roles, Actions.Search),
        Build(Resources.Roles, Actions.Create),
        Build(Resources.Roles, Actions.Update),
        Build(Resources.Roles, Actions.Delete),
        Build(Resources.Roles, Actions.Import),
        Build(Resources.Roles, Actions.Export),
        #endregion
        
        #region Bookings
        Build(Resources.Bookings, Actions.View),
        Build(Resources.Bookings, Actions.Search),
        Build(Resources.Bookings, Actions.Create),
        Build(Resources.Bookings, Actions.Update),
        Build(Resources.Bookings, Actions.Delete),
        Build(Resources.Bookings, Actions.Import),
        Build(Resources.Bookings, Actions.Export),
        #endregion
        
        #region Apartments
        Build(Resources.Apartments, Actions.View),
        Build(Resources.Apartments, Actions.Search),
        Build(Resources.Apartments, Actions.Create),
        Build(Resources.Apartments, Actions.Update),
        Build(Resources.Apartments, Actions.Delete),
        Build(Resources.Apartments, Actions.Import),
        Build(Resources.Apartments, Actions.Export),
        #endregion
        
        #region Reviews
        Build(Resources.Reviews, Actions.View),
        Build(Resources.Reviews, Actions.Search),
        Build(Resources.Reviews, Actions.Create),
        Build(Resources.Reviews, Actions.Update),
        Build(Resources.Reviews, Actions.Delete),
        Build(Resources.Reviews, Actions.Import),
        Build(Resources.Reviews, Actions.Export),
        #endregion
    ];
    
    public static readonly string[] Basic = Filter(
        [Resources.Users, Resources.Bookings, Resources.Apartments, Resources.Reviews], 
        [Actions.View, Actions.Search], 
        [Build(Resources.Bookings, Actions.Create),
            Build(Resources.Bookings, Actions.Update),
            Build(Resources.Reviews, Actions.Create),
            Build(Resources.Reviews, Actions.Update)]
    );
    
    public static readonly string[] Admin = Filter(Resources.All, Actions.All);
    
    public static string Build(string resource, string action) => $"{resource}:{action}";
    
    private static string[] Filter(string[] resources, string[] actions, string[]? exceptions = null, string[]? excludes = null)
    {
        var filteredPermissions = All
            .Where(permission =>
                resources.Any(resource => permission.StartsWith(resource + ":")) &&
                actions.Any(action => permission.EndsWith(":" + action)))
            .ToList();
        
        if (exceptions != null)
        {
            filteredPermissions.AddRange(
                exceptions.Where(exception => !filteredPermissions.Contains(exception))
            );
        }
        
        if (excludes != null)
        {
            filteredPermissions = filteredPermissions
                .Where(permission => !excludes.Contains(permission))
                .ToList();
        }

        return filteredPermissions.ToArray();
    }
}