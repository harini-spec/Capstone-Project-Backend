USE [dbHealthTracker]
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
INSERT [dbo].[Metrics] ([Id], [MetricType], [MetricUnit], [Created_at], [Updated_at]) VALUES (9, N'Height', N'Feet', CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Metrics] ([Id], [MetricType], [MetricUnit], [Created_at], [Updated_at]) VALUES (10, N'Weight', N'Kg', CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Metrics] ([Id], [MetricType], [MetricUnit], [Created_at], [Updated_at]) VALUES (11, N'Calories_Intake', N'kcal', CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Metrics] ([Id], [MetricType], [MetricUnit], [Created_at], [Updated_at]) VALUES (12, N'Calories_Burned', N'kcal', CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-24T00:00:00.0000000' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Metrics] OFF
GO
