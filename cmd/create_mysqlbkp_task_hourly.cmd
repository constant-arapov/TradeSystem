schtasks /create /tn mysql_backup /SC HOURLY  /TR e:\Dropbox\proj\profinvest\plaza2\Plaza2Connector\Plaza2Connector\MySQLDumper\bin\x64\Release\MySQLDumper.exe /ST 12:21