USE [master]
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'$(database)')
DROP DATABASE [$(database)]
GO

USE [master]
GO

CREATE DATABASE [$(database)] ON  PRIMARY 
( NAME = N'$(database)', FILENAME = N'$(path)\$(database).mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'$(database)_log', FILENAME = N'$(path)\$(database)_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
