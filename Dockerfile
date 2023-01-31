# Set this argument to 1 or 0 to include or not include ca certificates from res 
ARG ADD_CA_CERTS=0

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# Copy certificates only if ADD_CA_CERTS argument is set to 1
FROM base as ca-certs-1
ONBUILD COPY res/*.crt /usr/local/share/ca-certificates/
ONBUILD RUN update-ca-certificates

# if ADD_CA_CERTS is empty or is set to 0 then only echo the message.
FROM base as ca-certs-
FROM base as ca-certs-0
ONBUILD RUN echo "Build without ca certificates"

# Trigger COPY and RUN defined in "ca-cert-1" when ADD_CA_CERTS variable is set to 1.
FROM ca-certs-${ADD_CA_CERTS}



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