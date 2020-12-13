# DeleteBackUpFileService
.NET Core Worker Service works like Windows Service. This service works background to delete database backup file once determined day.

In order to create windows service via publish, open command prompt as administrator mode and type below path. If it is success, your service will be created.

sc create binPath="yourServiceExeFile"
