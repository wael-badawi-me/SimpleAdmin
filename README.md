# Simple Admin

This project is in response to the FSIT System Analyst.

## Getting Started

### Clone the Repository

First, clone this repository to your local machine:

    git clone https://github.com/wael-badawi-me/SimpleAdmin.git

### Set Up the Database

1. Navigate to the `SimpleAdmin.DataAccess` directory.
2. Download `SimpleAdmin.bak`.
3. Restore the database using the downloaded `.bak` file.

### Update Connection Strings

1. Navigate to the `SimpleAdmin.API` directory.
2. Update the connection string in `appsettings.json` with your database details.

### Run the API

1. Run the `SimpleAdmin.API` project using IIS.
2. Get the API link.

### Configure Blazor UI

1. Navigate to the `SimpleAdmin.BlazorUI` directory.
2. Update `appsettings.json`:
   - Set `SimpleAdminBase` to your API link.
   - Set `SimpleAdminBasePublic` to your API link.

### Run the Application

1. Run the `SimpleAdmin.BlazorUI` project.

### Default Credentials

Use the following credentials to log in:

- **Username:** admin@admin.com
- **Password:** admin123$


## Contact

If you have any questions or need further assistance, please contact Wael Badawi at wael.badawi2000@gmail.com.
