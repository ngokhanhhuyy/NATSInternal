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

# Rocker container
docker run -d -p 8080:8080 --name nats-api natsinternal-api
