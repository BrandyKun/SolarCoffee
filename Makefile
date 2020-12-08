# Project Variables
PROJECT_NAME ?= SolarCoffe
ORG_NAME ?= SolarCoffee
REPO_NAME ?= SolarCoffee

.PHONY: migrations db

migrations:
     cd ./SolarCoffee.Data && dotnet ef --startup-project ../SolarCoffe.Web/ migrations add $(mname) && cd ..
db:
     cd ./SolarCoffee.Data && dotnet ef --startup-project ../SolarCoffe.Web/ database update && cd ..