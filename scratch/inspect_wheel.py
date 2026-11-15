import pefile
import capstone

# Read GameAssembly.dll
with open(r"TGLL_minCPU_v2\GameAssembly.dll", "rb") as f:
    dll_data = f.read()

# PE sections
pe = pefile.PE(data=dll_data)
def rva_to_offset(rva):
    for section in pe.sections:
        if section.VirtualAddress <= rva < section.VirtualAddress + section.Misc_VirtualSize:
            return section.PointerToRawData + (rva - section.VirtualAddress)
    return rva

md = capstone.Cs(capstone.CS_ARCH_X86, capstone.CS_MODE_64)

for rva in [0x374AD0, 0x4E4070, 0x495740]:
    offset = rva_to_offset(rva)
    code = dll_data[offset:offset+120]
    print(f"=== Disassembly at RVA 0x{rva:X} (offset 0x{offset:X}) ===")
    for ins in md.disasm(code, rva):
        print(f"  0x{ins.address:08X}: {ins.mnemonic} {ins.op_str}")
