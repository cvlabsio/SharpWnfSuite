﻿using System.Runtime.InteropServices;

namespace SharpWnfScan.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct GUID
    {
        public uint Data1;
        public ushort Data2;
        public ushort Data3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Data4;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct LIST_ENTRY32
    {
        public int Flink;
        public int Blink;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct LIST_ENTRY64
    {
        public long Flink;
        public long Blink;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct LUID
    {
        public uint LowPart;
        public uint HighPart;

        public LUID(uint _lowPart, uint _highPart)
        {
            LowPart = _lowPart;
            HighPart = _highPart;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    internal struct LUID_AND_ATTRIBUTES
    {
        public LUID Luid;
        public uint Attributes;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RTL_BALANCED_NODE32
    {
        public int /* PRTL_BALANCED_NODE */ Left;
        public int /* PRTL_BALANCED_NODE */ Right;
        public int ParentValue;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RTL_BALANCED_NODE64
    {
        public long /* PRTL_BALANCED_NODE */ Left;
        public long /* PRTL_BALANCED_NODE */ Right;
        public long ParentValue;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RTL_RB_TREE32
    {
        public int /* PRTL_BALANCED_NODE */ Root;
        public int /* PRTL_BALANCED_NODE */ Min;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RTL_RB_TREE64
    {
        public long /* PRTL_BALANCED_NODE */ Root;
        public long /* PRTL_BALANCED_NODE */ Min;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SYMBOL_INFO
    {
        public uint SizeOfStruct;
        public uint TypeIndex;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public ulong[] Reserved;
        public uint Index;
        public uint Size;
        public ulong ModBase;
        public uint Flags;
        public ulong Value;
        public ulong Address;
        public uint Register;
        public uint Scope;
        public uint Tag;
        public uint NameLen;
        public uint MaxNameLen;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)Win32Consts.MAX_SYM_NAME)]
        public byte[] Name;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct TOKEN_PRIVILEGES
    {
        public int PrivilegeCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public LUID_AND_ATTRIBUTES[] Privileges;

        public TOKEN_PRIVILEGES(int _privilegeCount)
        {
            PrivilegeCount = _privilegeCount;
            Privileges = new LUID_AND_ATTRIBUTES[1];
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WNF_CONTEXT_HEADER
    {
        public short NodeTypeCode;
        public short NodeByteSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WNF_DELIVERY_DESCRIPTOR
    {
        public ulong SubscriptionId;
        public WNF_STATE_NAME StateName;
        public uint ChangeStamp;
        public uint StateDataSize;
        public uint EventMask;
        public WNF_TYPE_ID TypeId;
        public uint StateDataOffset;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WNF_NAME_SUBSCRIPTION32
    {
        public WNF_CONTEXT_HEADER Header;
        public ulong SubscriptionId;
        public ulong /* WNF_STATE_NAME_INTERNAL */ StateName;
        public uint /* WNF_CHANGE_STAMP */ CurrentChangeStamp;
        public LIST_ENTRY32 NamesTableEntry;
        public int /* ref WNF_TYPE_ID */ TypeId;
        public int /* SRWLOCK */ SubscriptionLock;
        public LIST_ENTRY32 SubscriptionsListHead;
        public uint NormalDeliverySubscriptions;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public uint[] NotificationTypeCount;
        public int /* ref WNF_DELIVERY_DESCRIPTOR */ RetryDescriptor;
        public uint DeliveryState;
        public ulong ReliableRetryTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WNF_NAME_SUBSCRIPTION32_WIN11
    {
        public WNF_CONTEXT_HEADER Header;
        public ulong SubscriptionId;
        public ulong /* WNF_STATE_NAME_INTERNAL */ StateName;
        public uint /* WNF_CHANGE_STAMP */ CurrentChangeStamp;
        public RTL_BALANCED_NODE32 NamesTableEntry;
        public int /* ref WNF_TYPE_ID */ TypeId;
        public int /* SRWLOCK */ SubscriptionLock;
        public LIST_ENTRY32 SubscriptionsListHead;
        public uint NormalDeliverySubscriptions;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public uint[] NotificationTypeCount;
        public int /* ref WNF_DELIVERY_DESCRIPTOR */ RetryDescriptor;
        public uint DeliveryState;
        public ulong ReliableRetryTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WNF_NAME_SUBSCRIPTION64
    {
        public WNF_CONTEXT_HEADER Header;
        public ulong SubscriptionId;
        public ulong /* WNF_STATE_NAME_INTERNAL */ StateName;
        public uint /* WNF_CHANGE_STAMP */ CurrentChangeStamp;
        public LIST_ENTRY64 NamesTableEntry;
        public long /* ref WNF_TYPE_ID */ TypeId;
        public long /* SRWLOCK */ SubscriptionLock;
        public LIST_ENTRY64 SubscriptionsListHead;
        public uint NormalDeliverySubscriptions;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public uint[] NotificationTypeCount;
        public long /* ref WNF_DELIVERY_DESCRIPTOR */ RetryDescriptor;
        public uint DeliveryState;
        public ulong ReliableRetryTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WNF_NAME_SUBSCRIPTION64_WIN11
    {
        public WNF_CONTEXT_HEADER Header;
        public ulong SubscriptionId;
        public ulong /* WNF_STATE_NAME_INTERNAL */ StateName;
        public uint /* WNF_CHANGE_STAMP */ CurrentChangeStamp;
        public RTL_BALANCED_NODE64 NamesTableEntry;
        public long /* ref WNF_TYPE_ID */ TypeId;
        public long /* SRWLOCK */ SubscriptionLock;
        public LIST_ENTRY64 SubscriptionsListHead;
        public uint NormalDeliverySubscriptions;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public uint[] NotificationTypeCount;
        public long /* ref WNF_DELIVERY_DESCRIPTOR */ RetryDescriptor;
        public uint DeliveryState;
        public ulong ReliableRetryTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WNF_SERIALIZATION_GROUP32
    {
        public WNF_CONTEXT_HEADER Header;
        public uint GroupId;
        public LIST_ENTRY32 SerializationGroupList;
        public ulong SerializationGroupValue;
        public ulong SerializationGroupMemberCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WNF_SERIALIZATION_GROUP64
    {
        public WNF_CONTEXT_HEADER Header;
        public uint GroupId;
        public LIST_ENTRY64 SerializationGroupList;
        public ulong SerializationGroupValue;
        public ulong SerializationGroupMemberCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WNF_STATE_NAME
    {
        public ulong Data;

        public WNF_STATE_NAME(
            uint Version,
            WNF_STATE_NAME_LIFETIME NameLifeTime,
            WNF_DATA_SCOPE DataScope,
            uint PermanentData,
            uint SequenceNumber,
            uint OwnerTag)
        {
            Data = (ulong)Version & 0xF;
            Data |= ((ulong)NameLifeTime & 0x3) << 4;
            Data |= ((ulong)DataScope & 0xF) << 6;
            Data |= ((ulong)PermanentData & 0x1) << 10;
            Data |= ((ulong)SequenceNumber & 0x1FFFFF) << 11;
            Data |= ((ulong)OwnerTag & 0xFFFFFFFF) << 32;
            Data ^= 0x41C64E6DA3BC0074UL;
        }

        public uint GetVersion()
        {
            return (uint)((Data ^ 0x41C64E6DA3BC0074UL) & 0xF);
        }

        public WNF_STATE_NAME_LIFETIME GetNameLifeTime()
        {
            return (WNF_STATE_NAME_LIFETIME)(((Data ^ 0x41C64E6DA3BC0074UL) >> 4) & 0x3);
        }

        public WNF_DATA_SCOPE GetDataScope()
        {
            return (WNF_DATA_SCOPE)((((uint)Data ^ 0x41C64E6DA3BC0074UL) >> 6) & 0xF);
        }

        public uint GetPermanentData()
        {
            return (uint)(((Data ^ 0x41C64E6DA3BC0074UL) >> 10) & 0x1);
        }

        public uint GetSequenceNumber()
        {
            return (uint)(((Data ^ 0x41C64E6DA3BC0074UL) >> 11) & 0x1FFFFF);
        }

        public uint GetOwnerTag()
        {
            return (uint)(((Data ^ 0x41C64E6DA3BC0074UL) >> 32) & 0xFFFFFFFF);
        }

        public void SetVersion(uint version)
        {
            Data ^= 0x41C64E6DA3BC0074UL;
            Data &= 0xFFFFFFFFFFFFFFF0UL;
            Data |= (version & 0xF);
            Data ^= 0x41C64E6DA3BC0074UL;
        }

        public void SetNameLifeTime(WNF_STATE_NAME_LIFETIME nameLifeTime)
        {
            Data ^= 0x41C64E6DA3BC0074UL;
            Data &= 0xFFFFFFFFFFFFFFCFUL;
            Data |= (((uint)nameLifeTime & 0x3) << 4);
            Data ^= 0x41C64E6DA3BC0074UL;
        }

        public void SetDataScope(uint dataScope)
        {
            Data ^= 0x41C64E6DA3BC0074UL;
            Data &= 0xFFFFFFFFFFFFFC3FUL;
            Data |= ((dataScope & 0xF) << 6);
            Data ^= 0x41C64E6DA3BC0074UL;
        }

        public void SetPermanentData(uint parmanentData)
        {
            Data ^= 0x41C64E6DA3BC0074UL;
            Data &= 0xFFFFFFFFFFFFFBFFUL;
            Data |= ((parmanentData & 0x1) << 10);
            Data ^= 0x41C64E6DA3BC0074UL;
        }

        public void SetSequenceNumber(uint sequenceNumber)
        {
            Data ^= 0x41C64E6DA3BC0074UL;
            Data &= 0xFFFFFFFF000007FFUL;
            Data |= ((sequenceNumber & 0x1FFFFF) << 11);
            Data ^= 0x41C64E6DA3BC0074UL;
        }

        public void SetOwnerTag(uint ownerTag)
        {
            Data ^= 0x41C64E6DA3BC0074UL;
            Data &= 0x00000000FFFFFFFFUL;
            Data |= (ownerTag << 32);
            Data ^= 0x41C64E6DA3BC0074UL;
        }

        public bool IsValid()
        {
            var nameLifeTime = (uint)GetNameLifeTime();
            var dataScope = (uint)GetDataScope();

            return ((nameLifeTime < (uint)WNF_STATE_NAME_LIFETIME.Max) && (dataScope < (uint)WNF_DATA_SCOPE.Max));
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WNF_SUBSCRIPTION_TABLE32
    {
        public WNF_CONTEXT_HEADER Header;
        public int /* SRWLOCK */ NamesTableLock;
        public LIST_ENTRY32 NamesTableEntry;
        public LIST_ENTRY32 SerializationGroupListHead;
        public int /* SRWLOCK */ SerializationGroupLock;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public int[] Unknown1;
        public int SubscribedEventSet;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public int[] Unknown2;
        public int Timer;
        public ulong TimerDueTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WNF_SUBSCRIPTION_TABLE32_WIN11
    {
        public WNF_CONTEXT_HEADER Header;
        public int /* SRWLOCK */ NamesTableLock;
        public RTL_RB_TREE32 NamesTableEntry;
        public LIST_ENTRY32 SerializationGroupListHead;
        public int /* SRWLOCK */ SerializationGroupLock;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public int[] Unknown1;
        public int SubscribedEventSet;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public int[] Unknown2;
        public int Timer;
        public ulong TimerDueTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WNF_SUBSCRIPTION_TABLE64
    {
        public WNF_CONTEXT_HEADER Header;
        public long /* SRWLOCK */ NamesTableLock;
        public LIST_ENTRY64 NamesTableEntry;
        public LIST_ENTRY64 SerializationGroupListHead;
        public long /* SRWLOCK */ SerializationGroupLock;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public int[] Unknown1;
        public int SubscribedEventSet;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public int[] Unknown2;
        public long Timer;
        public ulong TimerDueTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WNF_SUBSCRIPTION_TABLE64_WIN11
    {
        public WNF_CONTEXT_HEADER Header;
        public long /* SRWLOCK */ NamesTableLock;
        public RTL_RB_TREE64 NamesTableEntry;
        public LIST_ENTRY64 SerializationGroupListHead;
        public long /* SRWLOCK */ SerializationGroupLock;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public int[] Unknown1;
        public int SubscribedEventSet;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public int[] Unknown2;
        public long Timer;
        public ulong TimerDueTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WNF_USER_SUBSCRIPTION32
    {
        public WNF_CONTEXT_HEADER Header;
        public LIST_ENTRY32 SubscriptionsListEntry;
        public int /* ref WNF_NAME_SUBSCRIPTION */ NameSubscription;
        public int /* ref WNF_USER_CALLBACK */ Callback;
        public int CallbackContext;
        public ulong SubProcessTag;
        public uint CurrentChangeStamp;
        public uint DeliveryOptions;
        public uint SubscribedEventSet;
        public int /* WNF_SERIALIZATION_GROUP */ SerializationGroup;
        public uint UserSubscriptionCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public ulong[] Unknown;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WNF_USER_SUBSCRIPTION64
    {
        public WNF_CONTEXT_HEADER Header;
        public LIST_ENTRY64 SubscriptionsListEntry;
        public long /* ref WNF_NAME_SUBSCRIPTION */ NameSubscription;
        public long /* ref WNF_USER_CALLBACK */ Callback;
        public long CallbackContext;
        public ulong SubProcessTag;
        public uint CurrentChangeStamp;
        public uint DeliveryOptions;
        public uint SubscribedEventSet;
        public long /* WNF_SERIALIZATION_GROUP */ SerializationGroup;
        public uint UserSubscriptionCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public ulong[] Unknown;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WNF_TYPE_ID
    {
        public GUID TypeId;
    }
}
