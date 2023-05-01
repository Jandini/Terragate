FROM mcr.microsoft.com/dotnet/aspnet:7.0 as base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Add certificates for development purposes
# ADD res/*.crt /usr/local/share/ca-certificates/
# RUN update-ca-certificates

# Install prerequisites
RUN apt-get update && \
    apt-get install -y unzip

# Download and unpack terraform
ADD https://releases.hashicorp.com/terraform/1.3.7/terraform_1.3.7_linux_amd64.zip /tmp
RUN unzip /tmp/terraform_1.3.7_linux_amd64.zip -d /usr/bin && \
    chmod +x /usr/bin/terraform && \
    rm /tmp/terraform_1.3.7_linux_amd64.zip

# Copy web application
COPY src/Terragate.Api/bin/Release/net7.0/linux-x64/publish .

# Get configuration from environment variables via --build-arg
ARG VRA_USER
ARG VRA_TENANT
ARG VRA_HOST
ENV TF_VRA_HOST=$VRA_HOST
ENV TF_VRA_USER=$VRA_USER
ENV TF_VRA_TENANT=$VRA_TENANT

ENTRYPOINT ["dotnet", "Terragate.Api.dll"]