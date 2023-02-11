variable "VRA_USER" {}
variable "VRA_PASS" {}
variable "VRA_TENANT" {}
variable "VRA_HOST" {}

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
