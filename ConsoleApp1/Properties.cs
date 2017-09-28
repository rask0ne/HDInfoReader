using System;
using System.Management;

namespace HDSpecs
{
    class Properties
    {
        private String physicalName;
        private String diskName;
        private String diskModel;
        private String diskInterface;
        private bool mediaLoaded;
        private String mediaType;
        private UInt32 mediaSignature;
        private String mediaStatus;
        private String driveName;
        private String driveId;
        private bool driveCompressed;
        private UInt32 driveType;
        private String fileSystem;
        private UInt64 freeSpace;
        private UInt64 totalSpace;
        private UInt32 driveMediaType;
        private String volumeName;
        private String volumeSerial;

        public int MyProperty { get; set; }

        public void runCommand()
        {
            var driveQuery = new ManagementObjectSearcher("select * from Win32_DiskDrive");
            foreach (ManagementObject d in driveQuery.Get())
            {
                var deviceId = d.Properties["DeviceId"].Value;
                //Console.WriteLine("Device");
                //Console.WriteLine(d);
                var partitionQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_DiskDriveToDiskPartition", d.Path.RelativePath);
                var partitionQuery = new ManagementObjectSearcher(partitionQueryText);
                foreach (ManagementObject p in partitionQuery.Get())
                {
                    //Console.WriteLine("Partition");
                    //Console.WriteLine(p);
                    var logicalDriveQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_LogicalDiskToPartition", p.Path.RelativePath);
                    var logicalDriveQuery = new ManagementObjectSearcher(logicalDriveQueryText);
                    foreach (ManagementObject ld in logicalDriveQuery.Get())
                    {
                        //Console.WriteLine("Logical drive");
                        //Console.WriteLine(ld);

                        this.physicalName = Convert.ToString(d.Properties["Name"].Value); // \\.\PHYSICALDRIVE2
                        this.diskName = Convert.ToString(d.Properties["Caption"].Value); // WDC WD5001AALS-xxxxxx
                        this.diskModel = Convert.ToString(d.Properties["Model"].Value); // WDC WD5001AALS-xxxxxx
                        this.diskInterface = Convert.ToString(d.Properties["InterfaceType"].Value); // IDEg
                        this.mediaLoaded = Convert.ToBoolean(d.Properties["MediaLoaded"].Value); // bool
                        this.mediaType = Convert.ToString(d.Properties["MediaType"].Value); // Fixed hard disk media
                        this.mediaSignature = Convert.ToUInt32(d.Properties["Signature"].Value); // int32
                        this.mediaStatus = Convert.ToString(d.Properties["Status"].Value); // OK

                        this.driveName = Convert.ToString(ld.Properties["Name"].Value); // C:
                        this.driveId = Convert.ToString(ld.Properties["DeviceId"].Value); // C:
                        this.driveCompressed = Convert.ToBoolean(ld.Properties["Compressed"].Value);
                        this.driveType = Convert.ToUInt32(ld.Properties["DriveType"].Value); // C: - 3
                        this.fileSystem = Convert.ToString(ld.Properties["FileSystem"].Value); // NTFS
                        this.freeSpace = Convert.ToUInt64(ld.Properties["FreeSpace"].Value); // in bytes
                        this.totalSpace = Convert.ToUInt64(ld.Properties["Size"].Value); // in bytes
                        this.driveMediaType = Convert.ToUInt32(ld.Properties["MediaType"].Value); // c: 12
                        this.volumeName = Convert.ToString(ld.Properties["VolumeName"].Value); // System
                        this.volumeSerial = Convert.ToString(ld.Properties["VolumeSerialNumber"].Value); // 12345678
                    }
                }
            }
        }

        public void printSpecs()
        {
            Console.WriteLine("PhysicalName: {0}", physicalName);
            Console.WriteLine("DiskName: {0}", diskName);
            Console.WriteLine("DiskModel: {0}", diskModel);
            Console.WriteLine("DiskInterface: {0}", diskInterface);
            Console.WriteLine("MediaLoaded: {0}", mediaLoaded);
            Console.WriteLine("MediaType: {0}", mediaType);
            Console.WriteLine("MediaSignature: {0}", mediaSignature);
            Console.WriteLine("MediaStatus: {0}", mediaStatus);

            Console.WriteLine("DriveName: {0}", driveName);
            Console.WriteLine("DriveId: {0}", driveId);
            Console.WriteLine("DriveCompressed: {0}", driveCompressed);
            Console.WriteLine("DriveType: {0}", driveType);
            Console.WriteLine("FileSystem: {0}", fileSystem);
            Console.WriteLine("FreeSpace: {0}", freeSpace);
            Console.WriteLine("TotalSpace: {0}", totalSpace);
            Console.WriteLine("DriveMediaType: {0}", driveMediaType);
            Console.WriteLine("VolumeName: {0}", volumeName);
            Console.WriteLine("VolumeSerial: {0}", volumeSerial);

            Console.WriteLine(new string('-', 79));

            Console.ReadLine();
        }
    }
}
