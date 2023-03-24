using AutoMapper.Configuration.Annotations;
using System.Text.Json.Serialization;

namespace Terragate.Api.Services
{
    public class TerraformState
    {
        public class Configuration
        {
            [JsonPropertyName("cpu")]
            public string? Cpu { get; set; }

            [JsonPropertyName("description")]
            public string? Description { get; set; }

            [JsonPropertyName("memory")]
            public string? Memory { get; set; }
        }

        public class Instance
        {
            [JsonPropertyName("index_key")]
            public int IndexKey { get; set; }

            [JsonPropertyName("schema_version")]
            public int SchemaVersion { get; set; }

            [JsonPropertyName("attributes")]
            public Attributes? Attributes { get; set; }

            [JsonPropertyName("sensitive_attributes")]
            public List<object>? SensitiveAttributes { get; set; }

            [JsonPropertyName("private")]
            public string? Private { get; set; }

            [JsonPropertyName("description")]
            public string? Description { get; set; }

            [JsonPropertyName("ip_address")]
            public string? IpAddress { get; set; }

            [JsonPropertyName("name")]
            public string? Name { get; set; }

            [JsonPropertyName("properties")]
            public Properties? Properties { get; set; }

            [JsonPropertyName("resource_id")]
            public string? ResourceId { get; set; }

            [JsonPropertyName("resource_type")]
            public string? ResourceType { get; set; }
        }

        public class Owner
        {
            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("name")]
            public string? Name { get; set; }
        }

        public class Properties
        {
            [JsonPropertyName("Cafe.Shim.VirtualMachine.TotalStorageSize")]
            public string? CafeShimVirtualMachineTotalStorageSize { get; set; }

            [JsonPropertyName("ChangeLease")]
            public string? ChangeLease { get; set; }

            [JsonPropertyName("ChangeOwner")]
            public string? ChangeOwner { get; set; }

            [JsonPropertyName("Component")]
            public string? Component { get; set; }

            [JsonPropertyName("ConnectViaNativeVmrc")]
            public string? ConnectViaNativeVmrc { get; set; }

            [JsonPropertyName("ConnectViaRdp")]
            public string? ConnectViaRdp { get; set; }

            [JsonPropertyName("ConnectViaVmrc")]
            public string? ConnectViaVmrc { get; set; }

            [JsonPropertyName("CreateSnapshot")]
            public string? CreateSnapshot { get; set; }

            [JsonPropertyName("Destroy")]
            public string? Destroy { get; set; }

            [JsonPropertyName("EXTERNAL_REFERENCE_ID")]
            public string? EXTERNALREFERENCEID { get; set; }

            [JsonPropertyName("Expire")]
            public string? Expire { get; set; }

            [JsonPropertyName("Extensibility.Lifecycle.Properties.VMPSMasterWorkflow32.BuildingMachine")]
            public string? ExtensibilityLifecyclePropertiesVMPSMasterWorkflow32BuildingMachine { get; set; }

            [JsonPropertyName("Extensibility.Lifecycle.Properties.VMPSMasterWorkflow32.Disposing")]
            public string? ExtensibilityLifecyclePropertiesVMPSMasterWorkflow32Disposing { get; set; }

            [JsonPropertyName("Extensibility.Lifecycle.Properties.VMPSMasterWorkflow32.MachineActivated")]
            public string? ExtensibilityLifecyclePropertiesVMPSMasterWorkflow32MachineActivated { get; set; }

            [JsonPropertyName("Extensibility.Lifecycle.Properties.VMPSMasterWorkflow32.MachineProvisioned")]
            public string? ExtensibilityLifecyclePropertiesVMPSMasterWorkflow32MachineProvisioned { get; set; }

            [JsonPropertyName("Extensibility.Lifecycle.Properties.VMPSMasterWorkflow32.Requested")]
            public string? ExtensibilityLifecyclePropertiesVMPSMasterWorkflow32Requested { get; set; }

