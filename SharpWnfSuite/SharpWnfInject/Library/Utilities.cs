﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Principal;
using SharpWnfInject.Interop;

namespace SharpWnfInject.Library
{
    using NTSTATUS = Int32;

    internal class Utilities
    {
        public static bool EnableDebugPrivilege()
        {
            NTSTATUS ntstatus;
            IntPtr pTokenPrivilege = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TOKEN_PRIVILEGES)));
            var tp = new TOKEN_PRIVILEGES(1);
            tp.Privileges[0].Luid.LowPart = 0x00000014;
            tp.Privileges[0].Luid.HighPart = 0;
            tp.Privileges[0].Attributes = (uint)PrivilegeAttributeFlags.ENABLED;
            Marshal.StructureToPtr(tp, pTokenPrivilege, true);

            ntstatus = NativeMethods.NtAdjustPrivilegesToken(
                WindowsIdentity.GetCurrent().Token,
                BOOLEAN.FALSE,
                pTokenPrivilege,
                0u,
                IntPtr.Zero,
                IntPtr.Zero);
            Marshal.FreeHGlobal(pTokenPrivilege);

            return (ntstatus == Win32Consts.STATUS_SUCCESS);
        }


        public static Dictionary<ulong, IntPtr> GetNameSubscriptions(
            PeProcess proc,
            IntPtr pSubscriptionTable)
        {
            IntPtr buffer;
            IntPtr pFirstNameSubscription;
            IntPtr pNameSubscription;
            IntPtr pCurrentNameSubscription;
            uint nSizeSubscriptionTable;
            uint nSizeNameSubscription;
            uint nNameTableEntryOffset;
            var results = new Dictionary<ulong, IntPtr>();

            if (proc.GetArchitecture() == "x64")
            {
                nSizeSubscriptionTable = (uint)Marshal.SizeOf(typeof(WNF_SUBSCRIPTION_TABLE64));
                WNF_NAME_SUBSCRIPTION64 nameSubscription;
                nSizeNameSubscription = (uint)Marshal.SizeOf(
                    typeof(WNF_NAME_SUBSCRIPTION64));
                nNameTableEntryOffset = (uint)Marshal.OffsetOf(
                    typeof(WNF_NAME_SUBSCRIPTION64),
                    "NamesTableEntry").ToInt32();
                buffer = proc.ReadMemory(pSubscriptionTable, nSizeSubscriptionTable);

                if (buffer == IntPtr.Zero)
                {
                    Console.WriteLine("[-] Failed to read WNF_SUBSCRIPTION_TABLE.");
                    return results;
                }

                var subscriptionTable = (WNF_SUBSCRIPTION_TABLE64)Marshal.PtrToStructure(
                    buffer,
                    typeof(WNF_SUBSCRIPTION_TABLE64));
                NativeMethods.LocalFree(buffer);

                pFirstNameSubscription = new IntPtr(subscriptionTable.NamesTableEntry.Flink - nNameTableEntryOffset);
                pNameSubscription = pFirstNameSubscription;

                while (true)
                {
                    pCurrentNameSubscription = pNameSubscription;
                    buffer = proc.ReadMemory(pNameSubscription, nSizeNameSubscription);

                    if (buffer == IntPtr.Zero)
                        break;

                    nameSubscription = (WNF_NAME_SUBSCRIPTION64)Marshal.PtrToStructure(
                        buffer,
                        typeof(WNF_NAME_SUBSCRIPTION64));
                    NativeMethods.LocalFree(buffer);
                    pNameSubscription = new IntPtr(nameSubscription.NamesTableEntry.Flink - nNameTableEntryOffset);

                    if (pNameSubscription == pFirstNameSubscription)
                        break;

                    results.Add(nameSubscription.StateName, pCurrentNameSubscription);
                }
            }
            else if (proc.GetArchitecture() == "x86")
            {
                nSizeSubscriptionTable = (uint)Marshal.SizeOf(typeof(WNF_SUBSCRIPTION_TABLE32));
                WNF_NAME_SUBSCRIPTION32 nameSubscription;
                nSizeNameSubscription = (uint)Marshal.SizeOf(
                    typeof(WNF_NAME_SUBSCRIPTION32));
                nNameTableEntryOffset = (uint)Marshal.OffsetOf(
                    typeof(WNF_NAME_SUBSCRIPTION32),
                    "NamesTableEntry").ToInt32();
                buffer = proc.ReadMemory(pSubscriptionTable, nSizeSubscriptionTable);

                if (buffer == IntPtr.Zero)
                {
                    Console.WriteLine("[-] Failed to read WNF_SUBSCRIPTION_TABLE.");

                    return results;
                }

                var subscriptionTable = (WNF_SUBSCRIPTION_TABLE32)Marshal.PtrToStructure(
                    buffer,
                    typeof(WNF_SUBSCRIPTION_TABLE32));
                NativeMethods.LocalFree(buffer);

                pFirstNameSubscription = new IntPtr(subscriptionTable.NamesTableEntry.Flink - nNameTableEntryOffset);
                pNameSubscription = pFirstNameSubscription;

                while (true)
                {
                    pCurrentNameSubscription = pNameSubscription;
                    buffer = proc.ReadMemory(pNameSubscription, nSizeNameSubscription);

                    if (buffer == IntPtr.Zero)
                        break;

                    nameSubscription = (WNF_NAME_SUBSCRIPTION32)Marshal.PtrToStructure(
                        buffer,
                        typeof(WNF_NAME_SUBSCRIPTION32));
                    NativeMethods.LocalFree(buffer);
                    pNameSubscription = new IntPtr(nameSubscription.NamesTableEntry.Flink - nNameTableEntryOffset);

                    if (pNameSubscription == pFirstNameSubscription)
                        break;

                    results.Add(nameSubscription.StateName, pCurrentNameSubscription);
                }
            }
            else
            {
                Console.WriteLine("[-] Unsupported architecture.");
            }

            return results;
        }


        public static Dictionary<ulong, IntPtr> GetNameSubscriptionsWin11(
            PeProcess proc,
            IntPtr pSubscriptionTable)
        {
            IntPtr buffer;
            IntPtr pNameSubscription;
            uint nSizeSubscriptionTable;
            uint nNameTableEntryOffset;
            var results = new Dictionary<ulong, IntPtr>();

            if (proc.GetArchitecture() == "x64")
            {
                nSizeSubscriptionTable = (uint)Marshal.SizeOf(typeof(WNF_SUBSCRIPTION_TABLE64_WIN11));
                nNameTableEntryOffset = (uint)Marshal.OffsetOf(
                    typeof(WNF_NAME_SUBSCRIPTION64_WIN11),
                    "NamesTableEntry").ToInt32();
                buffer = proc.ReadMemory(pSubscriptionTable, nSizeSubscriptionTable);

                if (buffer == IntPtr.Zero)
                {
                    Console.WriteLine("[-] Failed to read WNF_SUBSCRIPTION_TABLE.");

                    return results;
                }

                var subscriptionTable = (WNF_SUBSCRIPTION_TABLE64_WIN11)Marshal.PtrToStructure(
                    buffer,
                    typeof(WNF_SUBSCRIPTION_TABLE64_WIN11));
                NativeMethods.LocalFree(buffer);

                pNameSubscription = new IntPtr(subscriptionTable.NamesTableEntry.Root - nNameTableEntryOffset);
                ListWin11NameSubscriptions(proc, pNameSubscription, ref results);
            }
            else if (proc.GetArchitecture() == "x86")
            {
                nSizeSubscriptionTable = (uint)Marshal.SizeOf(typeof(WNF_SUBSCRIPTION_TABLE32_WIN11));
                nNameTableEntryOffset = (uint)Marshal.OffsetOf(
                    typeof(WNF_NAME_SUBSCRIPTION32_WIN11),
                    "NamesTableEntry").ToInt32();
                buffer = proc.ReadMemory(pSubscriptionTable, nSizeSubscriptionTable);

                if (buffer == IntPtr.Zero)
                {
                    Console.WriteLine("[-] Failed to read WNF_SUBSCRIPTION_TABLE.");

                    return results;
                }

                var subscriptionTable = (WNF_SUBSCRIPTION_TABLE32_WIN11)Marshal.PtrToStructure(
                    buffer,
                    typeof(WNF_SUBSCRIPTION_TABLE32_WIN11));
                NativeMethods.LocalFree(buffer);

                pNameSubscription = new IntPtr(subscriptionTable.NamesTableEntry.Root - nNameTableEntryOffset);
                ListWin11NameSubscriptions(proc, pNameSubscription, ref results);
            }
            else
            {
                Console.WriteLine("[-] Unsupported architecture.");
            }

            return results;
        }


        public static IntPtr GetSubscriptionTable(PeProcess proc)
        {
            if (proc.GetCurrentModuleName() != "ntdll.dll")
                proc.SetBaseModule("ntdll.dll");

            IntPtr pDataSection = proc.GetSectionAddress(".data");
            uint nSizeSubscriptionTable;
            uint nSizeDataSection = proc.GetSectionVirtualSize(".data");
            uint count;
            uint nSizePointer;
            WNF_CONTEXT_HEADER tableHeader;
            IntPtr pSubscriptionTable;
            IntPtr buffer;

            if (proc.GetArchitecture() == "x64")
            {
                nSizeSubscriptionTable = (uint)Marshal.SizeOf(
                    typeof(WNF_SUBSCRIPTION_TABLE64));
                nSizePointer = 8u;
                count = nSizeDataSection / nSizePointer;
            }
            else if (proc.GetArchitecture() == "x86")
            {
                nSizeSubscriptionTable = (uint)Marshal.SizeOf(
                    typeof(WNF_SUBSCRIPTION_TABLE32));
                nSizePointer = 4u;
                count = nSizeDataSection / nSizePointer;
            }
            else
            {
                Console.WriteLine("[-] Unsupported architecture.");

                return IntPtr.Zero;
            }

            for (var idx = 0u; idx < count; idx++)
            {
                pSubscriptionTable = proc.ReadIntPtr(pDataSection, idx * nSizePointer);

                if (proc.IsHeapAddress(pSubscriptionTable))
                {
                    buffer = proc.ReadMemory(pSubscriptionTable, nSizeSubscriptionTable);

                    if (buffer != IntPtr.Zero)
                    {
                        tableHeader = (WNF_CONTEXT_HEADER)Marshal.PtrToStructure(
                            buffer,
                            typeof(WNF_CONTEXT_HEADER));

                        NativeMethods.LocalFree(buffer);
                    }
                    else
                    {
                        continue;
                    }

                    if ((tableHeader.NodeTypeCode == Win32Consts.WNF_NODE_SUBSCRIPTION_TABLE) &&
                        (tableHeader.NodeByteSize == nSizeSubscriptionTable))
                    {
                        return pSubscriptionTable;
                    }
                }
            }

            return IntPtr.Zero;
        }


        public static Dictionary<IntPtr, Dictionary<IntPtr, IntPtr>> GetUserSubscriptions(
            PeProcess proc,
            IntPtr pNameSubscription)
        {
            IntPtr buffer;
            IntPtr pCurrentUserSubscription;
            IntPtr pFirstUserSubscription;
            IntPtr pUserSubscription;
            uint nSizeNameSubscription;
            uint nSizeUserSubscription;
            uint nSubscriptionsListEntryOffset;
            Dictionary<IntPtr, IntPtr> callback;
            var results = new Dictionary<IntPtr, Dictionary<IntPtr, IntPtr>> ();

            if (proc.GetArchitecture() == "x64")
            {
                nSizeNameSubscription = (uint)Marshal.SizeOf(typeof(WNF_NAME_SUBSCRIPTION64));
                WNF_USER_SUBSCRIPTION64 userSubscription;
                buffer = proc.ReadMemory(pNameSubscription, nSizeNameSubscription);

                if (buffer == IntPtr.Zero)
                {
                    Console.WriteLine("[-] Failed to read WNF_NAME_SUBSCRIPTION.");

                    return results;
                }

                var nameSubscription = (WNF_NAME_SUBSCRIPTION64)Marshal.PtrToStructure(
                    buffer,
                    typeof(WNF_NAME_SUBSCRIPTION64));
                NativeMethods.LocalFree(buffer);

                nSizeUserSubscription = (uint)Marshal.SizeOf(typeof(WNF_USER_SUBSCRIPTION64));
                nSubscriptionsListEntryOffset = (uint)Marshal.OffsetOf(
                    typeof(WNF_USER_SUBSCRIPTION64),
                    "SubscriptionsListEntry").ToInt32();

                if (nameSubscription.Header.NodeTypeCode == Win32Consts.WNF_NODE_NAME_SUBSCRIPTION)
                {
                    pFirstUserSubscription = new IntPtr(nameSubscription.SubscriptionsListHead.Flink - nSubscriptionsListEntryOffset);
                    pUserSubscription = pFirstUserSubscription;

                    while (true)
                    {
                        pCurrentUserSubscription = pUserSubscription;
                        buffer = proc.ReadMemory(pUserSubscription, nSizeUserSubscription);

                        if (buffer == IntPtr.Zero)
                            break;

                        userSubscription = (WNF_USER_SUBSCRIPTION64)Marshal.PtrToStructure(
                            buffer,
                            typeof(WNF_USER_SUBSCRIPTION64));
                        NativeMethods.LocalFree(buffer);
                        pUserSubscription = new IntPtr(userSubscription.SubscriptionsListEntry.Flink - nSubscriptionsListEntryOffset);

                        if (pUserSubscription == pFirstUserSubscription)
                            break;

                        callback = new Dictionary<IntPtr, IntPtr> {
                            { new IntPtr(userSubscription.Callback), new IntPtr(userSubscription.CallbackContext) }
                        };

                        results.Add(
                            pCurrentUserSubscription,
                            callback);
                    }
                }
                else
                {
                    Console.WriteLine("[-] Failed to get valid WNF_NAME_SUBSCRIPTION.");
                }
            }
            else if (proc.GetArchitecture() == "x86")
            {
                nSizeNameSubscription = (uint)Marshal.SizeOf(typeof(WNF_NAME_SUBSCRIPTION32));
                WNF_USER_SUBSCRIPTION32 userSubscription;
                buffer = proc.ReadMemory(pNameSubscription, nSizeNameSubscription);

                if (buffer == IntPtr.Zero)
                {
                    Console.WriteLine("[-] Failed to read WNF_NAME_SUBSCRIPTION.");

                    return results;
                }

                var nameSubscription = (WNF_NAME_SUBSCRIPTION32)Marshal.PtrToStructure(
                    buffer,
                    typeof(WNF_NAME_SUBSCRIPTION32));
                NativeMethods.LocalFree(buffer);

                nSizeUserSubscription = (uint)Marshal.SizeOf(typeof(WNF_USER_SUBSCRIPTION32));
                nSubscriptionsListEntryOffset = (uint)Marshal.OffsetOf(
                    typeof(WNF_USER_SUBSCRIPTION32),
                    "SubscriptionsListEntry").ToInt32();

                if (nameSubscription.Header.NodeTypeCode == Win32Consts.WNF_NODE_NAME_SUBSCRIPTION)
                {
                    pFirstUserSubscription = new IntPtr(nameSubscription.SubscriptionsListHead.Flink - nSubscriptionsListEntryOffset);
                    pUserSubscription = pFirstUserSubscription;

                    while (true)
                    {
                        pCurrentUserSubscription = pUserSubscription;
                        buffer = proc.ReadMemory(pUserSubscription, nSizeUserSubscription);

                        if (buffer == IntPtr.Zero)
                            break;

                        userSubscription = (WNF_USER_SUBSCRIPTION32)Marshal.PtrToStructure(
                            buffer,
                            typeof(WNF_USER_SUBSCRIPTION32));
                        NativeMethods.LocalFree(buffer);
                        pUserSubscription = new IntPtr(userSubscription.SubscriptionsListEntry.Flink - nSubscriptionsListEntryOffset);

                        if (pUserSubscription == pFirstUserSubscription)
                            break;

                        callback = new Dictionary<IntPtr, IntPtr> {
                            { new IntPtr(userSubscription.Callback), new IntPtr(userSubscription.CallbackContext) }
                        };

                        results.Add(
                            pCurrentUserSubscription,
                            callback);
                    }
                }
                else
                {
                    Console.WriteLine("[-] Failed to get valid WNF_NAME_SUBSCRIPTION.");
                }
            }
            else
            {
                Console.WriteLine("[-] Unsupported architecture.");
            }

            return results;
        }


        public static Dictionary<IntPtr, Dictionary<IntPtr, IntPtr>> GetUserSubscriptionsWin11(
            PeProcess proc,
            IntPtr pNameSubscription)
        {
            IntPtr buffer;
            IntPtr pCurrentUserSubscription;
            IntPtr pFirstUserSubscription;
            IntPtr pUserSubscription;
            uint nSizeNameSubscription;
            uint nSizeUserSubscription;
            uint nSubscriptionsListEntryOffset;
            Dictionary<IntPtr, IntPtr> callback;
            var results = new Dictionary<IntPtr, Dictionary<IntPtr, IntPtr>>();

            if (proc.GetArchitecture() == "x64")
            {
                nSizeNameSubscription = (uint)Marshal.SizeOf(typeof(WNF_NAME_SUBSCRIPTION64_WIN11));
                WNF_USER_SUBSCRIPTION64 userSubscription;
                buffer = proc.ReadMemory(pNameSubscription, nSizeNameSubscription);

                if (buffer == IntPtr.Zero)
                {
                    Console.WriteLine("[-] Failed to read WNF_NAME_SUBSCRIPTION.");

                    return results;
                }

                var nameSubscription = (WNF_NAME_SUBSCRIPTION64_WIN11)Marshal.PtrToStructure(
                    buffer,
                    typeof(WNF_NAME_SUBSCRIPTION64_WIN11));
                NativeMethods.LocalFree(buffer);

                nSizeUserSubscription = (uint)Marshal.SizeOf(typeof(WNF_USER_SUBSCRIPTION64));
                nSubscriptionsListEntryOffset = (uint)Marshal.OffsetOf(
                    typeof(WNF_USER_SUBSCRIPTION64),
                    "SubscriptionsListEntry").ToInt32();

                if (nameSubscription.Header.NodeTypeCode == Win32Consts.WNF_NODE_NAME_SUBSCRIPTION)
                {
                    pFirstUserSubscription = new IntPtr(nameSubscription.SubscriptionsListHead.Flink - nSubscriptionsListEntryOffset);
                    pUserSubscription = pFirstUserSubscription;

                    while (true)
                    {
                        pCurrentUserSubscription = pUserSubscription;
                        buffer = proc.ReadMemory(pUserSubscription, nSizeUserSubscription);

                        if (buffer == IntPtr.Zero)
                            break;

                        userSubscription = (WNF_USER_SUBSCRIPTION64)Marshal.PtrToStructure(
                            buffer,
                            typeof(WNF_USER_SUBSCRIPTION64));
                        NativeMethods.LocalFree(buffer);
                        pUserSubscription = new IntPtr(userSubscription.SubscriptionsListEntry.Flink - nSubscriptionsListEntryOffset);

                        if (pUserSubscription == pFirstUserSubscription)
                            break;

                        callback = new Dictionary<IntPtr, IntPtr> {
                            { new IntPtr(userSubscription.Callback), new IntPtr(userSubscription.CallbackContext) }
                        };

                        results.Add(
                            pCurrentUserSubscription,
                            callback);
                    }
                }
                else
                {
                    Console.WriteLine("[-] Failed to get valid WNF_NAME_SUBSCRIPTION.");
                }
            }
            else if (proc.GetArchitecture() == "x86")
            {
                nSizeNameSubscription = (uint)Marshal.SizeOf(typeof(WNF_NAME_SUBSCRIPTION32_WIN11));
                WNF_USER_SUBSCRIPTION32 userSubscription;
                buffer = proc.ReadMemory(pNameSubscription, nSizeNameSubscription);

                if (buffer == IntPtr.Zero)
                {
                    Console.WriteLine("[-] Failed to read WNF_NAME_SUBSCRIPTION.");

                    return results;
                }

                var nameSubscription = (WNF_NAME_SUBSCRIPTION32_WIN11)Marshal.PtrToStructure(
                    buffer,
                    typeof(WNF_NAME_SUBSCRIPTION32_WIN11));
                NativeMethods.LocalFree(buffer);

                nSizeUserSubscription = (uint)Marshal.SizeOf(typeof(WNF_USER_SUBSCRIPTION32));
                nSubscriptionsListEntryOffset = (uint)Marshal.OffsetOf(
                    typeof(WNF_USER_SUBSCRIPTION32),
                    "SubscriptionsListEntry").ToInt32();

                if (nameSubscription.Header.NodeTypeCode == Win32Consts.WNF_NODE_NAME_SUBSCRIPTION)
                {
                    pFirstUserSubscription = new IntPtr(nameSubscription.SubscriptionsListHead.Flink - nSubscriptionsListEntryOffset);
                    pUserSubscription = pFirstUserSubscription;

                    while (true)
                    {
                        pCurrentUserSubscription = pUserSubscription;
                        buffer = proc.ReadMemory(pUserSubscription, nSizeUserSubscription);

                        if (buffer == IntPtr.Zero)
                            break;

                        userSubscription = (WNF_USER_SUBSCRIPTION32)Marshal.PtrToStructure(
                            buffer,
                            typeof(WNF_USER_SUBSCRIPTION32));
                        NativeMethods.LocalFree(buffer);
                        pUserSubscription = new IntPtr(userSubscription.SubscriptionsListEntry.Flink - nSubscriptionsListEntryOffset);

                        if (pUserSubscription == pFirstUserSubscription)
                            break;

                        callback = new Dictionary<IntPtr, IntPtr> {
                            { new IntPtr(userSubscription.Callback), new IntPtr(userSubscription.CallbackContext) }
                        };

                        results.Add(
                            pCurrentUserSubscription,
                            callback);
                    }
                }
                else
                {
                    Console.WriteLine("[-] Failed to get valid WNF_NAME_SUBSCRIPTION.");
                }
            }
            else
            {
                Console.WriteLine("[-] Unsupported architecture.");
            }

            return results;
        }


        public static void ListWin11NameSubscriptions(
            PeProcess proc,
            IntPtr pNameSubscription,
            ref Dictionary<ulong, IntPtr> nameSubscriptions)
        {
            uint nSizeNameSubscription;
            uint nNameTableEntryOffset;
            IntPtr pNameSubscriptionLeft;
            IntPtr pNameSubscriptionRight;
            IntPtr buffer;

            if (!proc.IsHeapAddress(pNameSubscription))
                return;

            if (proc.GetArchitecture() == "x64")
            {
                nSizeNameSubscription = (uint)Marshal.SizeOf(typeof(WNF_NAME_SUBSCRIPTION64_WIN11));
                nNameTableEntryOffset = (uint)Marshal.OffsetOf(
                    typeof(WNF_NAME_SUBSCRIPTION64_WIN11),
                    "NamesTableEntry").ToInt32();
                buffer = proc.ReadMemory(pNameSubscription, nSizeNameSubscription);

                if (buffer == IntPtr.Zero)
                    return;

                var entry = (WNF_NAME_SUBSCRIPTION64_WIN11)Marshal.PtrToStructure(
                    buffer,
                    typeof(WNF_NAME_SUBSCRIPTION64_WIN11));

                if (!nameSubscriptions.ContainsKey(entry.StateName))
                    nameSubscriptions.Add(entry.StateName, pNameSubscription);

                if (entry.NamesTableEntry.Left != 0L)
                {
                    pNameSubscriptionLeft = new IntPtr(entry.NamesTableEntry.Left - nNameTableEntryOffset);
                    ListWin11NameSubscriptions(proc, pNameSubscriptionLeft, ref nameSubscriptions);
                }

                if (entry.NamesTableEntry.Right != 0L)
                {
                    pNameSubscriptionRight = new IntPtr(entry.NamesTableEntry.Right - nNameTableEntryOffset);
                    ListWin11NameSubscriptions(proc, pNameSubscriptionRight, ref nameSubscriptions);
                }

                NativeMethods.LocalFree(buffer);
            }
            else if (proc.GetArchitecture() == "x86")
            {
                nSizeNameSubscription = (uint)Marshal.SizeOf(typeof(WNF_NAME_SUBSCRIPTION32_WIN11));
                nNameTableEntryOffset = (uint)Marshal.OffsetOf(
                    typeof(WNF_NAME_SUBSCRIPTION32_WIN11),
                    "NamesTableEntry").ToInt32();
                buffer = proc.ReadMemory(pNameSubscription, nSizeNameSubscription);

                if (buffer == IntPtr.Zero)
                    return;

                var entry = (WNF_NAME_SUBSCRIPTION32_WIN11)Marshal.PtrToStructure(
                    buffer,
                    typeof(WNF_NAME_SUBSCRIPTION32_WIN11));

                if (!nameSubscriptions.ContainsKey(entry.StateName))
                    nameSubscriptions.Add(entry.StateName, pNameSubscription);

                if (entry.NamesTableEntry.Left != 0)
                {
                    pNameSubscriptionLeft = new IntPtr(entry.NamesTableEntry.Left - nNameTableEntryOffset);
                    ListWin11NameSubscriptions(proc, pNameSubscriptionLeft, ref nameSubscriptions);
                }

                if (entry.NamesTableEntry.Right != 0)
                {
                    pNameSubscriptionRight = new IntPtr(entry.NamesTableEntry.Right - nNameTableEntryOffset);
                    ListWin11NameSubscriptions(proc, pNameSubscriptionRight, ref nameSubscriptions);
                }

                NativeMethods.LocalFree(buffer);
            }
        }
    }
}
