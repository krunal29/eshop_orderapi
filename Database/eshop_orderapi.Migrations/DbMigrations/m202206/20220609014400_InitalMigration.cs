using FluentMigrator;

namespace eshop_orderapi.Migrations.DbMigrations.m202206
{
    [Migration(20220609014400)]
    public class InitalMigration : Migration
    {
        public const string initialMigration = @"
        SET ANSI_NULLS ON
        GO
        SET QUOTED_IDENTIFIER ON
        GO
        CREATE TABLE [dbo].[AspNetRoleClaims](
          [Id] [int] IDENTITY(1,1) NOT NULL,
          [RoleId] [nvarchar](450) NOT NULL,
          [ClaimType] [nvarchar](max) NULL,
          [ClaimValue] [nvarchar](max) NULL,
         CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED
        (
          [Id] ASC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
        ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
        GO

        SET ANSI_NULLS ON
        GO
        SET QUOTED_IDENTIFIER ON
        GO
        CREATE TABLE [dbo].[AspNetRoles](
          [Id] [nvarchar](450) NOT NULL,
          [Name] [nvarchar](256) NULL,
          [NormalizedName] [nvarchar](256) NULL,
          [ConcurrencyStamp] [nvarchar](max) NULL,
         CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED
        (
          [Id] ASC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
        ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
        GO

        SET ANSI_NULLS ON
        GO
        SET QUOTED_IDENTIFIER ON
        GO
        CREATE TABLE [dbo].[AspNetUserClaims](
          [Id] [int] IDENTITY(1,1) NOT NULL,
          [UserId] [nvarchar](450) NOT NULL,
          [ClaimType] [nvarchar](max) NULL,
          [ClaimValue] [nvarchar](max) NULL,
         CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED
        (
          [Id] ASC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
        ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
        GO

        SET ANSI_NULLS ON
        GO
        SET QUOTED_IDENTIFIER ON
        GO
        CREATE TABLE [dbo].[AspNetUserLogins](
          [LoginProvider] [nvarchar](450) NOT NULL,
          [ProviderKey] [nvarchar](450) NOT NULL,
          [ProviderDisplayName] [nvarchar](max) NULL,
          [UserId] [nvarchar](450) NOT NULL,
         CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED
        (
          [LoginProvider] ASC,
          [ProviderKey] ASC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
        ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
        GO

        SET ANSI_NULLS ON
        GO
        SET QUOTED_IDENTIFIER ON
        GO
        CREATE TABLE [dbo].[AspNetUserRoles](
          [UserId] [nvarchar](450) NOT NULL,
          [RoleId] [nvarchar](450) NOT NULL,
         CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED
        (
          [UserId] ASC,
          [RoleId] ASC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
        ) ON [PRIMARY]
        GO

        SET ANSI_NULLS ON
        GO
        SET QUOTED_IDENTIFIER ON
        GO
        CREATE TABLE [dbo].[AspNetUsers](
          [Id] [nvarchar](450) NOT NULL,
          [UserName] [nvarchar](256) NULL,
          [NormalizedUserName] [nvarchar](256) NULL,
          [Email] [nvarchar](256) NULL,
          [NormalizedEmail] [nvarchar](256) NULL,
          [EmailConfirmed] [bit] NOT NULL,
          [PasswordHash] [nvarchar](max) NULL,
          [SecurityStamp] [nvarchar](max) NULL,
          [ConcurrencyStamp] [nvarchar](max) NULL,
          [PhoneNumber] [nvarchar](max) NULL,
          [PhoneNumberConfirmed] [bit] NOT NULL,
          [TwoFactorEnabled] [bit] NOT NULL,
          [LockoutEnd] [datetimeoffset](7) NULL,
          [LockoutEnabled] [bit] NOT NULL,
          [AccessFailedCount] [int] NOT NULL,
          [PasswordReset] [uniqueidentifier] NULL,
          [PasswordResetDate] [datetime2](7) NULL,
         CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED
        (
          [Id] ASC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
        ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
        GO

        SET ANSI_NULLS ON
        GO
        SET QUOTED_IDENTIFIER ON
        GO
        CREATE TABLE [dbo].[AspNetUserTokens](
          [UserId] [nvarchar](450) NOT NULL,
          [LoginProvider] [nvarchar](450) NOT NULL,
          [Name] [nvarchar](450) NOT NULL,
          [Value] [nvarchar](max) NULL,
         CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED
        (
          [UserId] ASC,
          [LoginProvider] ASC,
          [Name] ASC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
        ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
        GO

        SET ANSI_NULLS ON
        GO
        SET QUOTED_IDENTIFIER ON
        GO
        CREATE TABLE [dbo].[Person](
          [Id] [int] IDENTITY(1,1) NOT NULL,
          [Name] [nvarchar](max) NULL,
         CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED
        (
          [Id] ASC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
        ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
        GO
        ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
        REFERENCES [dbo].[AspNetRoles] ([Id])
        ON DELETE CASCADE
        GO
        ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
        GO
        ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
        REFERENCES [dbo].[AspNetUsers] ([Id])
        ON DELETE CASCADE
        GO
        ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
        GO
        ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
        REFERENCES [dbo].[AspNetUsers] ([Id])
        ON DELETE CASCADE
        GO
        ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
        GO
        ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
        REFERENCES [dbo].[AspNetRoles] ([Id])
        ON DELETE CASCADE
        GO
        ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
        GO
        ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
        REFERENCES [dbo].[AspNetUsers] ([Id])
        ON DELETE CASCADE
        GO
        ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
        GO
        ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
        REFERENCES [dbo].[AspNetUsers] ([Id])
        ON DELETE CASCADE
        GO
        ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
        GO

        SET ANSI_NULLS ON
        GO
        SET QUOTED_IDENTIFIER ON
        GO
        CREATE TABLE [dbo].[AccessModule](
	        [Id] [int] IDENTITY(1,1) NOT NULL,
	        [ModuleId] [int] NOT NULL,
	        [Overview] [bit] NOT NULL,
	        [View] [bit] NOT NULL,
	        [Add] [bit] NOT NULL,
	        [Edit] [bit] NOT NULL,
	        [Delete] [bit] NOT NULL,
         CONSTRAINT [PK_AccessModule] PRIMARY KEY CLUSTERED
        (
	        [Id] ASC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
        ) ON [PRIMARY]
        GO

        SET ANSI_NULLS ON
        GO
        SET QUOTED_IDENTIFIER ON
        GO
        CREATE TABLE [dbo].[Module](
	        [Id] [int] IDENTITY(1,1) NOT NULL,
	        [Name] [nvarchar](50) NOT NULL,
	        [ParentId] [int] NULL,
	        [Type] [smallint] NOT NULL,
	        [SortOrder] [int] NULL,
         CONSTRAINT [PK_Module] PRIMARY KEY CLUSTERED
        (
	        [Id] ASC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
        ) ON [PRIMARY]
        GO

        SET ANSI_NULLS ON
        GO
        SET QUOTED_IDENTIFIER ON
        GO
        CREATE TABLE [dbo].[RoleModule](
	        [Id] [int] IDENTITY(1,1) NOT NULL,
	        [RoleId] [int] NOT NULL,
	        [ModuleId] [int] NOT NULL,
	        [Overview] [bit] NOT NULL,
	        [View] [bit] NOT NULL,
	        [Add] [bit] NOT NULL,
	        [Edit] [bit] NOT NULL,
	        [Delete] [bit] NOT NULL,
         CONSTRAINT [PK_RoleModule] PRIMARY KEY CLUSTERED
        (
	        [Id] ASC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
        ) ON [PRIMARY]
        GO

        SET ANSI_NULLS ON
        GO
        SET QUOTED_IDENTIFIER ON
        GO
        CREATE TABLE [dbo].[Roles](
	        [Id] [int] IDENTITY(1,1) NOT NULL,
	        [ParentId] [int] NULL,
	        [Name] [nvarchar](50) NOT NULL,
	        [IsActive] [bit] NOT NULL,
         CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED
        (
	        [Id] ASC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
        ) ON [PRIMARY]
        GO
        SET IDENTITY_INSERT [dbo].[AccessModule] ON
        GO
        INSERT [dbo].[AccessModule] ([Id], [ModuleId], [Overview], [View], [Add], [Edit], [Delete]) VALUES (1, 1, 1, 1, 1, 1, 1)
        GO
        SET IDENTITY_INSERT [dbo].[AccessModule] OFF
        GO
        SET IDENTITY_INSERT [dbo].[Module] ON
        GO
        INSERT [dbo].[Module] ([Id], [Name], [ParentId], [Type], [SortOrder]) VALUES (1, N'Dashboard', NULL, 1, NULL)
        GO
        SET IDENTITY_INSERT [dbo].[Module] OFF
        GO
        SET IDENTITY_INSERT [dbo].[RoleModule] ON
        GO
        INSERT [dbo].[RoleModule] ([Id], [RoleId], [ModuleId], [Overview], [View], [Add], [Edit], [Delete]) VALUES (1, 1, 1, 1, 1, 1, 1, 1)
        GO
        SET IDENTITY_INSERT [dbo].[RoleModule] OFF
        GO
        SET IDENTITY_INSERT [dbo].[Roles] ON
        GO
        INSERT [dbo].[Roles] ([Id], [ParentId], [Name], [IsActive]) VALUES (1, NULL, N'Admin', 1)
        GO
        SET IDENTITY_INSERT [dbo].[Roles] OFF
        GO
        ALTER TABLE [dbo].[AccessModule]  WITH NOCHECK ADD  CONSTRAINT [FK_AccessModule_AccessModule] FOREIGN KEY([Id])
        REFERENCES [dbo].[AccessModule] ([Id])
        GO
        ALTER TABLE [dbo].[AccessModule] CHECK CONSTRAINT [FK_AccessModule_AccessModule]
        GO
        ALTER TABLE [dbo].[AccessModule]  WITH NOCHECK ADD  CONSTRAINT [FK_AccessModule_Module] FOREIGN KEY([ModuleId])
        REFERENCES [dbo].[Module] ([Id])
        GO
        ALTER TABLE [dbo].[AccessModule] CHECK CONSTRAINT [FK_AccessModule_Module]
        GO
        ALTER TABLE [dbo].[Module]  WITH NOCHECK ADD  CONSTRAINT [FK_Module_Module] FOREIGN KEY([Id])
        REFERENCES [dbo].[Module] ([Id])
        GO
        ALTER TABLE [dbo].[Module] CHECK CONSTRAINT [FK_Module_Module]
        GO
        ALTER TABLE [dbo].[Module]  WITH NOCHECK ADD  CONSTRAINT [FK_Module_Module1] FOREIGN KEY([ParentId])
        REFERENCES [dbo].[Module] ([Id])
        GO
        ALTER TABLE [dbo].[Module] CHECK CONSTRAINT [FK_Module_Module1]
        GO
        ALTER TABLE [dbo].[RoleModule]  WITH NOCHECK ADD  CONSTRAINT [FK_RoleModule_Module] FOREIGN KEY([ModuleId])
        REFERENCES [dbo].[Module] ([Id])
        GO
        ALTER TABLE [dbo].[RoleModule] CHECK CONSTRAINT [FK_RoleModule_Module]
        GO
        ALTER TABLE [dbo].[RoleModule]  WITH NOCHECK ADD  CONSTRAINT [FK_RoleModule_Roles] FOREIGN KEY([RoleId])
        REFERENCES [dbo].[Roles] ([Id])
        GO
        ALTER TABLE [dbo].[RoleModule] CHECK CONSTRAINT [FK_RoleModule_Roles]
        GO
        ALTER TABLE [dbo].[Roles]  WITH NOCHECK ADD  CONSTRAINT [FK_Roles_Roles] FOREIGN KEY([ParentId])
        REFERENCES [dbo].[Roles] ([Id])
        GO
        ALTER TABLE [dbo].[Roles] CHECK CONSTRAINT [FK_Roles_Roles]
        GO
        INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [PasswordReset], [PasswordResetDate]) VALUES (N'97da558f-9ba9-49e0-8e02-7203d91bde5b', N'DemoUser', N'DEMOUSER', N'Test@gmail.com', N'TEST@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEI8tmop13naZ1cIfyxFgrV113wxFB3z7Bmvf4JXGlQdAY8nV8kWqf3QlSj+2lneW2g==', N'XR6MVOINWQVIWPBXAQAXIYR2QXPBXPVS', N'c5e66eaf-641a-4e74-98b4-76f44ac36185', NULL, 0, 0, NULL, 1, 0, NULL, NULL)
        GO

";

        public override void Down()
        {
        }

        public override void Up()
        {
            Execute.Sql(initialMigration);
        }
    }
}