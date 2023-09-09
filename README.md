# MagicLink

## Overview
MagicLink is a system that utilizes RabbitMQ for background processing (Worker module) and ASP.NET Core for building the API. It allows users to generate and use Magic Links for authentication and access control.

## Prerequisites
Before running this project, ensure you have the following prerequisites installed:

- **RabbitMQ**: You need RabbitMQ installed and running to use the worker module for background processing. You can download it from [here](https://www.rabbitmq.com/download.html).

## Configuration
To configure the MagicLink project, you need to set an environment variable:

- `SECURITY_KEY`: This is the security key used for generating JSON Web Tokens (JWT) to secure the API. Replace `<your_security_key>` with your chosen security key.

You can set environment variables in different ways depending on your development environment. For example, in Windows, you can set them using the command prompt:

```shell
setx SECURITY_KEY "<your_security_key>"
```

Or, in Unix-based systems (Linux/macOS), you can add the following line to your shell profile (e.g., `.bashrc`, `.zshrc`, or `.profile`):

```shell
export SECURITY_KEY="<your_security_key>"
```

## Running the Project
To run the MagicLink project, follow these steps:

1. Clone this repository to your local machine:

```shell
git clone https://github.com/your-username/magiclink.git
```

2. Navigate to the project directory:

```shell
cd magiclink
```

3. Restore the required packages:

```shell
dotnet restore
```

4. Build the project:

```shell
dotnet build
```

5. Set the SECURITY_KEY environment variable as mentioned in the Configuration section.

6. Start the ASP.NET Core API:

```shell
dotnet run --project MagicLink.Api
```

7. Start the RabbitMQ worker module:

```shell
dotnet run --project MagicLink.Worker
```

The API should now be accessible at http://localhost:5000. The RabbitMQ worker module will be listening for background tasks.

## Project Structure
The project structure is organized as follows:

- `MagicLink.API`: Contains the ASP.NET Core API code.
- `MagicLink.Worker`: Contains the RabbitMQ worker module code.
- `MagicLink.Shared`: Shared code and utilities used by both the API and worker.

## Contributing


## License


## Issues
Since the project is still a work in progress, there may be ongoing refactoring and improvements to be made.
