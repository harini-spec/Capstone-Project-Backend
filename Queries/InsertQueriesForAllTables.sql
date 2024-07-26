use dbHealthTracker
GO

SET IDENTITY_INSERT [dbo].[Metrics] ON 
GO
INSERT [dbo].[Metrics] ([Id], [MetricType], [MetricUnit], [Created_at], [Updated_at]) VALUES (1, N'Blood_Pressure_Diastolic', N'mmHg', CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Metrics] ([Id], [MetricType], [MetricUnit], [Created_at], [Updated_at]) VALUES (3, N'Blood_Pressure_Systolic', N'mmHg', CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Metrics] ([Id], [MetricType], [MetricUnit], [Created_at], [Updated_at]) VALUES (5, N'Sugar_Level', N'mg/dL', CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Metrics] ([Id], [MetricType], [MetricUnit], [Created_at], [Updated_at]) VALUES (6, N'Water_Intake', N'L', CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Metrics] ([Id], [MetricType], [MetricUnit], [Created_at], [Updated_at]) VALUES (7, N'Steps_Count', N'Steps', CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Metrics] ([Id], [MetricType], [MetricUnit], [Created_at], [Updated_at]) VALUES (8, N'Sleep_Hours', N'Hours', CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Metrics] ([Id], [MetricType], [MetricUnit], [Created_at], [Updated_at]) VALUES (9, N'Height', N'Meters', CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Metrics] ([Id], [MetricType], [MetricUnit], [Created_at], [Updated_at]) VALUES (10, N'Weight', N'Kg', CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Metrics] ([Id], [MetricType], [MetricUnit], [Created_at], [Updated_at]) VALUES (11, N'Calories_Intake', N'kcal', CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Metrics] ([Id], [MetricType], [MetricUnit], [Created_at], [Updated_at]) VALUES (12, N'Calories_Burned', N'kcal', CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Metrics] ([Id], [MetricType], [MetricUnit], [Created_at], [Updated_at]) VALUES (13, N'BMI', N'kg/m²', CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Metrics] OFF
GO