            [JsonPropertyName("Extensibility.Lifecycle.Properties.VMPSMasterWorkflow32.UnprovisionMachine")]
            public string? ExtensibilityLifecyclePropertiesVMPSMasterWorkflow32UnprovisionMachine { get; set; }

            [JsonPropertyName("IS_COMPONENT_MACHINE")]
            public string? ISCOMPONENTMACHINE { get; set; }

            [JsonPropertyName("InstallTools")]
            public string? InstallTools { get; set; }

            [JsonPropertyName("MachineBlueprintName")]
            public string? MachineBlueprintName { get; set; }

            [JsonPropertyName("MachineDailyCost")]
            public string? MachineDailyCost { get; set; }

            [JsonPropertyName("MachineDestructionDate")]
            public DateTime MachineDestructionDate { get; set; }

            [JsonPropertyName("MachineExpirationDate")]
            public DateTime MachineExpirationDate { get; set; }

            [JsonPropertyName("MachineGroupName")]
            public string? MachineGroupName { get; set; }

            [JsonPropertyName("MachineGuestOperatingSystem")]
            public string? MachineGuestOperatingSystem { get; set; }

            [JsonPropertyName("MachineInterfaceDisplayName")]
            public string? MachineInterfaceDisplayName { get; set; }

            [JsonPropertyName("MachineInterfaceType")]
            public string? MachineInterfaceType { get; set; }

            [JsonPropertyName("MachineReservationName")]
            public string? MachineReservationName { get; set; }

            [JsonPropertyName("PowerOff")]
            public string? PowerOff { get; set; }

            [JsonPropertyName("Reboot")]
            public string? Reboot { get; set; }

            [JsonPropertyName("Reconfigure")]
            public string? Reconfigure { get; set; }

            [JsonPropertyName("Relocate")]
            public string? Relocate { get; set; }

            [JsonPropertyName("Reprovision")]
            public string? Reprovision { get; set; }

            [JsonPropertyName("Reset")]
            public string? Reset { get; set; }

            [JsonPropertyName("Shutdown")]
            public string? Shutdown { get; set; }

            [JsonPropertyName("Suspend")]
            public string? Suspend { get; set; }

            [JsonPropertyName("Unregister")]
            public string? Unregister { get; set; }

            [JsonPropertyName("VMware.VirtualCenter.Folder")]
            public string? VMwareVirtualCenterFolder { get; set; }

            [JsonPropertyName("VirtualMachine.Admin.AddOwnerToAdmins")]
            public string? VirtualMachineAdminAddOwnerToAdmins { get; set; }

            [JsonPropertyName("VirtualMachine.Admin.AgentID")]
            public string? VirtualMachineAdminAgentID { get; set; }

            [JsonPropertyName("VirtualMachine.Admin.ThinProvision")]
            public string? VirtualMachineAdminThinProvision { get; set; }

            [JsonPropertyName("VirtualMachine.Admin.TotalDiskUsage")]
            public string? VirtualMachineAdminTotalDiskUsage { get; set; }

            [JsonPropertyName("VirtualMachine.Admin.UUID")]
            public string? VirtualMachineAdminUUID { get; set; }

            [JsonPropertyName("VirtualMachine.Admin.UseGuestAgent")]
            public string? VirtualMachineAdminUseGuestAgent { get; set; }

            [JsonPropertyName("VirtualMachine.CPU.Count")]
            public string? VirtualMachineCPUCount { get; set; }

            [JsonPropertyName("VirtualMachine.Cafe.Blueprint.Component.Cluster.Index")]
            public string? VirtualMachineCafeBlueprintComponentClusterIndex { get; set; }

            [JsonPropertyName("VirtualMachine.Cafe.Blueprint.Component.Id")]
            public string? VirtualMachineCafeBlueprintComponentId { get; set; }

