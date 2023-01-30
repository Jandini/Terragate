variable "VRA_USER" {}
variable "VRA_PASS" {}
variable "VRA_TENANT" {}
variable "VRA_HOST" {}

variable "DEPLOYMENT_CATALOG_NAME" {}
variable "DEPLOYMENT_COUNT" { default = 1 }
variable "DEPLOYMENT_DESCRIPTION" { default = "Simple deployment with one virtual machine in the cluster." }

variable "RESOURCE_COMPONENT_NAME" {}
variable "RESOURCE_CLUSTER_SIZE" { default = 1 }
variable "RESOURCE_CPU" { default = 2 }
variable "RESOURCE_MEMORY" { default = 16384 }

terraform {
  required_providers {
    vra7 = {
      source = "vmware/vra7"
      version = "3.0.2"
    }
  }
}

provider "vra7" {
  username = var.VRA_USER
  password = var.VRA_PASS
  tenant   = var.VRA_TENANT
  host     = var.VRA_HOST
}


resource "vra7_deployment" "simple" {
  count             = var.DEPLOYMENT_COUNT
  description       = var.DEPLOYMENT_DESCRIPTION
  catalog_item_name = var.DEPLOYMENT_CATALOG_NAME
  resource_configuration {
    component_name = var.RESOURCE_COMPONENT_NAME
    cluster        = var.RESOURCE_CLUSTER_SIZE
    configuration = {
      cpu         = var.RESOURCE_CPU
      memory      = var.RESOURCE_MEMORY
      description = "Virtual machine #${count.index + 1}"   
    }
  }
}

output "hosts" {
    value = vra7_deployment.simple[*].resource_configuration[*].instances[*].properties.name
}
