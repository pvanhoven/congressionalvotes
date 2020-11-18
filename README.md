# Congressional Votes

Application for viewing the voting records of recent senate sessions

## Azure Storage Account

https://congressvotesstorage.z21.web.core.windows.net/  
Storage account is configured with enabled static website. Deployment occurs via Azure devops pipeline to this storage account and App Service for API.

## Azure Static Web App

https://brave-island-0dc64c81e.azurestaticapps.net/
Static web app is deployed via github action. No api deployment to App Service is included in the github action.

Free tier services are slow, please be patient!

## Projects

### congress

API for front end application

### congress_downloader

Application downloads xml files containing senate voting records

### conress_importer

Application loads downloaded xml files and inserts/updates data to database

### congress.tests

Project containing .net core, xunit based tests

### congressional-votes

Angular project to view senate voting records
