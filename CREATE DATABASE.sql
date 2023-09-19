CREATE DATABASE [3D_Modular_Gaming_Tool];
GO
USE [3D_Modular_Gaming_Tool];
GO

CREATE TABLE [dbo].[Player] (
    [playerID]  INT          IDENTITY (1, 1) NOT NULL,
    [firstName] VARCHAR (50) NULL,
    [lastName]  VARCHAR (50) NULL,
    CONSTRAINT [PK_NewTable] PRIMARY KEY CLUSTERED ([playerID] ASC)
);

CREATE TABLE [dbo].[Level] (
    [levelName]   VARCHAR (50) NOT NULL,
    [minimumTime] INT          CONSTRAINT [DEFAULT_Level_minimumTime] DEFAULT ((5)) NOT NULL,
    [maximumTime] INT          CONSTRAINT [DEFAULT_Level_maximumTime] DEFAULT ((10)) NOT NULL,
    CONSTRAINT [PK_Level] PRIMARY KEY CLUSTERED ([levelName] ASC),
    CONSTRAINT [CK_Level_1] CHECK ([maximumTime]>[minimumTime])
);

CREATE TABLE [dbo].[Score] (
    [scoreID]  INT          IDENTITY (1, 1) NOT NULL,
    [score]    INT          NOT NULL,
    [playerID] INT          NOT NULL,
    [level]    VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Score] PRIMARY KEY CLUSTERED ([scoreID] ASC),
    CONSTRAINT [FK_Score_Level] FOREIGN KEY ([level]) REFERENCES [dbo].[Level] ([levelName]),
    CONSTRAINT [FK_Score_Player] FOREIGN KEY ([playerID]) REFERENCES [dbo].[Player] ([playerID]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[Floor] (
    [floorNumber] INT           NOT NULL,
    [floorplan]   VARCHAR (300) NULL,
    [level]       VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_Floor] PRIMARY KEY CLUSTERED ([floorNumber] ASC, [level] ASC),
    CONSTRAINT [FK_Floor_Level] FOREIGN KEY ([level]) REFERENCES [dbo].[Level] ([levelName]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[Object] (
    [objectID] INT            IDENTITY (1, 1) NOT NULL,
    [type]     VARCHAR (50) NOT NULL,
    [position] VARCHAR (50)   NOT NULL,
    [rotation] VARCHAR (50)   NOT NULL,
    [scale]    VARCHAR (50)   NOT NULL,
    [level]    VARCHAR (50)   NOT NULL,
    CONSTRAINT [PK_Object] PRIMARY KEY CLUSTERED ([objectID] ASC),
    CONSTRAINT [FK_Object_Level] FOREIGN KEY ([level]) REFERENCES [dbo].[Level] ([levelName]) ON DELETE CASCADE
);