## Technologies and features

### Server
Chpokk is built using ASP.Net 4.5 using the FubuMVC framework (https://fubumvc.github.io/, not actively developed now). The main server code is in the Features folder. I used Spark view engine (https://github.com/SparkViewEngine/spark). Elmah is used for automated error reporting, LibGit2Sharp for working with Git repositories, Roslyn -- for providing Intellisense, Simple.Data -- for ORM, StructureMap -- as a DI container. The user code is stored in /UserFiles and backed up to S3. Project templates are stored in /SystemFiles/Templates/ProjectTemplates. There is a homemade blog that uses markdown files as posts (put in the /Blog folder) and renders them using Tanka. SendGrid is used for sending emails.

There's a unit test runner (meaning users can write and run unit tests online) built using Gallio (which is also abandoned I'm afraid).

There are unit tests, but I wasn't running them regularly, so they're not reliable.


### Client
HTML is done using Boostrap. Client code is jQuery. The code editor is Ace (https://ace.c9.io/).


### Known issues
Azure doesn't like ".config" files that it finds in project templates during deployment, so creating a project from template stopped working after I moved to Azure.

## Installation
I'm hosting it on Azure Websites, 32-bit, Integrated pipeline, HTTP1.1. Using MSSQL for storing data (users and visit data) -- you should import the bacpac file into your database. Everything works just fine within the free plan.

Settings: 
SENDGRID_KEY

Connection strings: 
Simple.Data.Properties.Settings.DefaultConnectionString