SET IDENTITY_INSERT [dbo].[IdealDatas] ON 
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (1, 1, 0, 60, 80, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (2, 1, 1, 80, 90, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (3, 1, 2, 0, 60, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (4, 1, 2, 90, 500, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (7, 3, 0, 90, 120, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (8, 3, 1, 120, 140, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (9, 3, 2, 0, 89, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (10, 3, 2, 140, 500, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (11, 5, 0, 70, 100, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (12, 5, 1, 100, 140, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (13, 5, 2, 0, 70, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (14, 5, 2, 140, 500, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (15, 6, 0, 2, 3, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (16, 6, 1, 1.5, 4, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (17, 6, 2, 0, 1.5, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (18, 6, 2, 4, 10, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (19, 7, 0, 7000, 10000, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (21, 7, 1, 5000, 7000, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (22, 7, 2, 0, 5000, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (23, 8, 0, 7, 9, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (24, 8, 1, 6, 7, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (25, 8, 2, 0, 6, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (26, 8, 2, 9, 24, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (27, 11, 0, 1800, 2400, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (28, 11, 1, 1500, 2500, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (29, 11, 2, 0, 1500, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (30, 11, 2, 2500, 10000, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (31, 13, 0, 18.5, 24.9, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (32, 13, 1, 25, 29.9, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (33, 13, 2, 0, 18.5, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[IdealDatas] ([ID], [MetricId], [HealthStatus], [MinVal], [MaxVal], [Created_at], [Updated_at]) VALUES (34, 13, 2, 30, 100, CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[IdealDatas] OFF
GO

SET IDENTITY_INSERT [dbo].[UsersDetails] ON 
GO
INSERT [dbo].[UsersDetails] ([Id], [PasswordEncrypted], [PasswordHashKey], [Status], [Created_at], [Updated_at]) VALUES (1, 0x60AD15C4E5C9FEF9684FCA66446A50DAB6EB652E136E7DBD7E65B1F0296CC8B1D75D19C5BF04DEC89CA3096E6EB0DA080022E8F9CEC5B39E96CBA5FEB88277DD, 0x9085A3E118EA25DCBD32E9EF164DB669A9F19A6542FB0DC49AE96B871779F683D5EC668C11BE53021F5D3C029881DB391747979CEFEA006DEE3F8D8A9A228305DA972E25ED82A339CC734720F123093E5E4429F326B2338A9443868052B183B96298E4190839E873478CAD7D0655EE2745D0E9890045C9B22139141BD2A2BBE2, 0, CAST(N'2024-07-24T12:57:02.2293696' AS DateTime2), CAST(N'2024-07-24T12:57:02.2295599' AS DateTime2))
GO
INSERT [dbo].[UsersDetails] ([Id], [PasswordEncrypted], [PasswordHashKey], [Status], [Created_at], [Updated_at]) VALUES (2, 0x83D611AE04408A2B2FC1C22D708896762637B39F282E889BFA36DE780184A8E21055A614936659F8FBEE688B365AA7A8E3D3308B03B7A266CD287FF68A66E069, 0xA0D712B783DB772A75F4B31D401C160249A12BA06679229C06C63BB9DC66056227DC16BC2FB7B449BE44F879249334B0F9EA6946E8DD57BD75EF6167993A82923F813DD668865B1ACBED82448F0D8D9A21A85FC941B60C3024B980102CFCBC4D1BE602FD74D42057054C78429BB59380A8F7D282E80E4492316D6F4AF0E72557, 1, CAST(N'2024-07-24T12:58:02.3325624' AS DateTime2), CAST(N'2024-07-24T12:58:02.3325855' AS DateTime2))
GO
INSERT [dbo].[UsersDetails] ([Id], [PasswordEncrypted], [PasswordHashKey], [Status], [Created_at], [Updated_at]) VALUES (3, 0x2D3FB53875305FADEA842AFB1A574392328B3AB04920006C8AC68FC5B159B5349D284E0F69F43091EFE0D21C807F3BF54DFA42B6EDED0DF1085F09469D89A855, 0x87146F30B4D1966145C7EE87CD1589EA1FCC21F8FE16A71777F7A132EB25C55B90C06CBADB061E821D798A469E5D390100960C65471490E689AECC83F07E30CD6A91AB719DEB7BF25FA625F58FCCEE5803F670B36E1C27F38C158FC96C13861DE9C1656AAD08410AACBBC86147BFF1C20E9B3A4CFAB83049F8A2459F708D73BE, 0, CAST(N'2024-07-24T13:58:04.6000795' AS DateTime2), CAST(N'2024-07-24T13:58:04.6006829' AS DateTime2))
GO
INSERT [dbo].[UsersDetails] ([Id], [PasswordEncrypted], [PasswordHashKey], [Status], [Created_at], [Updated_at]) VALUES (4, 0x8B78CA321F95FA29AA01CE778C8E35F12D552B6C656DE7A8AE7665B2C08E3B83C75F1683B6BFD1E526406CA35BDAC3CA63F05EBEEF42E23E8862B1050A2B07FA, 0xC0EBB06A93150BC893D3BAC702CA5AE3555E1B64A04DD0E78FCB54A206406998753BE618C9D460E96CC3C5265447686966E018D5A83FFD0977BE317A8EF152B196D118CC5C5D93BDC34B9AC88B9192034524EC807EECA992C85D84AB5332BEBFE1DA9984F27ADEFF9179C0BEAF6ADBEED59FFDB10A06EDF618161CF4D45A5B3D, 0, CAST(N'2024-07-24T13:58:43.6035967' AS DateTime2), CAST(N'2024-07-24T13:58:43.6035991' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[UsersDetails] OFF
GO

INSERT [dbo].[Users] ([UserId], [Name], [Age], [Gender], [Phone], [Email], [Role], [Created_at], [Updated_at], [is_preferenceSet]) VALUES (1, N'Han', 23, 1, N'9999999999', N'han@gmail.com', 0, CAST(N'2024-07-24T12:57:02.6878212' AS DateTime2), CAST(N'2024-07-24T12:57:02.6878804' AS DateTime2), 0)
GO
INSERT [dbo].[Users] ([UserId], [Name], [Age], [Gender], [Phone], [Email], [Role], [Created_at], [Updated_at], [is_preferenceSet]) VALUES (2, N'Han', 23, 1, N'9999999999', N'hani@gmail.com', 1, CAST(N'2024-07-24T12:58:02.3823378' AS DateTime2), CAST(N'2024-07-24T12:58:02.3823395' AS DateTime2), 0)
GO
INSERT [dbo].[Users] ([UserId], [Name], [Age], [Gender], [Phone], [Email], [Role], [Created_at], [Updated_at], [is_preferenceSet]) VALUES (3, N'Hani', 21, 0, N'9999988888', N'hanilar@gmail.com', 0, CAST(N'2024-07-24T13:58:05.4402976' AS DateTime2), CAST(N'2024-07-24T13:58:05.4403746' AS DateTime2), 1)
GO
INSERT [dbo].[Users] ([UserId], [Name], [Age], [Gender], [Phone], [Email], [Role], [Created_at], [Updated_at], [is_preferenceSet]) VALUES (4, N'Hanh', 21, 1, N'9999988888', N'hann@gmail.com', 1, CAST(N'2024-07-24T13:58:43.6410045' AS DateTime2), CAST(N'2024-07-24T13:58:43.6410065' AS DateTime2), 1)
GO

SET IDENTITY_INSERT [dbo].[MonitorPreferences] ON 
GO
INSERT [dbo].[MonitorPreferences] ([CoachId], [MetricId], [Created_at], [Updated_at], [Id]) VALUES (4, 11, CAST(N'2024-07-24T17:29:50.7416891' AS DateTime2), CAST(N'2024-07-24T17:29:50.7420626' AS DateTime2), 11)
GO
INSERT [dbo].[MonitorPreferences] ([CoachId], [MetricId], [Created_at], [Updated_at], [Id]) VALUES (4, 7, CAST(N'2024-07-24T17:29:51.4089636' AS DateTime2), CAST(N'2024-07-24T17:29:51.4089656' AS DateTime2), 12)
GO
INSERT [dbo].[MonitorPreferences] ([CoachId], [MetricId], [Created_at], [Updated_at], [Id]) VALUES (4, 12, CAST(N'2024-07-24T17:29:51.4329753' AS DateTime2), CAST(N'2024-07-24T17:29:51.4329772' AS DateTime2), 13)
GO
INSERT [dbo].[MonitorPreferences] ([CoachId], [MetricId], [Created_at], [Updated_at], [Id]) VALUES (4, 13, CAST(N'2024-07-25T20:31:33.1584878' AS DateTime2), CAST(N'2024-07-25T20:31:33.1585706' AS DateTime2), 14)
GO
INSERT [dbo].[MonitorPreferences] ([CoachId], [MetricId], [Created_at], [Updated_at], [Id]) VALUES (4, 10, CAST(N'2024-07-25T20:33:07.6268927' AS DateTime2), CAST(N'2024-07-25T20:33:07.6268945' AS DateTime2), 15)
GO
INSERT [dbo].[MonitorPreferences] ([CoachId], [MetricId], [Created_at], [Updated_at], [Id]) VALUES (4, 8, CAST(N'2024-07-25T20:43:24.8672274' AS DateTime2), CAST(N'2024-07-25T20:43:24.8673224' AS DateTime2), 16)
GO
SET IDENTITY_INSERT [dbo].[MonitorPreferences] OFF
GO

SET IDENTITY_INSERT [dbo].[UserPreferences] ON 
GO
INSERT [dbo].[UserPreferences] ([UserId], [MetricId], [Created_at], [Updated_at], [Id]) VALUES (3, 6, CAST(N'2024-07-24T17:28:00.8903660' AS DateTime2), CAST(N'2024-07-24T17:28:00.8907566' AS DateTime2), 4)
GO
INSERT [dbo].[UserPreferences] ([UserId], [MetricId], [Created_at], [Updated_at], [Id]) VALUES (3, 7, CAST(N'2024-07-24T17:28:01.6419473' AS DateTime2), CAST(N'2024-07-24T17:28:01.6419495' AS DateTime2), 5)
GO
INSERT [dbo].[UserPreferences] ([UserId], [MetricId], [Created_at], [Updated_at], [Id]) VALUES (3, 8, CAST(N'2024-07-24T17:28:01.6674036' AS DateTime2), CAST(N'2024-07-24T17:28:01.6674053' AS DateTime2), 6)
GO
INSERT [dbo].[UserPreferences] ([UserId], [MetricId], [Created_at], [Updated_at], [Id]) VALUES (3, 13, CAST(N'2024-07-25T16:55:24.3946606' AS DateTime2), CAST(N'2024-07-25T16:55:24.3951663' AS DateTime2), 7)
GO
INSERT [dbo].[UserPreferences] ([UserId], [MetricId], [Created_at], [Updated_at], [Id]) VALUES (3, 10, CAST(N'2024-07-25T17:02:23.2528967' AS DateTime2), CAST(N'2024-07-25T17:02:23.2530183' AS DateTime2), 8)
GO
INSERT [dbo].[UserPreferences] ([UserId], [MetricId], [Created_at], [Updated_at], [Id]) VALUES (3, 9, CAST(N'2024-07-25T17:09:45.1664433' AS DateTime2), CAST(N'2024-07-25T17:09:45.1670018' AS DateTime2), 9)
GO
SET IDENTITY_INSERT [dbo].[UserPreferences] OFF
GO

SET IDENTITY_INSERT [dbo].[Suggestions] ON 
GO
INSERT [dbo].[Suggestions] ([Id], [CoachId], [UserId], [Description], [Created_at], [Updated_at]) VALUES (1, 4, 3, N'Eat Well', CAST(N'2024-07-25T21:14:42.4225508' AS DateTime2), CAST(N'2024-07-25T21:14:42.4228424' AS DateTime2))
GO
INSERT [dbo].[Suggestions] ([Id], [CoachId], [UserId], [Description], [Created_at], [Updated_at]) VALUES (2, 4, 3, N'Drink more water', CAST(N'2024-07-25T21:17:11.2606687' AS DateTime2), CAST(N'2024-07-25T21:17:11.2611478' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Suggestions] OFF
GO

SET IDENTITY_INSERT [dbo].[HealthLogs] ON 
GO
INSERT [dbo].[HealthLogs] ([Id], [PreferenceId], [value], [HealthStatus], [Created_at], [Updated_at]) VALUES (5, 6, 4, 2, CAST(N'2024-07-25T15:00:05.1491114' AS DateTime2), CAST(N'2024-07-25T20:42:12.4327364' AS DateTime2))
GO
INSERT [dbo].[HealthLogs] ([Id], [PreferenceId], [value], [HealthStatus], [Created_at], [Updated_at]) VALUES (6, 5, 6000, 1, CAST(N'2024-07-25T15:20:16.3069511' AS DateTime2), CAST(N'2024-07-25T16:46:05.3491964' AS DateTime2))
GO
INSERT [dbo].[HealthLogs] ([Id], [PreferenceId], [value], [HealthStatus], [Created_at], [Updated_at]) VALUES (8, 7, 19, 0, CAST(N'2024-07-25T17:00:50.8005114' AS DateTime2), CAST(N'2024-07-25T17:00:50.8010805' AS DateTime2))
GO
INSERT [dbo].[HealthLogs] ([Id], [PreferenceId], [value], [HealthStatus], [Created_at], [Updated_at]) VALUES (9, 9, 180, 3, CAST(N'2024-07-25T17:10:18.2292130' AS DateTime2), CAST(N'2024-07-25T17:10:18.2293615' AS DateTime2))
GO
INSERT [dbo].[HealthLogs] ([Id], [PreferenceId], [value], [HealthStatus], [Created_at], [Updated_at]) VALUES (10, 8, 52, 2, CAST(N'2024-07-25T17:12:57.9689782' AS DateTime2), CAST(N'2024-07-25T17:26:17.2875959' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[HealthLogs] OFF
GO

SET IDENTITY_INSERT [dbo].[Targets] ON 
GO
INSERT [dbo].[Targets] ([Id], [PreferenceId], [TargetMinValue], [TargetMaxValue], [TargetStatus], [TargetDate], [Created_at], [Updated_at]) VALUES (9, 6, 8, 10, 1, CAST(N'2024-07-27T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T15:04:54.7866089' AS DateTime2), CAST(N'2024-07-25T20:43:44.2004022' AS DateTime2))
GO
INSERT [dbo].[Targets] ([Id], [PreferenceId], [TargetMinValue], [TargetMaxValue], [TargetStatus], [TargetDate], [Created_at], [Updated_at]) VALUES (10, 5, 6000, 10000, 0, CAST(N'2024-07-29T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T15:17:46.5366892' AS DateTime2), CAST(N'2024-07-25T16:46:05.3967317' AS DateTime2))
GO
INSERT [dbo].[Targets] ([Id], [PreferenceId], [TargetMinValue], [TargetMaxValue], [TargetStatus], [TargetDate], [Created_at], [Updated_at]) VALUES (11, 7, 19, 22, 0, CAST(N'2024-07-29T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T16:56:12.9037207' AS DateTime2), CAST(N'2024-07-25T20:42:32.4426660' AS DateTime2))
GO
INSERT [dbo].[Targets] ([Id], [PreferenceId], [TargetMinValue], [TargetMaxValue], [TargetStatus], [TargetDate], [Created_at], [Updated_at]) VALUES (12, 8, 50, 55, 0, CAST(N'2024-07-29T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-25T17:13:35.0627982' AS DateTime2), CAST(N'2024-07-25T17:26:23.0412029' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Targets] OFF
GO
