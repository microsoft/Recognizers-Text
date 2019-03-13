# Bot Prerequisites
This bot has prerequisites that must be installed in order for the bot to function properly.

This document will enumerate the required prerequisites and show how to install them.

## Overview
Prerequisites are provided that will enable the bot to be deployed to Azure using additional CLI tools.

## Prerequisites
- [.NET Core SDK][2] version 2.1 or higher
	```bash
	# determine dotnet version
	dotnet --version
	```
- If you don't have an Azure subscription, create a [free account][3].
- Install the latest version of the [Azure CLI][4] tool. Version 2.0.54 or higher.
- Install latest version of the `MSBot` CLI tool. Version 4.3.2 or higher.
    ```bash
    # install msbot CLI tool
    npm install -g msbot
    ```
[Return to README.md][1]

[1]: ./README.md
[2]: https://nodejs.org
[3]: https://azure.microsoft.com/free/
[4]: https://docs.microsoft.com/cli/azure/install-azure-cli?view=azure-cli-latest
