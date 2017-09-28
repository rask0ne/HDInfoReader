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
        private String serialNumber;
        private UInt64 size;
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
                var partitionQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_DiskDriveToDiskPartition", d.Path.RelativePath);
                var partitionQuery = new ManagementObjectSearcher(partitionQueryText);
                foreach (ManagementObject p in partitionQuery.Get())
                {
                    physicalName = Convert.ToString(d.Properties["Name"].Value);
                    diskName = Convert.ToString(d.Properties["Caption"].Value);
                    diskModel = Convert.ToString(d.Properties["Model"].Value);
                    diskInterface = Convert.ToString(d.Properties["InterfaceType"].Value);
                    serialNumber = Convert.ToString(d.Properties["SerialNumber"].Value); // bool
                    size = Convert.ToUInt64(d.Properties["Size"].Value); // Fixed hard disk media
                    mediaSignature = Convert.ToUInt32(d.Properties["Signature"].Value); // int32
                    mediaStatus = Convert.ToString(d.Properties["Status"].Value); // OK

                    var logicalDriveQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_LogicalDiskToPartition", p.Path.RelativePath);
                    var logicalDriveQuery = new ManagementObjectSearcher(logicalDriveQueryText);
                    foreach (ManagementObject ld in logicalDriveQuery.Get())
                    {
                        driveName = Convert.ToString(ld.Properties["Name"].Value); 
                        driveId = Convert.ToString(ld.Properties["DeviceId"].Value); 
                        driveCompressed = Convert.ToBoolean(ld.Properties["Compressed"].Value);
                        driveType = Convert.ToUInt32(ld.Properties["DriveType"].Value); 
                        fileSystem = Convert.ToString(ld.Properties["FileSystem"].Value); // NTFS
                        freeSpace = Convert.ToUInt64(ld.Properties["FreeSpace"].Value); // in bytes
                        totalSpace = Convert.ToUInt64(ld.Properties["Size"].Value); // in bytes
                        driveMediaType = Convert.ToUInt32(ld.Properties["MediaType"].Value); // c: 12
                        volumeName = Convert.ToString(ld.Properties["VolumeName"].Value); // System
                        volumeSerial = Convert.ToString(ld.Properties["VolumeSerialNumber"].Value);

                        printLocalDiskSpecs();
                    }
                }
                printMainSpecs();
                Console.ReadLine();
            }
        }

        public void printMainSpecs()
        {
            Console.WriteLine("PhysicalName: {0}", physicalName);
            Console.WriteLine("DiskName: {0}", diskName);
            Console.WriteLine("DiskModel: {0}", diskModel);
            Console.WriteLine("DiskInterface: {0}", diskInterface);
            Console.WriteLine("SerialNumber: {0}", serialNumber);
            Console.WriteLine("Size: {0}", size);
            Console.WriteLine("MediaSignature: {0}", mediaSignature);
            Console.WriteLine("MediaStatus: {0}", mediaStatus);
        }

        public void printLocalDiskSpecs ()
        {
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
        }
    }
}
