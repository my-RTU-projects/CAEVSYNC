Add migration:
dotnet ef --startup-project ../CAEVSYNC.Api/ migrations add <MigrationName>

Update database:
dotnet ef --startup-project ../CAEVSYNC.Api/ database update
