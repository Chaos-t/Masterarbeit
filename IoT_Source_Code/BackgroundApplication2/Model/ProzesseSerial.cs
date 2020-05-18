using System.Collections.Generic;
using System.Runtime.Serialization;


namespace BackgroundApplication2.Model
{

    [DataContract]
    public sealed class Version
    {
        [DataMember]
        public int Build { get; set; }
        [DataMember]
        public int Major { get; set; }
        [DataMember]
        public int Minor { get; set; }
        [DataMember]
        public int Revision { get; set; }
    }
    [DataContract]
    public sealed class Process
    {
        [DataMember]
        public double CPUUsage { get; set; }
        [DataMember]
        public string ImageName { get; set; }
        [DataMember]
        public int PageFileUsage { get; set; }
        [DataMember]
        public int PrivateWorkingSet { get; set; }
        [DataMember]
        public int ProcessId { get; set; }
        [DataMember]
        public int SessionId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public int VirtualSize { get; set; }
        [DataMember]
        public int WorkingSetSize { get; set; }
        [DataMember]
        public int? TotalCommit { get; set; }
        [DataMember]
        public string AppName { get; set; }
        [DataMember]
        public bool? IsRunning { get; set; }
        [DataMember]
        public string PackageFullName { get; set; }
        [DataMember]
        public string Publisher { get; set; }
        [DataMember]
        public Version Version { get; set; }
    }
    [DataContract]
    public sealed class RootObject
    {
        [DataMember]
        public IReadOnlyList<Process> Processes { get; set; }
    }
}
