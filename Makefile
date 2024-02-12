sonar-scan:
	dotnet clean
	dotnet sonarscanner begin /k:"food-totem-catalog" /d:sonar.host.url="http://localhost:9000"  /d:sonar.token="sqp_ee45e83a1f3f074acf357a8e5854587471fa571d" /d:sonar.cs.opencover.reportsPaths="**\TestResults\*\*.xml"
	dotnet build
	dotnet test --no-build --collect:"XPlat Code Coverage" -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover
	dotnet sonarscanner end /d:sonar.token="sqp_ee45e83a1f3f074acf357a8e5854587471fa571d"

test:
	dotnet clean
	dotnet build
	dotnet test --no-build --collect:"XPlat Code Coverage" -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover

full-clean:
	find . -type d -name "bin" -o -name "obj" -o -name "TestResults" | xargs rm -rf
	dotnet clean

run-services:
	cd src; docker-compose build --no-cache;
	cd src; docker-compose up -d

run-database:
	cd infra/local;	docker-compose up -d catalog-database

run-api:
	cd infra/local; docker-compose up -d catalog-api

stop-services:
	cd src; docker-compose down