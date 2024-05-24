IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'FlashCard')
  BEGIN
    CREATE DATABASE [FlashCard]
    USE [master]
    ALTER AUTHORIZATION ON DATABASE::[FlashCard] TO [sa];
  END