            [JsonPropertyName("VirtualMachine.Cafe.Blueprint.Component.TypeId")]
            public string? VirtualMachineCafeBlueprintComponentTypeId { get; set; }

            [JsonPropertyName("VirtualMachine.Cafe.Blueprint.Id")]
            public string? VirtualMachineCafeBlueprintId { get; set; }

            [JsonPropertyName("VirtualMachine.Cafe.Blueprint.Name")]
            public string? VirtualMachineCafeBlueprintName { get; set; }

            [JsonPropertyName("VirtualMachine.Customize.WaitComplete")]
            public string? VirtualMachineCustomizeWaitComplete { get; set; }

            [JsonPropertyName("VirtualMachine.Disk0.IsClone")]
            public string? VirtualMachineDisk0IsClone { get; set; }

            [JsonPropertyName("VirtualMachine.Disk0.Storage.Cluster.ExternalReferenceId")]
            public string? VirtualMachineDisk0StorageClusterExternalReferenceId { get; set; }

            [JsonPropertyName("VirtualMachine.Disk0.Storage.Cluster.Name")]
            public string? VirtualMachineDisk0StorageClusterName { get; set; }

            [JsonPropertyName("VirtualMachine.Disk1.IsClone")]
            public string? VirtualMachineDisk1IsClone { get; set; }

            [JsonPropertyName("VirtualMachine.Disk1.Storage.Cluster.ExternalReferenceId")]
            public string? VirtualMachineDisk1StorageClusterExternalReferenceId { get; set; }

            [JsonPropertyName("VirtualMachine.Disk1.Storage.Cluster.Name")]
            public string? VirtualMachineDisk1StorageClusterName { get; set; }

            [JsonPropertyName("VirtualMachine.Memory.Size")]
            public string? VirtualMachineMemorySize { get; set; }

            [JsonPropertyName("VirtualMachine.Network0.DnsSearchSuffixes")]
            public string? VirtualMachineNetwork0DnsSearchSuffixes { get; set; }

            [JsonPropertyName("VirtualMachine.Network0.DnsSuffix")]
            public string? VirtualMachineNetwork0DnsSuffix { get; set; }

            [JsonPropertyName("VirtualMachine.Network0.Gateway")]
            public string? VirtualMachineNetwork0Gateway { get; set; }

            [JsonPropertyName("VirtualMachine.Network0.PrimaryDns")]
            public string? VirtualMachineNetwork0PrimaryDns { get; set; }

            [JsonPropertyName("VirtualMachine.Network0.SecondaryDns")]
            public string? VirtualMachineNetwork0SecondaryDns { get; set; }

            [JsonPropertyName("VirtualMachine.Network0.SubnetMask")]
            public string? VirtualMachineNetwork0SubnetMask { get; set; }

            [JsonPropertyName("VirtualMachine.Storage.Cluster.Automation.Behavior")]
            public string? VirtualMachineStorageClusterAutomationBehavior { get; set; }

            [JsonPropertyName("VirtualMachine.Storage.Cluster.Automation.Enabled")]
            public string? VirtualMachineStorageClusterAutomationEnabled { get; set; }

            [JsonPropertyName("VirtualMachine.Storage.Cluster.ExternalReferenceId")]
            public string? VirtualMachineStorageClusterExternalReferenceId { get; set; }

            [JsonPropertyName("VirtualMachine.Storage.Cluster.Name")]
            public string? VirtualMachineStorageClusterName { get; set; }

            [JsonPropertyName("VirtualMachine.Storage.Name")]
            public string? VirtualMachineStorageName { get; set; }

            [JsonPropertyName("Vrm.ProxyAgent.Uri")]
            public string? VrmProxyAgentUri { get; set; }

            [JsonPropertyName("_number_of_instances")]
            public string? NumberOfInstances { get; set; }

            [JsonPropertyName("cpu")]
            public string? Cpu { get; set; }

