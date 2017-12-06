# Get this up and Running on a Mac

## Install Docker
[Docker for Mac](https://www.docker.com/docker-mac)

## Setup Docker
Changes I did below link

Follow this post https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker

1. Didn't need sudo
2. In Step 2 use -p 1433:1433 (This allows using localhost and not IP address and port)
3. in Step 2 I named my sql-server
4. I didn't change the SA Password
5. I stopped here (after talk of changing password)
6. I use 1 of the following for testing connections and interacting with the server
  * [Microsoft Sql Operations Studio](https://docs.microsoft.com/en-us/sql/sql-operations-studio/download)
  * [VS Code mssql extension](https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-develop-use-vscode)

** NOTE**
  
If you remove the container you created, you will lose all data.
  
If you want a separate container for data follow this http://thedatafarm.com/data-access/mashup-sql-server-on-linux-in-docker-on-a-mac-with-visual-studio-code/

If you delete the Data Volume container, you will lose all data.

## Terminal Work
1. From Project Root (where csproj file is) run `dotnet restore`
2. From Project Root (where newmigration.sh is) run `chmod +x ./newmigration.sh`
3. From Project Root (where csproj file is) run `dotnet user-secrets localdbconnection "<your connection string here>"`

## Code Editing

You can use either VS for Mac or VS Code.  I prefer VS Code due to better syntax highlighting for sql files, but both will work.

There is a manual piece to adding `.sql` files as Embedded Resources.  Easier in VS for Mac, but not much.

## Adding new Scripts

For non Migrations just add a file to the correct location.

For Migration scripts we need to use the terminal.
1. navigate to project root folder where newmigration.sh is 
2. run ./newmigration version name
  * version is current version folder.  Must exist
  * name is a friendly name for the script.  Enclose in quotes if spaces in name
  * this uses UTC timestamps for the first part of the name.  This is **Absolutely** necessary to keep order, and the reason for the script.

Whichever type of file was added, it needs to be marked as an embeded resource in the `.csproj` file.

VS for Mac
* Right Click In Solution Explorer where you want the file chose `Add Files...`
* Add the file
* Right Click the file chose `Properties`
* Change Build Action to `EmbededResource`

VS Code
* Edit `.csproj` file
* Add a `<None Remove="filename"/>` by the others (Prevents Duplicate showings of file in VS)
* Add a ` <EmbeddedResource Include="filename" />` (Embeds file like it needs to be)

## Run this thing

The first time it will have to be run from the command line, since the flag for creating the database is defaulted to False.

### First Time

run `dotnet run --create-db`

### Additional Runs

run `dotnet run` (Flags can be added, see Program Start for available flags)

Use VS Mac

Use VS Code