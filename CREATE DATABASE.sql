-- Check if the database exists
IF DB_ID('3D_Modular_Gaming_Tool') IS NULL
BEGIN
    -- Create the database
    CREATE DATABASE [3D_Modular_Gaming_Tool];
END
GO

-- Use the database
USE [3D_Modular_Gaming_Tool];
GO

-- Check if the Player table exists
IF OBJECT_ID('dbo.Player', 'U') IS NULL
BEGIN
    -- Create the Player table
    CREATE TABLE [dbo].[Player] (
        [playerID]  INT          IDENTITY (1, 1) NOT NULL,
        [firstName] VARCHAR (50) NULL,
        [lastName]  VARCHAR (50) NULL,
        CONSTRAINT [PK_NewTable] PRIMARY KEY CLUSTERED ([playerID] ASC)
    );
END

-- Check if the Level table exists
IF OBJECT_ID('dbo.Level', 'U') IS NULL
BEGIN
    -- Create the Level table
    CREATE TABLE [dbo].[Level] (
        [levelName]   VARCHAR (50) NOT NULL,
        [minimumTime] INT          CONSTRAINT [DEFAULT_Level_minimumTime] DEFAULT ((5)) NOT NULL,
        [maximumTime] INT          CONSTRAINT [DEFAULT_Level_maximumTime] DEFAULT ((10)) NOT NULL,
        CONSTRAINT [PK_Level] PRIMARY KEY CLUSTERED ([levelName] ASC),
        CONSTRAINT [CK_Level_1] CHECK ([maximumTime]>[minimumTime])
    );
END

-- Check if the Score table exists
IF OBJECT_ID('dbo.Score', 'U') IS NULL
BEGIN
    -- Create the Score table
    CREATE TABLE [dbo].[Score] (
        [scoreID]  INT          IDENTITY (1, 1) NOT NULL,
        [score]    INT          NOT NULL,
        [playerID] INT          NOT NULL,
        [level]    VARCHAR (50) NOT NULL,
        CONSTRAINT [PK_Score] PRIMARY KEY CLUSTERED ([scoreID] ASC),
        CONSTRAINT [FK_Score_Level] FOREIGN KEY ([level]) REFERENCES [dbo].[Level] ([levelName]),
        CONSTRAINT [FK_Score_Player] FOREIGN KEY ([playerID]) REFERENCES [dbo].[Player] ([playerID]) ON DELETE CASCADE
    );
END

-- Check if the Floor table exists
IF OBJECT_ID('dbo.Floor', 'U') IS NULL
BEGIN
    -- Create the Floor table
    CREATE TABLE [dbo].[Floor] (
        [floorNumber] INT           NOT NULL,
        [floorplan]   VARCHAR (300) NULL,
        [level]       VARCHAR (50)  NOT NULL,
        CONSTRAINT [PK_Floor] PRIMARY KEY CLUSTERED ([floorNumber] ASC, [level] ASC),
        CONSTRAINT [FK_Floor_Level] FOREIGN KEY ([level]) REFERENCES [dbo].[Level] ([levelName]) ON DELETE CASCADE
    );
END

-- Check if the Object table exists
IF OBJECT_ID('dbo.Object', 'U') IS NULL
BEGIN
    -- Create the Object table
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
END