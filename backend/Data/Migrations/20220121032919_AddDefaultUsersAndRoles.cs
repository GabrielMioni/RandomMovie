using Microsoft.EntityFrameworkCore.Migrations;

namespace backend.Data.Migrations
{
    public partial class AddDefaultUsersAndRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'1', N'Admin', N'Admin', NULL)
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'2', N'User', N'User', NULL)

                INSERT INTO [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'33ab3dda-8724-422a-a8c4-3289eef13736', N'Admin', N'ADMIN', N'Admin@notarealadmin.org', N'ADMIN@NOTAREALEMAIL.ORG', 0, N'AQAAAAEAACcQAAAAEKcPFxsHVSOTQXcVfjy52vx/6OEXKWTk4YuDGz8Dz/AMsYauMQ/JooJMOGlgbtZLhg==', N'JMCLYC4KN2CQ3TTHENODMG42Z6NF3LAV', N'b1c605a3-2b01-4e30-971c-8e5826d4eb75', NULL, 0, 0, NULL, 1, 0)                    
                INSERT INTO [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'cc71b458-9619-4b98-93bf-181fc6bf7e23', N'User', N'USER', N'User@notarealemail.org', N'USER@NOTAREALEMAIL.ORG', 0, N'AQAAAAEAACcQAAAAEDumEqdQ+Pg5yuVHJDoCsqQ6BS0xAUafMfw9RxtvMv9gkMlUYFvjgWyoIuEd0MPmxA==', N'AIZRAB3F2Q4AE42HZHMOBXOUEHY7O3PP', N'9e5d8c12-3fd0-4e9b-ada0-8cad1229b10c', NULL, 0, 0, NULL, 1, 0)

                INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'33ab3dda-8724-422a-a8c4-3289eef13736', N'1')
                INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'cc71b458-9619-4b98-93bf-181fc6bf7e23', N'2')

            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
