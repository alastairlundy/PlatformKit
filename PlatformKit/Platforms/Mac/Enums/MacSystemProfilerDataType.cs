/*
        MIT License
       
       Copyright (c) 2020-2025 Alastair Lundy
       
       Permission is hereby granted, free of charge, to any person obtaining a copy
       of this software and associated documentation files (the "Software"), to deal
       in the Software without restriction, including without limitation the rights
       to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
       copies of the Software, and to permit persons to whom the Software is
       furnished to do so, subject to the following conditions:
       
       The above copyright notice and this permission notice shall be included in all
       copies or substantial portions of the Software.
       
       THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
       IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
       FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
       AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
       LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
       OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
       SOFTWARE.
   */

namespace PlatformKit.Mac{
    /// <summary>
    /// An enum of all possible Mac SystemProfiler DataTypes
    /// </summary>
    public enum MacSystemProfilerDataType{
        // ReSharper disable once InconsistentNaming
        ParallelATADataType,
        UniversalAccessDataType,
        ApplicationsDataType,
        AudioDataType,
        BluetoothDataType,
        CameraDataType,
        CardReaderDataType,
        ComponentDataType,
        DeveloperToolsDataType,
        DiagnosticsDataType,
        DisabledSoftwareDataType,
        DiscBurningDataType,
        EthernetDataType,
        ExtensionsDataType,
        FibreChannelDataType,
        FireWireDataType,
        FirewallDataType,
        FontsDataType,
        FrameworksDataType,
        DilaysDataType,
        HardwareDataType,
        HardwareRaidDataType,
        InstallHistoryDataType,
        NetworkLocationDataType,
        LogsDataType,
        ManagedClientDataType,
        MemoryDataType,
        // ReSharper disable once InconsistentNaming
        NVMeDataType,
        NetworkDataType,
        // ReSharper disable once InconsistentNaming
        PCIDataType,
        // ReSharper disable once InconsistentNaming
        ParallelSCSIDataType,
        PowerDataType,
        PrefPaneDataType,
        PrintersSoftwareDataType,
        PrintersDataType,
        ConfigurationProfileDataType,
        // ReSharper disable once InconsistentNaming
        SASDataType,
        // ReSharper disable once InconsistentNaming
        SerialATADataType,
        // ReSharper disable once InconsistentNaming
        IDataType,
        SoftwareDataType,
        StartupItemDataType,
        StorageDataType,
        SyncServicesDataType,
        ThunderboltDataType,
        // ReSharper disable once InconsistentNaming
        USBDataType,
        NetworkVolumeDataType,
        // ReSharper disable once InconsistentNaming
        WWANDataType,
        AirPortDataType
    }
}