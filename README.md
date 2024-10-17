
## Getting Started

To run this project, you will need to have Visual Studio(Visual Studio 2022 IDE must be version 17.8+ to be compatible with .net 8) and .net8. You will also need a Microsoft SQL Server and an ApiKey from TheCatApi (https://thecatapi.com/) to be able to get the cat images.

`API_KEY`

`ConnectionString`


## Cloning the Project

To clone this project, follow these steps:

1. **Open a Terminal or Command Prompt**:
   - On Windows, you can use Command Prompt, PowerShell, or Git Bash.
   - On macOS or Linux, you can use the Terminal.

2. **Navigate to Your Desired Directory**:
   Change to the directory where you want to clone the project. And type the following command.

```

git clone https://github.com/pantelizer/StealAllTheCats.git


```
   


## Building and Running the project

After cloning the project, you can build and run the application by following these steps:

1. Restore Dependencies

```

dotnet restore

```

2. Build the Application:

```

dotnet build


```
3. Run the Application:

```

dotnet run


```

## Setting up the Api Key

To get things started we first want to get an Api Key. This can be achieved by going on https://thecatapi.com/ selecting the Get your Api Key option and choosing either the free or paid option. After that you just have to enter an email and a description of your app and a key will be sent to your email afterwards.


After getting this Key you will have to open the solution, navigate to the project called StealAllTheCats and then go to appsettings.json. Then you will have to copy the Api Key that you received on the "ApiKey" which is located inside the "CatApiSettings" object.

```

"CatApiSettings": {
  "ApiKey": "Place Api key here!!"
}

```



## Configuring the Database


Now to configure the Database you need to first place the connection string in the appsettings.json just as you did with the Api Key, but in a different object. You have to replace the string that's in the "DefaultConnection" which is inside the "ConnectionStrings" object.

```

  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CatsDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }


```

You are now ready to add your Migration. To do that you have to open the Package Manager Console, and then use the following command to add a migration.
```

Add-Migration InitialCreate

```


Afterwards to apply the migration and update your database schema, go to the Package Manager Console again, and run the following command:

```

Update-Database

```

And now you are ready to steal as many cat pictures as you want!