FROM mcr.microsoft.com/dotnet/aspnet:7.0 
WORKDIR /app

# Find all *.crt files, copy them to ca-certificates folder and run certificate update.
COPY *.crt /usr/local/share/ca-certificates/
RUN update-ca-certificates

# Install prerequisites
RUN apt-get update && \
    apt-get install -y wget unzip 

# Download and unpack terraform
RUN wget https://releases.hashicorp.com/terraform/1.3.7/terraform_1.3.7_linux_amd64.zip && \
    unzip terraform_1.3.7_linux_amd64.zip -d /usr/bin && \    
    chmod +x /usr/bin/terraform && \
    rm terraform_1.3.7_linux_amd64.zip

# Include test terraform file
COPY test.tf /app/

ENTRYPOINT terraform init && terraform apply -auto-approve