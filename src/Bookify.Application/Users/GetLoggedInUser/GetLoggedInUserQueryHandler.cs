﻿using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Abstractions.Persistent;
using Bookify.Domain.Abstractions;
using Bookify.Shared.Core;
using Dapper;

namespace Bookify.Application.Users.GetLoggedInUser;

internal sealed class GetLoggedInUserQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IUserContext userContext)
    : IQueryHandler<GetLoggedInUserQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetLoggedInUserQuery request, CancellationToken cancellationToken)
    {
        using var connection = sqlConnectionFactory.CreateConnection();

        const string sql = $"""
                            SELECT
                                id AS Id,
                                first_name AS FirstName,
                                last_name AS LastName,
                                email AS Email
                            FROM users
                            WHERE identity_id = @IdentityId
                            """;
        return await connection.QueryFirstOrDefaultAsync<UserResponse>(sql, new { userContext.IdentityId });
    }
}