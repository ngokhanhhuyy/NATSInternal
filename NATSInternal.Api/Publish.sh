# Publish
dotnet publish -c Release -r osx-arm64 \
  --self-contained true \
  /p:PublishSingleFile=true \
  /p:PublishTrimmed=false \
  /p:IncludeAllContentForSelfExtract=true \
  /p:DebugType=None \
  -o ./publish
  
# Docker build
cd ../
docker build -t natsinternal-api -f NATSInternal.Api/Dockerfile .

# Docker container run (MacOS)
docker run -p 5000:5000 --name natsinternal natsinternal-api

# Rocker container run (Linux)
docker run -p 5000:5000 --add-host=host.docker.internal:host-gateway --name natsinternal-api natsinternal-api