            [JsonPropertyName("dcLocation")]
            public string? DcLocation { get; set; }

            [JsonPropertyName("endpointExternalReferenceId")]
            public string? EndpointExternalReferenceId { get; set; }

            [JsonPropertyName("ip_address")]
            public string? IpAddress { get; set; }

            [JsonPropertyName("machineId")]
            public string? MachineId { get; set; }

            [JsonPropertyName("memory")]
            public string? Memory { get; set; }

            [JsonPropertyName("name")]
            public string? Name { get; set; }

            [JsonPropertyName("serverApp")]
            public string? ServerApp { get; set; }

            [JsonPropertyName("status")]
            public string? Status { get; set; }

            [JsonPropertyName("storage")]
            public string? Storage { get; set; }

            [JsonPropertyName("trace_id")]
            public string? TraceId { get; set; }

            [JsonPropertyName("type")]
            public string? Type { get; set; }
        }

        public class Resource
        {
            [JsonPropertyName("mode")]
            public string? Mode { get; set; }

            [JsonPropertyName("type")]
            public string? Type { get; set; }

            [JsonPropertyName("name")]
            public string? Name { get; set; }

            [JsonPropertyName("provider")]
            public string? Provider { get; set; }

            [JsonPropertyName("instances")]
            public List<Instance>? Instances { get; set; }
        }

        public class ResourceConfiguration
        {
            [JsonPropertyName("cluster")]
            public int Cluster { get; set; }

            [JsonPropertyName("component_name")]
            public string? ComponentName { get; set; }

            [JsonPropertyName("configuration")]
            public Configuration? Configuration { get; set; }

            [JsonPropertyName("instances")]
            public List<Instance>? Instances { get; set; }

            [JsonPropertyName("parent_resource_id")]
            public string? ParentResourceId { get; set; }

            [JsonPropertyName("request_id")]
            public string? RequestId { get; set; }
        }


        public class Attributes
        {
            [JsonPropertyName("businessgroup_id")]
            public string? BusinessGroupId { get; set; }

            [JsonPropertyName("businessgroup_name")]
            public string? BusinessGroupName { get; set; }

            [JsonPropertyName("catalog_item_id")]
            public string? CatalogItemId { get; set; }

            [JsonPropertyName("catalog_item_name")]
            public string? CatalogItemName { get; set; }

            [JsonPropertyName("created_date")]
            public DateTime CreatedDate { get; set; }

            [JsonPropertyName("deployment_configuration")]
            public object? DeploymentConfiguration { get; set; }

            [JsonPropertyName("deployment_destroy")]
            public bool DeploymentDestroy { get; set; }

            [JsonPropertyName("deployment_id")]
            public string? DeploymentId { get; set; }

            [JsonPropertyName("description")]
            public string? Description { get; set; }

            [JsonPropertyName("expiry_date")]
            public DateTime ExpiryDate { get; set; }

            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("lease_days")]
            public int LeaseDays { get; set; }

            [JsonPropertyName("name")]
            public string? Name { get; set; }

            [JsonPropertyName("owners")]
            public List<Owner>? Owners { get; set; }

            [JsonPropertyName("reasons")]
            public object? Reasons { get; set; }

            [JsonPropertyName("request_status")]
            public string? RequestStatus { get; set; }

            [JsonPropertyName("resource_configuration")]
            public List<ResourceConfiguration>? ResourceConfiguration { get; set; }

            [JsonPropertyName("wait_timeout")]
            public int WaitTimeout { get; set; }
        }



        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("terraform_version")]
        public string? TerraformVersion { get; set; }

        [JsonPropertyName("serial")]
        public int Serial { get; set; }

        [JsonPropertyName("lineage")]
        public string? Lineage { get; set; }

        [JsonPropertyName("resources")]
        public List<Resource>? Resources { get; set; }

        [JsonPropertyName("check_results")]
        public object? CheckResults { get; set; }
    }
}
