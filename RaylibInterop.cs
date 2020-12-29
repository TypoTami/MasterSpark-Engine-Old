using System;
using System.Runtime.InteropServices;
using System.Text;

// This class is pretty much copypasted from this link:
// https://github.com/videolan/libvlcsharp/blob/ff246a39fb9e35541357ffcd90062a8c0cc431f6/src/LibVLCSharp/Shared/Helpers/MarshalUtils.cs#L66
public static class RaylibInterop {
    
    /// <summary>
    /// This is EXTREMELY hacky and is, overall a huge mess, the entire class was needed just for this dumb method.
    /// This properly formats a string with the va_list IntPtr given, in a cross platform way with no errors.
    /// This won't work with UTF-16 characters and will only format them as "?"
    /// </summary>
    /// <param name="format">A pointer to the format string.</param>
    /// <param name="args">A pointer to the arguments.</param>
    /// <returns></returns>
    public static string GetLogMessage(IntPtr format, IntPtr args) {
#if APPLE
            return AppleLogCallback(format, args);
#else
        // Special marshalling is needed on Linux desktop 64 bits.
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && IntPtr.Size == 8) {
            return LinuxX64LogCallback(format, args);
        }

        var byteLength = VsnPrintF(IntPtr.Zero, UIntPtr.Zero, format, args) + 1;
        if (byteLength <= 1) return string.Empty;

        var buffer = IntPtr.Zero;
        try {
            buffer = Marshal.AllocHGlobal(byteLength);
            VsPrintF(buffer, format, args);
            return buffer.FromUtf8()!;
        } finally {
            Marshal.FreeHGlobal(buffer);
        }
#endif
    }
    
    private readonly struct Native {
        [DllImport("libSystem", EntryPoint = "vasprintf", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vasprintf_apple(ref IntPtr buffer, IntPtr format, IntPtr args);

        [DllImport("libc", EntryPoint = "vsprintf", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vsprintf_linux(IntPtr buffer, IntPtr format, IntPtr args);

        [DllImport("msvcrt", EntryPoint = "vsprintf", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vsprintf_windows(IntPtr buffer, IntPtr format, IntPtr args);

        [DllImport("libc", EntryPoint = "vsnprintf", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vsnprintf_linux(IntPtr buffer, UIntPtr size, IntPtr format, IntPtr args);

        [DllImport("msvcrt", EntryPoint = "vsnprintf", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vsnprintf_windows(IntPtr buffer, UIntPtr size, IntPtr format, IntPtr args);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    private struct VaListLinuxX64 {
        private readonly uint   gp_offset;
        private readonly uint   fp_offset;
        private readonly IntPtr overflow_arg_area;
        private readonly IntPtr reg_save_area;
    }

    // Helpers for marshalling UTF-8
    internal static IntPtr ToUtf8(this string? str) {
        if (str == null) return IntPtr.Zero;

        var bytes        = Encoding.UTF8.GetBytes(str);
        var nativeString = Marshal.AllocHGlobal(bytes.Length + 1);
        try {
            Marshal.Copy(bytes, 0, nativeString, bytes.Length);
            Marshal.WriteByte(nativeString, bytes.Length, 0);
        } catch (Exception) {
            Marshal.FreeHGlobal(nativeString);
            throw;
        }

        return nativeString;
    }

    private static string? FromUtf8(this IntPtr nativeString) {
        if (nativeString == IntPtr.Zero) return null;

        var length = 0;

        while (Marshal.ReadByte(nativeString, length) != 0) length++;

        var buffer = new byte[length];
        Marshal.Copy(nativeString, buffer, 0, buffer.Length);
        return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
    }

    // Helpers for marshalling va_list
    private static string AppleLogCallback(IntPtr format, IntPtr args) {
        var buffer = IntPtr.Zero;
        try {
            var count = Native.vasprintf_apple(ref buffer, format, args);

            if (count == -1) return string.Empty;

            return buffer.FromUtf8() ?? string.Empty;
        } finally {
            Marshal.FreeHGlobal(buffer);
        }
    }

    private static string LinuxX64LogCallback(IntPtr format, IntPtr args) {
        // The args pointer cannot be reused between two calls. We need to make a copy of the underlying structure.
        var listStructure = Marshal.PtrToStructure<VaListLinuxX64>(args);
        var byteLength    = 0;
        UseStructurePointer(listStructure, listPointer => {
            byteLength = Native.vsnprintf_linux(IntPtr.Zero, UIntPtr.Zero, format, listPointer) + 1;
        });

        var utf8Buffer = IntPtr.Zero;
        try {
            utf8Buffer = Marshal.AllocHGlobal(byteLength);

            return UseStructurePointer(listStructure, listPointer => {
                Native.vsprintf_linux(utf8Buffer, format, listPointer);
                return utf8Buffer.FromUtf8()!;
            });
        } finally {
            Marshal.FreeHGlobal(utf8Buffer);
        }
    }

    private static void UseStructurePointer<T>(T structure, Action<IntPtr> action) {
        if (structure == null) throw new ArgumentNullException(nameof(structure));
        
        var structurePointer = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
        try {
            Marshal.StructureToPtr(structure, structurePointer, false);
            action(structurePointer);
        } finally {
            Marshal.FreeHGlobal(structurePointer);
        }
    }

    private static string UseStructurePointer<T>(T structure, Func<IntPtr, string> action) {
        if (structure == null) throw new ArgumentNullException(nameof(structure));
        
        var structurePointer = IntPtr.Zero;
        try {
            structurePointer = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
            Marshal.StructureToPtr(structure, structurePointer, false);
            return action(structurePointer);
        } finally {
            Marshal.FreeHGlobal(structurePointer);
        }
    }

    private static int VsnPrintF(IntPtr buffer, UIntPtr size, IntPtr format, IntPtr args) {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return Native.vsnprintf_windows(buffer, size, format, args);
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return Native.vsnprintf_linux(buffer, size, format, args);
        return -1;
    }

    private static int VsPrintF(IntPtr buffer, IntPtr format, IntPtr args) {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return Native.vsprintf_windows(buffer, format, args);
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return Native.vsprintf_linux(buffer, format, args);
        return -1;
    }
}

