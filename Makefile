# Project Variables
 PROoject Variables
ORG_JECT_NAME ?= SolarCoffee
REPONAME?= SolarCoffee
REPO_NAME ?= SolarCoffee
 .PH
 .PHONY: migrations db 
migr
migrations:
db      cd ./SolarCoffee.Data && dotnet ef --startup-project ../SolarCoffee.Web migraations add $(name) && cd
..
db:
        cd ./SolarCoffee.Data && dotnet ef --startup-project ../SolarCoffee.Web databse update  && cd 
