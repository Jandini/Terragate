FROM mcr.microsoft.com/dotnet/aspnet:7.0 
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Find all *.crt files, copy them to ca-certificates folder and run certificate update.
COPY res/*.crt "/usr/local/share/ca-certificates/" /target
RUN update-ca-certificates

# Install prerequisites
RUN apt-get update && \
    apt-get install -y wget unzip 

# Download and unpack terraform
RUN wget https://releases.hashicorp.com/terraform/1.3.7/terraform_1.3.7_linux_amd64.zip && \
    unzip terraform_1.3.7_linux_amd64.zip -d /usr/bin && \    
    chmod +x /usr/bin/terraform && \
    rm terraform_1.3.7_linux_amd64.zip

# Copy web application
COPY src/Terragate.Api/bin/Release/net7.0/linux-x64/publish .

# Run api in developemnt mode with swagger
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "Terragate.Api.dll"]
