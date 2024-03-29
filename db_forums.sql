USE [DB_Forums]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 14/01/2024 23:10:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppRoles]    Script Date: 14/01/2024 23:10:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppRoles](
	[AppRoleId] [int] IDENTITY(1,1) NOT NULL,
	[SimpleRole] [nvarchar](255) NOT NULL,
	[ManagerRole] [nvarchar](255) NULL,
 CONSTRAINT [PK__AppRoles__E66DD698AA10D3CE] PRIMARY KEY CLUSTERED 
(
	[AppRoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[categories]    Script Date: 14/01/2024 23:10:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[categories](
	[categorie_id] [int] IDENTITY(1,1) NOT NULL,
	[titre_categorie] [nvarchar](255) NOT NULL,
	[description_categorie] [nvarchar](max) NOT NULL,
 CONSTRAINT [categorie_pk] PRIMARY KEY CLUSTERED 
(
	[categorie_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[discussions]    Script Date: 14/01/2024 23:10:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[discussions](
	[discussion_id] [int] IDENTITY(1,1) NOT NULL,
	[titre_discussion] [nvarchar](255) NOT NULL,
	[dateCreation_discussion] [date] NOT NULL,
	[themeid] [int] NULL,
	[utilisateurid] [int] NULL,
 CONSTRAINT [discussion_pk] PRIMARY KEY CLUSTERED 
(
	[discussion_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[forums]    Script Date: 14/01/2024 23:10:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[forums](
	[forum_id] [int] IDENTITY(1,1) NOT NULL,
	[titre_forum] [nvarchar](255) NOT NULL,
	[dateCreation_forum] [date] NOT NULL,
	[discription_forum] [nvarchar](max) NOT NULL,
	[categorieid] [int] NOT NULL,
 CONSTRAINT [forum_pk] PRIMARY KEY CLUSTERED 
(
	[forum_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[messages]    Script Date: 14/01/2024 23:10:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[messages](
	[messages_id] [int] IDENTITY(1,1) NOT NULL,
	[contenu_message] [nvarchar](max) NOT NULL,
	[datecréation_message] [date] NOT NULL,
	[lu] [bit] NOT NULL,
	[archive] [bit] NOT NULL,
	[auteur_id] [int] NOT NULL,
	[discussionid] [int] NOT NULL,
 CONSTRAINT [message_pk] PRIMARY KEY CLUSTERED 
(
	[messages_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[themes]    Script Date: 14/01/2024 23:10:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[themes](
	[theme_id] [int] IDENTITY(1,1) NOT NULL,
	[titre_theme] [nvarchar](255) NOT NULL,
	[dateCreation_theme] [date] NOT NULL,
	[forumid] [int] NOT NULL,
 CONSTRAINT [theme_pk] PRIMARY KEY CLUSTERED 
(
	[theme_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UtilisateurRoles]    Script Date: 14/01/2024 23:10:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UtilisateurRoles](
	[UtilisateurId] [int] NOT NULL,
	[AppRoleId] [int] NOT NULL,
 CONSTRAINT [PK_UtilisateurRoles] PRIMARY KEY CLUSTERED 
(
	[UtilisateurId] ASC,
	[AppRoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[utilisateurs]    Script Date: 14/01/2024 23:10:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[utilisateurs](
	[utilisateur_id] [int] IDENTITY(1,1) NOT NULL,
	[pseudonyme] [nvarchar](255) NOT NULL,
	[password] [nvarchar](255) NOT NULL,
	[email] [nvarchar](255) NOT NULL,
	[inscrit] [bit] NOT NULL,
	[valid] [bit] NOT NULL,
	[cheminavatar] [nvarchar](255) NULL,
	[signature] [nvarchar](255) NULL,
	[actif] [bit] NULL,
 CONSTRAINT [user_pk] PRIMARY KEY CLUSTERED 
(
	[utilisateur_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231201102452_NouvelleMigration', N'7.0.12')
GO
SET IDENTITY_INSERT [dbo].[AppRoles] ON 

INSERT [dbo].[AppRoles] ([AppRoleId], [SimpleRole], [ManagerRole]) VALUES (13, N'SimpleUser', NULL)
INSERT [dbo].[AppRoles] ([AppRoleId], [SimpleRole], [ManagerRole]) VALUES (14, N'SimpleUser', NULL)
INSERT [dbo].[AppRoles] ([AppRoleId], [SimpleRole], [ManagerRole]) VALUES (21, N'Administrateur', N'Manager')
SET IDENTITY_INSERT [dbo].[AppRoles] OFF
GO
SET IDENTITY_INSERT [dbo].[categories] ON 

INSERT [dbo].[categories] ([categorie_id], [titre_categorie], [description_categorie]) VALUES (1, N'Agriculture traditionnelle', N'C''est le type d''agriculture le plus répandu en Tunisie. Elle est basée sur des méthodes et des techniques traditionnelles, 
		   telles que l''utilisation d''engrais naturels et de pesticides. L''agriculture traditionnelle est principalement pratiquée dans les zones rurales du pays.')
INSERT [dbo].[categories] ([categorie_id], [titre_categorie], [description_categorie]) VALUES (2, N'Agriculture maraîchère', N'C''est un type d''agriculture qui se concentre sur la production de légumes.
		   L''agriculture maraîchère est pratiquée dans les zones irriguées du pays, notamment dans le nord et le centre.')
INSERT [dbo].[categories] ([categorie_id], [titre_categorie], [description_categorie]) VALUES (3, N'Agriculture Floricole', N'La culture de fleurs et de plantes ornementales est une activité agricole spécialisée qui contribue également à l''économie agricole du pays')
INSERT [dbo].[categories] ([categorie_id], [titre_categorie], [description_categorie]) VALUES (4, N'Agriculture Biologique ', N' Certains agriculteurs se tournent vers des méthodes agricoles biologiques, évitant l''utilisation de pesticides et d''engrais chimiques.')
INSERT [dbo].[categories] ([categorie_id], [titre_categorie], [description_categorie]) VALUES (5, N'Agriculture de Conservation', N'Des pratiques agricoles durables, telles que la conservation des sols et la rotation des cultures, sont de plus en plus adoptées pour préserver les ressources naturelles.')
INSERT [dbo].[categories] ([categorie_id], [titre_categorie], [description_categorie]) VALUES (6, N'Agriculture Irriguée ', N'En raison du climat méditerranéen sec, l''irrigation est souvent utilisée pour soutenir la croissance des cultures, en particulier dans les régions où les précipitations sont limitées.')
INSERT [dbo].[categories] ([categorie_id], [titre_categorie], [description_categorie]) VALUES (7, N'Agriculture Législative et Réglementaire ', N'Cette catégorie engloberait les lois, règlements et politiques gouvernementales qui encadrent les activités agricoles en Tunisie. Elle pourrait inclure des aspects tels que la gestion des terres, la sécurité alimentaire, les normes de qualité, les subventions agricoles, les droits fonciers, et d''autres dispositions juridiques liées au secteur agricole.')
SET IDENTITY_INSERT [dbo].[categories] OFF
GO
SET IDENTITY_INSERT [dbo].[discussions] ON 

INSERT [dbo].[discussions] ([discussion_id], [titre_discussion], [dateCreation_discussion], [themeid], [utilisateurid]) VALUES (2, N'Conversations sur la préservation du patrimoine génétique des variétés de céréales traditionnelles.', CAST(N'2023-11-23' AS Date), 1, 20)
INSERT [dbo].[discussions] ([discussion_id], [titre_discussion], [dateCreation_discussion], [themeid], [utilisateurid]) VALUES (3, N' Les valeurs nutritionnelles spécifiques de chaque variété de céréale.', CAST(N'2023-11-23' AS Date), 1, 20)
INSERT [dbo].[discussions] ([discussion_id], [titre_discussion], [dateCreation_discussion], [themeid], [utilisateurid]) VALUES (4, N' Les initiatives de conservation et de banques de semences.', CAST(N'2023-11-23' AS Date), 1, 20)
INSERT [dbo].[discussions] ([discussion_id], [titre_discussion], [dateCreation_discussion], [themeid], [utilisateurid]) VALUES (5, N' Les initiatives modernes d''amélioration génétique tout en préservant les caractéristiques traditionnelles.', CAST(N'2023-11-23' AS Date), 1, 21)
INSERT [dbo].[discussions] ([discussion_id], [titre_discussion], [dateCreation_discussion], [themeid], [utilisateurid]) VALUES (6, N'Discussions sur les défis de la conservation des variétés de céréales locales', CAST(N'2023-11-23' AS Date), 1, 21)
INSERT [dbo].[discussions] ([discussion_id], [titre_discussion], [dateCreation_discussion], [themeid], [utilisateurid]) VALUES (7, N'L''importation.', CAST(N'2023-11-23' AS Date), 1, 21)
INSERT [dbo].[discussions] ([discussion_id], [titre_discussion], [dateCreation_discussion], [themeid], [utilisateurid]) VALUES (8, N' Échanges sur les projets de sensibilisation visant à promouvoir l''utilisation et la conservation des variétés de céréales locales.', CAST(N'2023-11-23' AS Date), 1, 21)
INSERT [dbo].[discussions] ([discussion_id], [titre_discussion], [dateCreation_discussion], [themeid], [utilisateurid]) VALUES (9, N'test', CAST(N'2023-12-16' AS Date), 1, 23)
INSERT [dbo].[discussions] ([discussion_id], [titre_discussion], [dateCreation_discussion], [themeid], [utilisateurid]) VALUES (10, N'test1', CAST(N'2023-12-16' AS Date), 1, 23)
INSERT [dbo].[discussions] ([discussion_id], [titre_discussion], [dateCreation_discussion], [themeid], [utilisateurid]) VALUES (11, N'test1', CAST(N'2023-12-16' AS Date), 1, 23)
INSERT [dbo].[discussions] ([discussion_id], [titre_discussion], [dateCreation_discussion], [themeid], [utilisateurid]) VALUES (12, N'test2', CAST(N'2023-12-16' AS Date), 1, 23)
INSERT [dbo].[discussions] ([discussion_id], [titre_discussion], [dateCreation_discussion], [themeid], [utilisateurid]) VALUES (13, N'test2', CAST(N'2023-12-17' AS Date), 1, 23)
INSERT [dbo].[discussions] ([discussion_id], [titre_discussion], [dateCreation_discussion], [themeid], [utilisateurid]) VALUES (14, N'test3', CAST(N'2023-12-17' AS Date), 1, 23)
INSERT [dbo].[discussions] ([discussion_id], [titre_discussion], [dateCreation_discussion], [themeid], [utilisateurid]) VALUES (15, N'test', CAST(N'2023-12-23' AS Date), 2, 23)
INSERT [dbo].[discussions] ([discussion_id], [titre_discussion], [dateCreation_discussion], [themeid], [utilisateurid]) VALUES (16, N'test', CAST(N'2023-12-23' AS Date), 3, 23)
INSERT [dbo].[discussions] ([discussion_id], [titre_discussion], [dateCreation_discussion], [themeid], [utilisateurid]) VALUES (17, N'bonjour', CAST(N'2023-12-26' AS Date), 1, 23)
SET IDENTITY_INSERT [dbo].[discussions] OFF
GO
SET IDENTITY_INSERT [dbo].[forums] ON 

INSERT [dbo].[forums] ([forum_id], [titre_forum], [dateCreation_forum], [discription_forum], [categorieid]) VALUES (1, N'Agriculture Céréalière', CAST(N'2023-11-15' AS Date), N'https://img.freepik.com/photos-gratuite/coup-mise-au-point-selective-ble-vert-sous-vent_181624-11363.jpg?w=360&t=st=1703973384~exp=1703973984~hmac=9edc8531db690f1a1e07b53104ae2b7b5e5c28c1d7b427b4ca75f6845c241571', 1)
INSERT [dbo].[forums] ([forum_id], [titre_forum], [dateCreation_forum], [discription_forum], [categorieid]) VALUES (2, N'Agriculture Arboricole', CAST(N'2023-11-15' AS Date), N'https://www.agritunisie.com/wp-content/uploads/agrumes-660x330.jpg', 1)
INSERT [dbo].[forums] ([forum_id], [titre_forum], [dateCreation_forum], [discription_forum], [categorieid]) VALUES (3, N'Agriculture Olière', CAST(N'2023-11-15' AS Date), N'https://cdnfr.africanmanager.com/wp-content/uploads/2020/11/onegh.jpg?_gl=1*qngej6*_ga*OTU2MDkwODAwLjE3MDM5NzU3NDM.*_ga_YJF8Y51B7Q*MTcwMzk3NTc0Mi4xLjAuMTcwMzk3NTc0Mi42MC4wLjA.&_ga=2.141395954.198249812.1703975743-956090800.1703975743', 1)
INSERT [dbo].[forums] ([forum_id], [titre_forum], [dateCreation_forum], [discription_forum], [categorieid]) VALUES (4, N'Serres Maraîchères', CAST(N'2023-11-15' AS Date), N'https://www.serres-jrc.com/media/serres2_bandeau_page_accueil.jpg', 2)
INSERT [dbo].[forums] ([forum_id], [titre_forum], [dateCreation_forum], [discription_forum], [categorieid]) VALUES (5, N'Cultures de Plantes Aromatiques et Médicinales', CAST(N'2023-11-15' AS Date), N'https://fnh.ma/uploads/actualites/5b531108e6340.png', 2)
INSERT [dbo].[forums] ([forum_id], [titre_forum], [dateCreation_forum], [discription_forum], [categorieid]) VALUES (6, N'Cultures de Légumes Feuilles', CAST(N'2023-11-15' AS Date), N'https://www.rustica.fr/images/legumes-feuilles-diverses-varietes-salades-et-laitues-bios-2143982-l1200-h0.jpg.webp', 2)
INSERT [dbo].[forums] ([forum_id], [titre_forum], [dateCreation_forum], [discription_forum], [categorieid]) VALUES (11, N'FloriTunis', CAST(N'2023-01-12' AS Date), N' https://www.middleeasteye.net/sites/default/files/styles/article_page/public/images-story/Orange_Blossom-alexander-hardin-wikimedia_0.jpg.webp?itok=DwmItURp', 3)
INSERT [dbo].[forums] ([forum_id], [titre_forum], [dateCreation_forum], [discription_forum], [categorieid]) VALUES (12, N'Commercialisation des fleurs ', CAST(N'2023-01-12' AS Date), N' https://static8.depositphotos.com/1474433/980/i/450/depositphotos_9801641-stock-photo-florist-%C2%B4-shop.jpg', 3)
INSERT [dbo].[forums] ([forum_id], [titre_forum], [dateCreation_forum], [discription_forum], [categorieid]) VALUES (13, N'Défis environnementaux ', CAST(N'2023-01-12' AS Date), N' https://www.airzen.fr/wp-content/uploads/2022/03/AdobeStock_438975514-scaled.jpeg.webp', 3)
INSERT [dbo].[forums] ([forum_id], [titre_forum], [dateCreation_forum], [discription_forum], [categorieid]) VALUES (14, N'FloralConnections ', CAST(N'2023-01-12' AS Date), N' https://ac-franchise.com/wp-content/uploads/2021/05/Effet-Covid-19-Vente-Fleurs-900x450-1.jpg', 3)
INSERT [dbo].[forums] ([forum_id], [titre_forum], [dateCreation_forum], [discription_forum], [categorieid]) VALUES (15, N'Commercialisation', CAST(N'2023-01-12' AS Date), N' https://local-fr-public.s3.eu-west-3.amazonaws.com/prod/webtool/userfiles/28091/Primeur-%C3%A0-Tarbes1.jpg', 2)
INSERT [dbo].[forums] ([forum_id], [titre_forum], [dateCreation_forum], [discription_forum], [categorieid]) VALUES (16, N'Agriculture Fourragère', CAST(N'2023-11-12' AS Date), N' https://www.agriculture-durable.ch/wp-content/uploads/2020/04/agriculture-durable-fourrage-aliments-animaux-foin-balle-ronde-scaled.jpg', 1)
SET IDENTITY_INSERT [dbo].[forums] OFF
GO
SET IDENTITY_INSERT [dbo].[messages] ON 

INSERT [dbo].[messages] ([messages_id], [contenu_message], [datecréation_message], [lu], [archive], [auteur_id], [discussionid]) VALUES (1, N'Bonjour à tous ! Je travaille dans le domaine de la recherche agricole, 
		   et je suis ravi de voir une discussion sur ce sujet crucial. 
		   Actuellement, il existe des projets visant à cataloguer et à conserver les semences de ces variétés. 
		   Je peux partager des informations sur ces projets et discuter de la manière dont chacun peut s''impliquer.', CAST(N'2023-12-01' AS Date), 1, 1, 24, 2)
INSERT [dbo].[messages] ([messages_id], [contenu_message], [datecréation_message], [lu], [archive], [auteur_id], [discussionid]) VALUES (2, N'En tant qu''agriculteur traditionnel,
		   je constate les défis auxquels nous sommes confrontés pour maintenir ces variétés. 
		   Certains facteurs tels que le changement climatique et l''évolution des pratiques agricoles menacent la diversité génétique. 
		   Comment pouvons-nous sensibiliser davantage et encourager l''adoption de ces variétés par les agriculteurs?', CAST(N'2023-12-01' AS Date), 1, 1, 23, 2)
INSERT [dbo].[messages] ([messages_id], [contenu_message], [datecréation_message], [lu], [archive], [auteur_id], [discussionid]) VALUES (3, N'Bonjour à tous,

Je voulais partager quelques réflexions sur l''importance de préserver le patrimoine génétique des variétés de céréales traditionnelles.
Ces variétés ont joué un rôle essentiel dans notre histoire agricole,
et leur diversité génétique est précieuse pour assurer la résilience de nos cultures face aux changements environnementaux.', CAST(N'2023-12-17' AS Date), 1, 1, 23, 2)
SET IDENTITY_INSERT [dbo].[messages] OFF
GO
SET IDENTITY_INSERT [dbo].[themes] ON 

INSERT [dbo].[themes] ([theme_id], [titre_theme], [dateCreation_theme], [forumid]) VALUES (1, N'Variétés de Céréales Locales', CAST(N'2023-12-20' AS Date), 1)
INSERT [dbo].[themes] ([theme_id], [titre_theme], [dateCreation_theme], [forumid]) VALUES (2, N'Techniques de Plantation et de Récolte', CAST(N'2023-12-20' AS Date), 1)
INSERT [dbo].[themes] ([theme_id], [titre_theme], [dateCreation_theme], [forumid]) VALUES (3, N'Gestion des Sols et Fertilisation', CAST(N'2023-12-20' AS Date), 1)
INSERT [dbo].[themes] ([theme_id], [titre_theme], [dateCreation_theme], [forumid]) VALUES (4, N'Défis Climatiques et Solutions', CAST(N'2023-12-20' AS Date), 1)
INSERT [dbo].[themes] ([theme_id], [titre_theme], [dateCreation_theme], [forumid]) VALUES (5, N'Innovations Technologiques', CAST(N'2023-12-20' AS Date), 1)
INSERT [dbo].[themes] ([theme_id], [titre_theme], [dateCreation_theme], [forumid]) VALUES (6, N'Questions Réglementaires et Législatives', CAST(N'2023-12-20' AS Date), 1)
INSERT [dbo].[themes] ([theme_id], [titre_theme], [dateCreation_theme], [forumid]) VALUES (7, N'Éducation Agricole et Formation', CAST(N'2023-12-20' AS Date), 1)
INSERT [dbo].[themes] ([theme_id], [titre_theme], [dateCreation_theme], [forumid]) VALUES (8, N'Arbres Fruitiers Locaux ', CAST(N'2023-12-20' AS Date), 2)
INSERT [dbo].[themes] ([theme_id], [titre_theme], [dateCreation_theme], [forumid]) VALUES (9, N'Ravageurs et Maladies', CAST(N'2023-12-20' AS Date), 2)
INSERT [dbo].[themes] ([theme_id], [titre_theme], [dateCreation_theme], [forumid]) VALUES (10, N'Culture de Fruits Secs', CAST(N'2023-12-20' AS Date), 2)
INSERT [dbo].[themes] ([theme_id], [titre_theme], [dateCreation_theme], [forumid]) VALUES (11, N'Festivals de Récolte', CAST(N'2023-12-20' AS Date), 2)
INSERT [dbo].[themes] ([theme_id], [titre_theme], [dateCreation_theme], [forumid]) VALUES (12, N'Irrigation et Gestion de l''eau', CAST(N'2023-12-20' AS Date), 2)
INSERT [dbo].[themes] ([theme_id], [titre_theme], [dateCreation_theme], [forumid]) VALUES (13, N'Économie et Commercialisation', CAST(N'2023-12-20' AS Date), 2)
INSERT [dbo].[themes] ([theme_id], [titre_theme], [dateCreation_theme], [forumid]) VALUES (14, N'Questions Réglementaires et Législatives', CAST(N'2023-12-20' AS Date), 2)
SET IDENTITY_INSERT [dbo].[themes] OFF
GO
INSERT [dbo].[UtilisateurRoles] ([UtilisateurId], [AppRoleId]) VALUES (23, 13)
INSERT [dbo].[UtilisateurRoles] ([UtilisateurId], [AppRoleId]) VALUES (24, 14)
INSERT [dbo].[UtilisateurRoles] ([UtilisateurId], [AppRoleId]) VALUES (31, 21)
GO
SET IDENTITY_INSERT [dbo].[utilisateurs] ON 

INSERT [dbo].[utilisateurs] ([utilisateur_id], [pseudonyme], [password], [email], [inscrit], [valid], [cheminavatar], [signature], [actif]) VALUES (20, N'Monde_Vert', N'sYZB2h+9v1oj5SugBOrl97Tgm2nvUPNHUtDANfFpyQ0=', N'Monde_vert@gmail.com', 0, 0, N'https://img.freepik.com/photos-premium/logo-journee-enseignant_644450-2194.jpg?w=740', N'la_Terre', 0)
INSERT [dbo].[utilisateurs] ([utilisateur_id], [pseudonyme], [password], [email], [inscrit], [valid], [cheminavatar], [signature], [actif]) VALUES (21, N'usertest', N'$2a$11$0J4qF8f/0zSiVvG2TG70YejBJGDPNCZJ6cgquQsiPMzKJmQoBD5Wy', N'usertest@gmail.com', 0, 0, N'https://img.freepik.com/photos-gratuite/dessin-anime-femme-couronne-fleurs-tete_1340-32035.jpg?w=360&t=st=1701097126~exp=1701097726~hmac=5c9eee7023a8ef700958329523e63ffe22f5eca1e236680cf45090a5b6f445e9', N'user', 0)
INSERT [dbo].[utilisateurs] ([utilisateur_id], [pseudonyme], [password], [email], [inscrit], [valid], [cheminavatar], [signature], [actif]) VALUES (23, N'olfa_forum', N'$2a$11$Y79t6B.BJeT/b3GKazyrn..kfQJT6sfLTdxGV0BZFb1E94qUPcDfG', N'olfa@gmail.com', 0, 0, N'https://img.freepik.com/photos-premium/logo-journee-enseignant_644450-2194.jpg?w=740', N'Forum', 0)
INSERT [dbo].[utilisateurs] ([utilisateur_id], [pseudonyme], [password], [email], [inscrit], [valid], [cheminavatar], [signature], [actif]) VALUES (24, N'user_for', N'$2a$11$vp.dystQv8lkCz2Hhtmq/udwnE..7UaFdpjS2fRsW.NelKdd3yIHu', N'user_for@gmail.com', 0, 0, N'https://img.freepik.com/photos-gratuite/dessin-anime-femme-couronne-fleurs-tete_1340-32035.jpg?w=360&t=st=1701097126~exp=1701097726~hmac=5c9eee7023a8ef700958329523e63ffe22f5eca1e236680cf45090a5b6f445e9', N'user', 0)
INSERT [dbo].[utilisateurs] ([utilisateur_id], [pseudonyme], [password], [email], [inscrit], [valid], [cheminavatar], [signature], [actif]) VALUES (31, N'Terre_Fertile', N'$2a$11$OwqjGn6cH.q.w0iLiaXGpeAwS4reuR03.TjHl1QUcIXMSmjDO2VQ6', N'm.nasr22342@pi.tn', 0, 0, N'https://img.freepik.com/vecteurs-libre/illustration-art-ligne-dessinee-main_23-2149298652.jpg?https://designtemlate.s3.us-west-1.wasabisys.com/Farmer-Avatar-Creative-3D-Graphic-Illustration-700.webp', N'Forum_TerreFertile', 0)
SET IDENTITY_INSERT [dbo].[utilisateurs] OFF
GO
ALTER TABLE [dbo].[utilisateurs] ADD  DEFAULT ((0)) FOR [actif]
GO
ALTER TABLE [dbo].[discussions]  WITH CHECK ADD  CONSTRAINT [FK_discussions_themes] FOREIGN KEY([themeid])
REFERENCES [dbo].[themes] ([theme_id])
GO
ALTER TABLE [dbo].[discussions] CHECK CONSTRAINT [FK_discussions_themes]
GO
ALTER TABLE [dbo].[discussions]  WITH CHECK ADD  CONSTRAINT [FK_discussions_utilisateurs] FOREIGN KEY([utilisateurid])
REFERENCES [dbo].[utilisateurs] ([utilisateur_id])
GO
ALTER TABLE [dbo].[discussions] CHECK CONSTRAINT [FK_discussions_utilisateurs]
GO
ALTER TABLE [dbo].[forums]  WITH CHECK ADD  CONSTRAINT [forum_categorie_id_fk] FOREIGN KEY([categorieid])
REFERENCES [dbo].[categories] ([categorie_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[forums] CHECK CONSTRAINT [forum_categorie_id_fk]
GO
ALTER TABLE [dbo].[messages]  WITH CHECK ADD  CONSTRAINT [message_auteur_id_fk] FOREIGN KEY([auteur_id])
REFERENCES [dbo].[utilisateurs] ([utilisateur_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[messages] CHECK CONSTRAINT [message_auteur_id_fk]
GO
ALTER TABLE [dbo].[messages]  WITH CHECK ADD  CONSTRAINT [message_discussion_id_fk] FOREIGN KEY([discussionid])
REFERENCES [dbo].[discussions] ([discussion_id])
GO
ALTER TABLE [dbo].[messages] CHECK CONSTRAINT [message_discussion_id_fk]
GO
ALTER TABLE [dbo].[themes]  WITH CHECK ADD  CONSTRAINT [thème_forum_id_fk] FOREIGN KEY([forumid])
REFERENCES [dbo].[forums] ([forum_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[themes] CHECK CONSTRAINT [thème_forum_id_fk]
GO
ALTER TABLE [dbo].[UtilisateurRoles]  WITH CHECK ADD  CONSTRAINT [FK_UtilisateurRoles_AppRoleId] FOREIGN KEY([AppRoleId])
REFERENCES [dbo].[AppRoles] ([AppRoleId])
GO
ALTER TABLE [dbo].[UtilisateurRoles] CHECK CONSTRAINT [FK_UtilisateurRoles_AppRoleId]
GO
ALTER TABLE [dbo].[UtilisateurRoles]  WITH CHECK ADD  CONSTRAINT [FK_UtilisateurRoles_UtilisateurId] FOREIGN KEY([UtilisateurId])
REFERENCES [dbo].[utilisateurs] ([utilisateur_id])
GO
ALTER TABLE [dbo].[UtilisateurRoles] CHECK CONSTRAINT [FK_UtilisateurRoles_UtilisateurId]
GO
