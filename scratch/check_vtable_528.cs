using System;
using System.IO;

public class Program {
    public static void Main() {
        byte[] b = File.ReadAllBytes(@"TGLL_minCPU_v2\GameAssembly.dll");
        // Lucky wheel class vtable was around 0x16C12A0
        // Let's find where 0x501420 is: it was at file 0x16C12A8
        // Let's find the vtable start of this class:
        // In il2cpp, the class has a vtable array.
        // Let's print 50 pointers before and after 0x16C12A8:
        for (int i = -0x100; i <= 0x600; i += 8) {
            int off = 0x16C12A8 + i;
            long ptr = BitConverter.ToInt64(b, off);
            long rva = ptr - 0x180000000L;
            if (i == 0x528 || i == 0x520 || i == 0x530) {
                Console.WriteLine(string.Format("Relative {0:+0000;-0000} (file 0x{1:X}): RVA 0x{2:X}", i, off, rva));
            }
        }
    }
}

