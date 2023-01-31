# Set this argument to 1 or 0 to include or not include ca certificates from res 
ARG ADD_CA_CERTS=0

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ADD res/*.crt /usr/local/share/ca-certificates/
RUN update-ca-certificates

# Install prerequisites
RUN apt-get update && \
    apt-get install -y unzip 

# Download and unpack terraform
ADD https://releases.hashicorp.com/terraform/1.3.7/terraform_1.3.7_linux_amd64.zip .
RUN unzip terraform_1.3.7_linux_amd64.zip -d /usr/bin && \
    chmod +x /usr/bin/terraform && \
    rm terraform_1.3.7_linux_amd64.zip

# Copy web application
COPY src/Terragate.Api/bin/Release/net7.0/linux-x64/publish .

# Run api in developemnt mode with swagger
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "Terragate.Api.dll"]