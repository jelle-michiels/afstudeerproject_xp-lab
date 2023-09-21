# 3DModularLearningEnvironment

Opdrachgever:
David Vandenbroeck

teamleden:
Jelle Michiels
Michiel De Bruyn
Patryk Piekarz
Jasper Coddens

Coach:
Daan Nijs


# set-up

## step 1

install docker desktop

## step 2

download  Microsoft ODBC Driver 17 for SQL Server
via: https://learn.microsoft.com/en-us/sql/connect/odbc/download-odbc-driver-for-sql-server?view=sql-server-ver16

MAKE SURE TO GET VERSION 17


## step 3

download sqlcmd utility
via: https://learn.microsoft.com/en-us/sql/tools/sqlcmd/sqlcmd-utility?view=sql-server-ver16&tabs=odbc%2Cwindows

restart pc before next step

## step 4
start docker desktop

go to the folder afstudeerproject_xp-lab\Assets\plugins 

next right click git bash here
and type 'docker compose up'

wait for the install and check docker desktop to see if the container is running


## step 5

open powershell and run 'sqlcmd -S localhost,1433 -U sa -P Password1234 -d master'

if you see '1>' you are in the sql server console

next copy the content of create database script from the folder mentioned above

paste the copied content in powershell press enter once and type 'go' press enter again

now the database should be created

to check this use the command 'select name from sys.databases'

to run a command in this server you need to type go on the next line before it runs.